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
        private const string compressedFileName = "\\files.tar";
        private List<string> isInitialized = new List<string>();
        private List<string> fileNames = new List<string>();

        public Form1()
        {
            DATA = FileSerializer.Load();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showMessageAsync("Set database connection!");
        }


        private void CopyAll(string SourcePath, string DestinationPath)
        {
            //Create all of the directories
            foreach (string dirPath in Directory.EnumerateDirectories(SourcePath, "*.*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files
            string[] paths = Directory.EnumerateFiles(SourcePath, "*.*", SearchOption.AllDirectories).ToArray();
            int i = 1;
            infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Text = "");
            foreach (string sourcePath in paths)
            {
                FileInfo sourceInfo = new FileInfo(sourcePath);
                infoSizeCopied.ThreadSafeInvoke(() => infoSizeCopied.Text = sourceInfo.Length / 1024 + " KB");
                infoSizeProgressCopied.ThreadSafeInvoke(() => setProggressBarTo(infoSizeProgressCopied, 1));

                string destFullPath = sourcePath.Replace(SourcePath, DestinationPath);
                File.Copy(sourcePath, destFullPath, true);

                infoSizeProgressCopied.ThreadSafeInvoke(() => setProggressBarTo(infoSizeProgressCopied, 100));
                //infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Text = destFullPath + "\r\n" + infoFilesOverriden.Text);
                infoProgressTotalFilesCopied.ThreadSafeInvoke(() =>
                {
                    double percentage = ((double)i / (double)paths.Length) * 100;
                    setProggressBarTo(infoProgressTotalFilesCopied, (int)(percentage));
                });
                i++;
                infoTotalFilesCopied.ThreadSafeInvoke(() => infoTotalFilesCopied.Text = "Kopylandı: " + destFullPath);
            }
        }

        private void setProggressBarTo(ProgressBar progress, int value)
        {
            if (value < 1) value = 1;
            if (value > 100) value = 100;

            initProgressBar(progress);
            progress.Maximum = (int)((double)progress.Value / (double)(value) * 100);
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

        private void ProcessXcopy(string SolutionDirectory, string TargetDirectory)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "xcopy",
                Arguments = "\"" + SolutionDirectory + "\"" + " " + "\"" + TargetDirectory + "\"" + @" /e /y /I",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Clear());
                    exeProcess.OutputDataReceived += exeProcess_OutputDataReceived;
                    exeProcess.BeginOutputReadLine();
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

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

        private static void promptServerAddress(string title = "", int i = int.MaxValue)
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
                    paths.ForEach(x =>
                    {
                        infoSyncStatusLabel.Text = "Syncing: " + x.path;
                        foreach (string serverPath in DATA.cloneServers)
                        {
                            i += 100.0 / (paths.Count * DATA.cloneServers.Length);
                            if (!CopyFileFromFileToFile(DATA.mainServer + x.path, serverPath + x.path, x))
                                break;
                            setProggressBarTo(infoSyncStatusProgress, (int)i);
                        }
                        x.isSynced = true;
                        x.Save();
                    });
                }
            }
            catch
            {
                MessageBox.Show("Couldn't connect to database! Background process is not started. \nPlease enter your connection string right!");
                this.ThreadSafeInvoke(() => this.Close());
            }

        }

        private void setProggressBarTo(ToolStripProgressBar progress, int value)
        {
            if (value < 1) value = 1;
            if (value > 100) value = 100;

            initProgressBar(progress);
            progress.Maximum = (int)((double)progress.Value / (double)(value) * 100);

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

        private bool CopyFileFromFileToFile(string srcName, string destName, SyncronizedFilePath path)
        {
            FileInfo sourceFile = new FileInfo(srcName);

            if (!sourceFile.Exists)
            {
                path.ErrorMessage = "Source file " + srcName + " does not exist";
                return false;
            }

            FileInfo destFile = new FileInfo(destName);

            File.Copy(sourceFile.FullName, destFile.FullName, true);

            return true;
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



        private void copyAllAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            resetButtonsBackColor();
            setServerSettingButtonsEnable(false);
            btnOverrideServers.ThreadSafeInvoke(() => btnOverrideServers.Enabled = false);

            infoTotalFilesCopied.ThreadSafeInvoke(() => infoTotalFilesCopied.Text = "Started compressing.");
            string _7zipLocation = "C:\\Users\\student\\Desktop\\7-Zip\\7z";
            executeCommand(_7zipLocation + " a -ttar " + DATA.mainServer + "\\files.tar " + DATA.mainServer + "\\*", true);

            for (int i = 0; i < DATA.cloneServers.Length; i++)
            {
                setServerCloneIsProcesingColor(i);
                copyZippedFile(DATA.cloneServers[i]);
                infoTotalFilesCopied.ThreadSafeInvoke(() => infoTotalFilesCopied.Text = "Started extracting to: " + DATA.cloneServers[i]);
                executeCommand(_7zipLocation + " x -y " + DATA.cloneServers[i] + "\\files.tar -o" + DATA.cloneServers[i] + " -r -aoa");
                setServerCloneIsDoneColor(i);
            }


            //TODO: file lari ust uste yazdirma listeyi UI da goster.
            infoFilesOverriden.ThreadSafeInvoke(() => infoFilesOverriden.Text = "Output file:\r\n" + AppDomain.CurrentDomain.BaseDirectory + "output.txt");
            File.WriteAllLines("output.txt", fileNames);
            btnOverrideServers.ThreadSafeInvoke(() => btnOverrideServers.Enabled = true);
            infoTotalFilesCopied.ThreadSafeInvoke(() => infoTotalFilesCopied.Text = "Done!");
            File.Delete(DATA.mainServer + compressedFileName);
            setServerSettingButtonsEnable(true);

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                resetButtonsBackColor();
            });
        }

        private void executeCommand(string withCommand, bool isOutput = false)
        {
            string exec = withCommand.Split(' ').First();
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Environment.GetEnvironmentVariable("comspec");//exec;
            //startInfo.Arguments = withCommand.Substring(exec.Length);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
            startInfo.ErrorDialog = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

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
                    if (isOutput)
                    {
                        // Begin async stdout and stderr reading
                        exeProcess.BeginOutputReadLine();
                        exeProcess.BeginErrorReadLine();
                    }

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

        }

        private void exeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            fileNames.Add(e.Data);
        }

        private void copyZippedFile(string toDirectory)
        {

            string srcName = DATA.mainServer + compressedFileName;
            string destName = toDirectory + compressedFileName;
            FileInfo sourceFile = new FileInfo(srcName);
            int buflen = 4 * 1024 * 1024; //4 MB buffer
            byte[] buf = new byte[buflen];
            long totalBytesRead = 0;
            double pctDone = 0;
            string msg = "";
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
                        if (numReads % 10 == 0)
                        {
                            pctDone = (double)((double)totalBytesRead / (double)sourceFile.Length) * 100;
                            msg = string.Format("Copying to directory: " + toDirectory + ", {0}% done!", (int)pctDone);
                            infoTotalFilesCopied.ThreadSafeInvoke(() => infoTotalFilesCopied.Text = msg);
                            infoProgressTotalFilesCopied.ThreadSafeInvoke(() => setProggressBarTo(infoProgressTotalFilesCopied, (int)pctDone + 1));
                        }

                        if (bytesRead < buflen) break;

                    }
                }
            }
        }

        private void setServerSettingButtonsEnable(bool isEnabled)
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
            btnClone1Settings.BackColor = back;
            btnClone2Settings.BackColor = back;
            btnClone3Settings.BackColor = back;
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

        public static void showMessageAsync(string message)
        {
            Task.Run(() => MessageBox.Show(message, "Asynchronous Message"));
        }

    }

    public static class ControlExtension
    {
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
    }
}
