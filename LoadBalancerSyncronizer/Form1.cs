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
        public static double numOfArchivedFile;
        public static double tempNumOfArchivedFile;
        public Stopwatch s = new Stopwatch();
        private const string compressedFileName = "\\files.tar";
        private List<string> isInitialized = new List<string>();
        private List<string> fileNames = new List<string>();
        private List<string> errorLines = new List<string>();

        public Form1()
        {
            DATA = FileSerializer.Load();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showMessageAsync("Set database connection!");
        }

        #region Button Bindings



        private void overrideServers_Click(object sender, EventArgs e)
        {
            copyAllAsync.RunWorkerAsync();
        }

        private void btnMainServer_Click(object sender, EventArgs e)
        {
            promptServerAddress("Main Server Settings");
        }

        private void btnClone1Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 1 Settings", 0);
        }

        private void btnClone2Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 2 Settings", 1);
        }

        private void btnClone3Settings_Click(object sender, EventArgs e)
        {
            promptServerAddress("Server Clone 3 Settings", 2);
        }

        private static void promptServerAddress(string title, int i = int.MaxValue)
        {
            PromptResult res = Prompt.ShowDialog("Enter server root path:", title, false, i == int.MaxValue ? DATA.mainServer : DATA.cloneServers[i]);

            while (checkFile(res) && res.closeOperation == DialogResult.OK)
            {
                res = Prompt.ShowDialog("File path does not exist! Enter again:", title, false, i == int.MaxValue ? DATA.mainServer : DATA.cloneServers[i]);
            }
            if (!checkFile(res))
            {
                if (i == int.MaxValue) DATA.mainServer = res.inputValue;
                else DATA.cloneServers[i] = res.inputValue;
                DATA.Save();
            }
        }

        private static bool checkFile(PromptResult res)
        {
            return (string.IsNullOrEmpty(res.inputValue.Trim()) || !(new DirectoryInfo(res.inputValue).Exists));
        }


        private void btnDbSettings_Click(object sender, EventArgs e)
        {
            string title = "Database Settings";

            PromptResult res = Prompt.ShowDialog("Enter connection string:", title, true, DATA.ConnectionString);

            if (res.closeOperation == DialogResult.OK && !isDBConnected(res))
            {
                MessageBox.Show("Couldn't connect to database! Background process is not started. \nPlease enter your connection string right!");
                this.Close();
            }
            if (isDBConnected(res))
            {
                infoSyncStatusLabel.Text = "Connected to DB!";
                DATA.ConnectionString = res.inputValue;
                DATA.ConnectionType = res.DbConnectionType;
                DATA.Save();
            }

            //run background operation
            if (!checkDatabaseForFileChanges.IsBusy)
                checkDatabaseForFileChanges.RunWorkerAsync();
        }

        private bool isDBConnected(PromptResult res)
        {
            string version = "";
            try
            {
                version = new Database(res.inputValue, res.DbConnectionType).GetVersion();
            }
            catch
            {
                return false;
            }

            return version != "";
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
                List<SyncronizedFilePath> paths = Provider.Database.ReadList<SyncronizedFilePath>(FilterExpression.Create("isSynced", CriteriaTypes.Eq, false));
                if (paths.Count != 0)
                {
                    double i = 0;
                    setProggressBarTo(infoSyncStatusProgress, (int)i);
                    paths.ForEach(x =>
                    {
                        infoSyncStatusLabel.Text = "Syncing: " + x.path;
                        foreach (string serverPath in DATA.cloneServers)
                        {
                            i += 100.0 / (paths.Count * DATA.cloneServers.Length);
                            setProggressBarTo(infoSyncStatusProgress, (int)i);
                            if (!CopyFileFromFileToFile(DATA.mainServer + x.path, serverPath + x.path, x))
                            {
                                infoSyncStatusLabel.Text = "Failed!";
                                break;
                            }
                        }
                        infoSyncStatusLabel.Text = DATA.mainServer + x.path + " synced!";
                        x.isSynced = true;
                        x.Save();
                    });
                    infoSyncStatusLabel.Text = "Sync done!";
                    statusStrip1.ThreadSafeInvoke(() => infoSyncStatusProgress.Value = 0);
                }
                else
                {
                    Task.Run(() => {
                        Thread.Sleep(5000);
                        infoSyncStatusLabel.Text = "No files to sync!"; 
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this.ThreadSafeInvoke(() => this.Close());
            }

        }

        private bool CopyFileFromFileToFile(string srcName, string destName, SyncronizedFilePath x)
        {
            FileInfo sourceFile = new FileInfo(Path.GetFullPath(srcName));

            if (!sourceFile.Exists)
            {
                x.ErrorMessage = "Source file " + srcName + " does not exist";
                return false;
            }

            FileInfo destFile = new FileInfo(destName);

            File.Copy(sourceFile.FullName, destFile.FullName, true);

            return true;
        }

        private void setProggressBarTo(ProgressBar progress, int value)
        {
            progress.ThreadSafeInvoke(() =>
            {
                if (value < 1) value = 1;
                if (value > 100) value = 100;

                initProgressBar(progress);
                progress.Maximum = (int)((double)progress.Value / (double)(value) * 100);
            });
        }

        private void initProgressBar(ProgressBar progress)
        {
            if (!isInitialized.Contains(progress.Name))
            {
                progress.Maximum *= 100;
                progress.Value = progress.Maximum / 100;
                isInitialized.Add(progress.Name);
            }
        }



        #endregion

        #region Override whole main server to clones



        private void copyAllAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            s.Reset();
            s.Start();

            InitializeUI();
            TarMainServer();

            //When files are archived into TAR in main server,
            //take the count. So that when its extracted, 
            //percentage will be calculatable.
            numOfArchivedFile = fileNames.Count;

            for (int i = 0; i < DATA.cloneServers.Length; i++)
                ExtractToCloneServer(i);

            CreateOutput();
            CreateErrorFileIfAny();

            Finish();
        }

        private void Finish()
        {
            File.Delete(DATA.mainServer + compressedFileName);

            s.Stop();
            infoTotalFilesCopied.ThreadSafeSetText("Completed in " + TimeSpan.FromMilliseconds(s.ElapsedMilliseconds).Seconds + " second/s.");

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                setButtonsEnable(true);
                resetButtonsBackColor();
            });
        }

        private void CreateErrorFileIfAny()
        {
            if (!string.IsNullOrWhiteSpace(errorLines.StringJoin().Trim()))
            {
                infoFilesOverriden.ThreadSafePrependText("Errors occured and stored at:" + Environment.NewLine
                    + AppDomain.CurrentDomain.BaseDirectory + "errors.txt" + Environment.NewLine);
                File.WriteAllLines("errors.txt", errorLines);
            }
        }

        private void CreateOutput()
        {
            //TODO: file lari ust uste yazdirma listeyi UI da goster.
            infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Text = "Output file:\r\n" + AppDomain.CurrentDomain.BaseDirectory + "output.txt");
            File.WriteAllLines("output.txt", fileNames);
        }

        private void ExtractToCloneServer(int i)
        {
            string command = "7z x -y " + DATA.cloneServers[i] + "\\files.tar -o" + DATA.cloneServers[i] + " -r -aoa";
            tempNumOfArchivedFile = 0;
            setProggressBarTo(infoProgressTotalFilesCopied, 0);
            setServerCloneIsProcesingColor(i);
            copyZippedFile(DATA.cloneServers[i]);
            infoTotalFilesCopied.ThreadSafeSetText("Started extracting to: " + DATA.cloneServers[i]);
            executeCommand(command);
            setServerCloneIsDoneColor(i);
        }

        private void TarMainServer()
        {
            infoTotalFilesCopied.ThreadSafeSetText("Started compressing.");
            executeCommand("7z a -ttar " + DATA.mainServer + "\\files.tar " + DATA.mainServer + "\\*", true);
        }

        private void InitializeUI()
        {
            resetButtonsBackColor();
            setButtonsEnable(false);
            btnOverrideServers.ThreadSafeInvoke(() => btnOverrideServers.Enabled = false);
        }


        #endregion

        #region Execute Command



        private void executeCommand(string withCommand, bool isOutput = false)
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
            tempNumOfArchivedFile = 0;

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
            errorLines.Add(e.Data);
        }

        private void exeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            tempNumOfArchivedFile++;
            setProggressBarTo(infoProgressTotalFilesCopied, (int)((tempNumOfArchivedFile / numOfArchivedFile) * 50) + 50);
            fileNames.Add(e.Data);
        }



        #endregion

        #region Copy Compressed File In Big Chunks



        private void copyZippedFile(string toDirectory)
        {

            string srcName = DATA.mainServer + compressedFileName;
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
                        int totalPercentage = 50;
                        numReads++;
                        int bytesRead = sourceStream.Read(buf, 0, buflen);
                        if (bytesRead == 0) break;
                        destStream.Write(buf, 0, bytesRead);

                        totalBytesRead += bytesRead;
                        if (numReads % 10 == 0)
                        {
                            pctDone = (double)((double)totalBytesRead / (double)sourceFile.Length) * totalPercentage;
                            infoTotalFilesCopied.ThreadSafeSetText("Copying to directory: " + toDirectory + ", " + (int)pctDone + "% done!");
                            infoProgressTotalFilesCopied.ThreadSafeInvoke(() => setProggressBarTo(infoProgressTotalFilesCopied, (int)pctDone + 1));
                        }

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
        }

        private void setServerCloneIsProcesingColor(int i)
        {
            Color processColor = Color.Yellow;

            switch (i)
            {
                case 0:
                    btnClone1Settings.BackColor = processColor;
                    break;
                case 1:
                    btnClone2Settings.BackColor = processColor;
                    break;
                case 2:
                    btnClone3Settings.BackColor = processColor;
                    break;
                default:
                    break;
            }
        }

        private void resetButtonsBackColor()
        {
            Color back = Color.FromKnownColor(KnownColor.Control);
            btnClone1Settings.BackColor = btnClone2Settings.BackColor = btnClone3Settings.BackColor = back;
        }

        private void setServerCloneIsDoneColor(int i)
        {
            Color doneColor = Color.GreenYellow;

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

        private void setProggressBarTo(ToolStripProgressBar progress, int value)
        {
            statusStrip1.ThreadSafeInvoke(() => {
                if (value < 1) value = 1;
                if (value > 100) value = 100;

                initProgressBar(progress);
                progress.Maximum = (int)((double)progress.Value / (double)(value) * 100);
            });
        }

        private void initProgressBar(ToolStripProgressBar progress)
        {
            if (!isInitialized.Contains(progress.Name))
            {
                progress.Maximum *= 100;
                progress.Value = progress.Maximum / 100;
                isInitialized.Add(progress.Name);
            }
        }



        #endregion
    }

    #region Control Extentions
    public static class ControlExtension
    {

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
    }
    #endregion

}
