using Cinar.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancerSyncronizer
{
    public partial class Form1 : Form
    {
        public static API DATA;

        public Stopwatch s = new Stopwatch();
        private const string compressedFileName = "\\files.tar";
        private List<string> fileNames = new List<string>();
        private List<string> errorLines = new List<string>();
        private int ServerId;
        private static double totalnumOfFiles;
        private static double numOfprocessedFiles;
        private static double percentageCheckPoint;
        private static double percentageStop = 33;

        public Form1()
        {
            DATA = FileSerializer.Load();
            InitializeComponent();

            //init button names..
            btnDbSettings.Text = DATA.ConnectionString.Item1;
            btnClone1Settings.Text = DATA.CloneServers[0].Item1;
            btnClone2Settings.Text = DATA.CloneServers[1].Item1;
            btnClone3Settings.Text = DATA.CloneServers[2].Item1;
            btnClone4Settings.Text = DATA.CloneServers[3].Item1;
            btnMainServer.Text = DATA.MainServer.Item1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showMessageAsync("Set database connection!");

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Override");
            cm.MenuItems[0].Click += RightClickOverride;


            btnClone1Settings.ContextMenu = btnClone2Settings.ContextMenu = cm;
            btnClone3Settings.ContextMenu = btnClone4Settings.ContextMenu = cm;

        }

        #region Button Bindings



        private void RightClickOverride(object sender, EventArgs e)
        {
            var tsItem = (MenuItem)sender;
            var menu = (ContextMenu)tsItem.Parent;
            AccessibleObject cms = menu.SourceControl.AccessibilityObject;

            Task.Run(() =>
            {
                //Get server Id from right clicked button
                ServerId = DATA.CloneServers.Select(x => x.Item1).IndexOf(x => x == cms.Name);

                StartCopyAll();

                //Checkpoint 1
                ControlExtension.percentage = percentageCheckPoint = 0;
                TarMainServer();

                //Checkpoint 2
                ControlExtension.percentage = percentageCheckPoint = 33;
                StartCopy(ServerId);

                //Checkpoint 3
                ControlExtension.percentage = percentageCheckPoint = 66;
                extractTarFile(ServerId);

                EndCopyAll();
            });
        }


        private void btnDbSettings_Click(object sender, EventArgs e)
        {
            string title = "Database Settings";

            PromptResult res = Prompt.ShowDialog("Enter connection string:", title, true, DATA.ConnectionString);

            if (res.closeOperation == DialogResult.OK && !isDBConnected(res))
            {
                res = Prompt.ShowDialog("Couldn't connect, check your connection and string then try again:", title, true, DATA.ConnectionString);
            }
            if (res.closeOperation == DialogResult.OK)
            {
                infoSyncStatusLabel.Text = "Connected to DB!";
                DATA.ConnectionString = res.inputValue;
                (sender as Button).Text = res.inputValue.Item1;
                DATA.ConnectionType = res.DbConnectionType;
                DATA.Save();

                //run background operation
                if (checkDatabaseForFileChanges.IsBusy)
                    checkDatabaseForFileChanges.CancelAsync();
                Task.Run(() => {
                    while (checkDatabaseForFileChanges.CancellationPending)
                        Thread.Sleep(100);
                        checkDatabaseForFileChanges.RunWorkerAsync();
                });
            }
        }

        private bool isDBConnected(PromptResult res)
        {
            string version = "";
            try
            {
                version = new Database(res.inputValue.Item2, res.DbConnectionType).GetVersion();
            }
            catch
            {
                return false;
            }

            return version != "";
        }


        private void btnBackgroundSync_Click(object sender, EventArgs e)
        {
            string title = "Server roots List";
            IncrementablePromptResult res = IncrementablePrompt.ShowDialog("Enter server root paths:", title, DATA.ServerRoots);
            string failedPath = "";
            while (!filesOK(res.inputValues, out failedPath) && res.closeOperation == DialogResult.OK)
            {
                res = IncrementablePrompt.ShowDialog("Path: " + failedPath + " does not exist! Try again!", title, res.inputValues);
            }

            if (res.closeOperation == DialogResult.OK)
            {
                DATA.ServerRoots = res.inputValues;
                DATA.Save();
            }
        }

        private void btnMainServer_Click(object sender, EventArgs e)
        {
            promptServerAddress("Main Server Settings", int.MaxValue, sender);
        }

        private void btnClone1Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 1 Settings", 0, sender);
        }

        private void btnClone2Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 2 Settings", 1, sender);
        }

        private void btnClone3Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 3 Settings", 2, sender);
        }

        private void btnClone4Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 4 Settings", 3, sender);
        }
        
        private static void promptServerAddress(string title, int i = int.MaxValue, object sender = null)
        {
            PromptResult res = Prompt.ShowDialog("Enter server root path:", title, false, i == int.MaxValue ? DATA.MainServer : DATA.CloneServers[i]);
            if (res.closeOperation == DialogResult.OK)
            {
                while (checkFile(res.inputValue.Item2) && res.closeOperation == DialogResult.OK)
                {
                    res = Prompt.ShowDialog("File path does not exist! Enter again:", title, false, i == int.MaxValue ? DATA.MainServer : DATA.CloneServers[i]);
                }
                if (res.closeOperation == DialogResult.OK)
                {
                    if (i == int.MaxValue) DATA.MainServer = res.inputValue;
                    else DATA.CloneServers[i] = res.inputValue;
                    (sender as Button).Text = res.inputValue.Item1;
                    DATA.Save();
                }
            }
        }


        private static bool checkFile(string path)
        {
            return (string.IsNullOrEmpty(path.Trim()) || !(new DirectoryInfo(path).Exists));
        }

        private static bool filesOK(List<string> paths, out string failedPath)
        {
            foreach (string path in paths)
                if (!(new DirectoryInfo(path).Exists))
                {
                    failedPath = path;
                    return false;
                }
            failedPath = "";
            return true;
        }



        #endregion

        #region Sync Database Files Async



        private void checkDatabaseForFileChanges_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                SyncNewFiles();
                System.Threading.Thread.Sleep(10000);
            }

        }

        private void SyncNewFiles()
        {
            try
            {
                List<ApplicationSyncPath> paths =
                    Provider.Database.ReadList<ApplicationSyncPath>(
                        FilterExpression.Create("isSynced", CriteriaTypes.Eq, false).And("PublishTime", CriteriaTypes.Lt, DateTime.Now)
                    );
                if (paths.Count != 0)
                {
                    btnBackgroundSync.ThreadSafeInvoke(() => btnBackgroundSync.Enabled = false);
                    ControlExtension.backgroundPercentage = 0;
                    infoSyncStatusProgress.UpdateStatus(statusStrip1);
                    paths.ForEach(x =>
                    {
                        statusStripSafe(() => infoSyncStatusLabel.Text = "Syncing: " + x.path);
                        for (int i = 1; i < DATA.ServerRoots.Count; i++)
                        {
                            string serverPath = DATA.ServerRoots[i];
                            ControlExtension.backgroundPercentage += 100.0 / (paths.Count * DATA.CloneServers.Count);
                            infoSyncStatusProgress.UpdateStatus(statusStrip1);
                            if (!CopyFileFromFileToFile(DATA.ServerRoots[0] + x.path, serverPath + x.path, x))
                            {
                                statusStripSafe(() => infoSyncStatusLabel.Text = "Failed!");
                                continue;
                            }
                            statusStripSafe(() => infoSyncStatusLabel.Text = "Sync is done!");
                        }

                        if (DATA.ServerRoots.Count == 0)
                            statusStripSafe(() => infoSyncStatusLabel.Text = "Enter server root paths!");
                        else
                        {
                            x.isSynced = true;
                            x.Save();
                        }

                    });
                    btnBackgroundSync.ThreadSafeInvoke(() => btnBackgroundSync.Enabled = true);
                    ControlExtension.backgroundPercentage = 100;
                    infoSyncStatusProgress.UpdateStatus(statusStrip1);
                }
                else
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(5000);
                        statusStripSafe(() => infoSyncStatusLabel.Text = "No files to sync!");

                        statusStrip1.ThreadSafeInvoke(() => infoSyncStatusProgress.Value = 0);
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this.ThreadSafeInvoke(() => this.Close());
            }

        }

        private void statusStripSafe(MethodInvoker method)
        {
            statusStrip1.ThreadSafeInvoke(() => method.Invoke());
        }

        private bool CopyFileFromFileToFile(string srcName, string destName, ApplicationSyncPath x)
        {
            FileInfo sourceFile = new FileInfo(Path.GetFullPath(srcName));

            if (!sourceFile.Exists)
            {
                x.ErrorMessage = srcName + " does not exist!";
                return false;
            }

            FileInfo destFile = new FileInfo(destName);

            Directory.CreateDirectory(Path.GetDirectoryName(destFile.FullName));

            File.Copy(sourceFile.FullName, destFile.FullName, true);

            return true;
        }



        #endregion

        #region Override whole main server to clones



        private void StartCopyAll()
        {
            s.Reset();
            s.Start();

            InitializeUI();

            //Get number of files to calculate percentage of process...
            totalnumOfFiles = Directory.EnumerateFiles(DATA.MainServer.Item2, "*", SearchOption.AllDirectories).Count();

            //Init for percentage
            numOfprocessedFiles = 0;
        }

        private void InitializeUI()
        {
            resetButtonsBackColor();
            setButtonsEnable(false);
        }

        private void TarMainServer()
        {
            infoTotalFilesCopied.ThreadSafeSetText("Started compressing.");
            executeCommand("7z a -ttar " + DATA.MainServer.Item2 + compressedFileName +" " + DATA.MainServer.Item2 + "\\*");
            infoProgressTotalFilesCopied.UpdateStatus();
        }

        private void extractTarFile(int index)
        {
            string command = "7z x -y " + DATA.CloneServers[index].Item2 + compressedFileName + " -o" + DATA.CloneServers[index].Item2 + " -r -aoa";
            infoTotalFilesCopied.ThreadSafeSetText("Started extracting to: " + DATA.CloneServers[index].Item2);
            executeCommand(command);
            setServerCloneIsDoneColor(index);
        }

        private void StartCopy(int index)
        {
            numOfprocessedFiles = 0;
            setServerCloneIsProcesingColor(index);
            copyCompressedFile(DATA.CloneServers[index].Item2);
            ControlExtension.percentage = 66;
            infoProgressTotalFilesCopied.UpdateStatus();
        }

        private void EndCopyAll()
        {
            CreateOutput();
            CreateErrorFileIfAny();
            RemoveTarFileFromMainServer();
            Finish();
        }

        private void CreateOutput()
        {
            //TODO: file lari ust uste yazdirma listeyi UI da goster.
            string outputFileName = "output" + ServerId + ".txt";
            infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Text = "Output file:\r\n" + AppDomain.CurrentDomain.BaseDirectory + outputFileName);
            File.WriteAllLines(outputFileName, fileNames);
        }

        private void CreateErrorFileIfAny()
        {
            if (!string.IsNullOrWhiteSpace(errorLines.StringJoin().Trim()))
            {
                string errorFileName = "error" + ServerId + ".txt";
                infoFilesOverriden.ThreadSafePrependText("Errors occured and stored at:" + Environment.NewLine
                    + AppDomain.CurrentDomain.BaseDirectory + errorFileName + Environment.NewLine);
                File.WriteAllLines(errorFileName, errorLines);
            }
        }

        private void RemoveTarFileFromMainServer()
        {
            File.Delete(DATA.MainServer.Item2 + compressedFileName);
        }

        private void Finish()
        {
            s.Stop();
            infoTotalFilesCopied.ThreadSafeSetText("Completed in " + TimeSpan.FromMilliseconds(s.ElapsedMilliseconds).Seconds + " second/s.");

            //Last Checkpoint for status bar
            ControlExtension.percentage = 100;
            infoProgressTotalFilesCopied.UpdateStatus();

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                setButtonsEnable(true);
                resetButtonsBackColor();
            });
        }



        #endregion

        #region Execute Command



        private void executeCommand(string withCommand)
        {
            string exec = withCommand.Split(' ').First();
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Environment.GetEnvironmentVariable("comspec");
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
            startInfo.ErrorDialog = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            numOfprocessedFiles = 0;

            try
            {
                Process exeProcess = new Process();
                exeProcess.StartInfo = startInfo;

                // Hook up async stdout and stderr reading
                exeProcess.OutputDataReceived += exeProcess_OutputDataReceived;
                exeProcess.ErrorDataReceived += exeProcess_ErrorDataReceived;

                // Execute the process
                if (exeProcess.Start())
                {
                    // Begin async stdout and stderr reading
                    exeProcess.BeginOutputReadLine();
                    exeProcess.BeginErrorReadLine();

                    exeProcess.StandardInput.WriteLine("cd 7zip");
                    exeProcess.StandardInput.WriteLine(withCommand);
                    exeProcess.StandardInput.WriteLine("exit");
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

        private void exeProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            numOfprocessedFiles++;
            ControlExtension.percentage = percentageCheckPoint + (int)((numOfprocessedFiles / totalnumOfFiles) * percentageStop);
            infoProgressTotalFilesCopied.UpdateStatus();
            errorLines.Add(e.Data);
        }

        private void exeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            numOfprocessedFiles++;
            ControlExtension.percentage = percentageCheckPoint + (int)((numOfprocessedFiles / totalnumOfFiles) * percentageStop);
            infoProgressTotalFilesCopied.UpdateStatus();
            fileNames.Add(e.Data);
        }



        #endregion

        #region Copy Compressed File In Big Chunks



        private void copyCompressedFile(string toDirectory)
        {

            string srcName = DATA.MainServer.Item2 + compressedFileName;
            string destName = toDirectory + compressedFileName;
            FileInfo sourceFile = new FileInfo(srcName);
            int buflen = 4 * 1024 * 1024; //4 MB buffer
            byte[] buf = new byte[buflen];
            long totalBytesRead = 0;
            double pctDone = 0;
            int numReads = 0;

            using (FileStream sourceStream = new FileStream(srcName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream destStream = new FileStream(destName, FileMode.Create, FileAccess.Write))
                {
                    while (true)
                    {
                        numReads++;
                        int bytesRead = sourceStream.Read(buf, 0, buflen);
                        if (bytesRead == 0) break;
                        destStream.Write(buf, 0, bytesRead);

                        totalBytesRead += bytesRead;
                        
                        pctDone = (double)(((double)totalBytesRead / (double)sourceFile.Length) * percentageStop);
                        infoTotalFilesCopied.ThreadSafeSetText("Copying to directory: " + toDirectory + ", " + (int)pctDone * (100/percentageStop) + "% done!");
                        ControlExtension.percentage = percentageCheckPoint + (int)pctDone;
                        infoProgressTotalFilesCopied.UpdateStatus();

                        if (bytesRead < buflen) break;

                    }
                }
            }
        }

        private void setButtonsEnable(bool isEnabled)
        {
            btnMainServer.ThreadSafeInvoke(() => btnMainServer.Enabled = isEnabled);
            btnClone1Settings.ThreadSafeInvoke(() => btnClone1Settings.Enabled = isEnabled);
            btnClone2Settings.ThreadSafeInvoke(() => btnClone2Settings.Enabled = isEnabled);
            btnClone3Settings.ThreadSafeInvoke(() => btnClone3Settings.Enabled = isEnabled);
            btnClone4Settings.ThreadSafeInvoke(() => btnClone4Settings.Enabled = isEnabled);
        }

        private void setServerCloneIsProcesingColor(int i)
        {
            Color processColor = Color.Yellow;

            setButtonColorAt(i, processColor);
        }

        private void resetButtonsBackColor()
        {
            Color back = Color.FromKnownColor(KnownColor.Control);
            btnClone1Settings.BackColor = btnClone2Settings.BackColor = btnClone3Settings.BackColor = btnClone4Settings.BackColor = back;
        }

        private void setServerCloneIsDoneColor(int i)
        {
            Color doneColor = Color.GreenYellow;

            setButtonColorAt(i, doneColor);
        }

        private void setButtonColorAt(int i, Color doneColor)
        {
            switch (i)
            {
                case 0:
                    btnClone1Settings.BackColor = doneColor;
                    break;
                case 1:
                    btnClone2Settings.BackColor = doneColor;
                    break;
                case 2:
                    btnClone3Settings.BackColor = doneColor;
                    break;
                case 3:
                    btnClone4Settings.BackColor = doneColor;
                    break;
                default:
                    break;
            }
        }



        #endregion

        #region Utility Functions



        public static void showMessageAsync(string message)
        {
            Task.Run(() => MessageBox.Show(message, "Asynchronous Message"));
        }


        #endregion
    }

    #region Control Extentions
    public static class ControlExtension
    {

        private static List<string> isInitialized = new List<string>();
        public static double percentage;
        public static double backgroundPercentage;


        private delegate void AppendText(Control control, string text);
        private static void Append(this Control control, string text)
        {
            control.Text = control.Text + text;
        }

        private delegate void PrependText(Control control, string text);
        private static void Prepend(this Control control, string text)
        {
            control.Text = text + control.Text;
        }

        private delegate void SetText(Control control, string text);
        private static void Set(this Control control, string text)
        {
            control.Text = text;
        }

        public static void ThreadSafeInvoke(this Control control, MethodInvoker method)
        {
            if (control != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(method);
                }
                else
                {
                    method.Invoke();
                }
            }
        }

        public static void ThreadSafeAppendText(this Control control, string text)
        {
            if (control != null)
            {
                AppendText handler = Append;
                if (control.InvokeRequired)
                {
                    control.Invoke(handler, control, text);
                }
                else
                {
                    handler.Invoke(control, text);
                }
            }
        }

        public static void ThreadSafePrependText(this Control control, string text)
        {
            if (control != null)
            {
                PrependText handler = Prepend;
                if (control.InvokeRequired)
                {
                    control.Invoke(handler, control, text);
                }
                else
                {
                    handler.Invoke(control, text);
                }
            }
        }

        public static void ThreadSafeSetText(this Control control, string text)
        {
            if (control != null)
            {
                SetText handler = Set;
                if (control.InvokeRequired)
                {
                    control.Invoke(handler, control, text);
                }
                else
                {
                    handler.Invoke(control, text);
                }
            }
        }

        public static void addPlaceHolder(this TextBox txtBox, string placeholder)
        {
            txtBox.Tag = placeholder;

            //init if empty string
            if (txtBox.Text == "")
            {
                txtBox.ForeColor = Color.Gray;
                txtBox.Text = txtBox.Tag.ToString();
            }

            txtBox.GotFocus += (sender, e) =>
            {
                if (txtBox.Text == txtBox.Tag.ToString())
                {
                    txtBox.ForeColor = Color.Black;
                    txtBox.Text = "";
                }
            };

            txtBox.LostFocus += (sender, e) =>
            {
                if (txtBox.Text == "")
                {
                    txtBox.ForeColor = Color.Gray;
                    txtBox.Text = txtBox.Tag.ToString();
                }
            };
        }

        public static void UpdateStatus(this ProgressBar progress)
        {
            progress.ThreadSafeInvoke(() =>
            {
                if (percentage < 1) percentage = 1;
                if (percentage > 100) percentage = 100;

                progress.initProgressBar();
                progress.Maximum = (int)((double)progress.Value / (double)(percentage) * 100);
            });
        }

        public static void initProgressBar(this ProgressBar progress)
        {
            if (!isInitialized.Contains(progress.Name))
            {
                progress.Maximum *= 100;
                progress.Value = progress.Maximum / 100;
                isInitialized.Add(progress.Name);
            }
        }

        public static void UpdateStatus(this ToolStripProgressBar progress, StatusStrip s)
        {
            s.ThreadSafeInvoke(() =>
            {
                if (backgroundPercentage < 1) backgroundPercentage = 1;
                if (backgroundPercentage > 100) backgroundPercentage = 100;

                initProgressBar(progress);
                progress.Maximum = (int)((double)progress.Value / (double)(backgroundPercentage) * 100);
            });
        }

        public static void initProgressBar(this ToolStripProgressBar progress)
        {
            if (!isInitialized.Contains(progress.Name))
            {
                progress.Maximum *= 100;
                progress.Value = progress.Maximum / 100;
                isInitialized.Add(progress.Name);
            }
        }
    }
    #endregion

}
