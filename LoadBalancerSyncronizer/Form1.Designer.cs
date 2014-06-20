namespace LoadBalancerSyncronizer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnOverrideServers = new System.Windows.Forms.Button();
            this.checkDatabaseForFileChanges = new System.ComponentModel.BackgroundWorker();
            this.btnClone1Settings = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDbSettings = new System.Windows.Forms.Button();
            this.btnMainServer = new System.Windows.Forms.Button();
            this.btnClone3Settings = new System.Windows.Forms.Button();
            this.btnClone2Settings = new System.Windows.Forms.Button();
            this.infoSizeProgressCopied = new System.Windows.Forms.ProgressBar();
            this.infoProgressTotalFilesCopied = new System.Windows.Forms.ProgressBar();
            this.infoSizeCopied = new System.Windows.Forms.Label();
            this.infoTotalFilesCopied = new System.Windows.Forms.Label();
            this.infoFilesOverriden = new System.Windows.Forms.TextBox();
            this.copyAllAsync = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoSyncStatusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.infoSyncStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOverrideServers
            // 
            this.btnOverrideServers.Location = new System.Drawing.Point(12, 19);
            this.btnOverrideServers.Name = "btnOverrideServers";
            this.btnOverrideServers.Size = new System.Drawing.Size(102, 54);
            this.btnOverrideServers.TabIndex = 0;
            this.btnOverrideServers.Text = "Override Clones";
            this.btnOverrideServers.UseVisualStyleBackColor = true;
            this.btnOverrideServers.Click += new System.EventHandler(this.overrideServers_Click);
            // 
            // checkDatabaseForFileChanges
            // 
            this.checkDatabaseForFileChanges.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkDatabaseForFileChanges_DoWork);
            // 
            // btnClone1Settings
            // 
            this.btnClone1Settings.Location = new System.Drawing.Point(12, 139);
            this.btnClone1Settings.Name = "btnClone1Settings";
            this.btnClone1Settings.Size = new System.Drawing.Size(102, 55);
            this.btnClone1Settings.TabIndex = 1;
            this.btnClone1Settings.Text = "Server 1";
            this.btnClone1Settings.UseVisualStyleBackColor = true;
            this.btnClone1Settings.Click += new System.EventHandler(this.btnClone1Settings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnOverrideServers);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 420);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDbSettings);
            this.groupBox2.Controls.Add(this.btnMainServer);
            this.groupBox2.Controls.Add(this.btnClone3Settings);
            this.groupBox2.Controls.Add(this.btnClone2Settings);
            this.groupBox2.Controls.Add(this.btnClone1Settings);
            this.groupBox2.Location = new System.Drawing.Point(0, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(130, 328);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // btnDbSettings
            // 
            this.btnDbSettings.BackColor = System.Drawing.SystemColors.Control;
            this.btnDbSettings.Location = new System.Drawing.Point(12, 19);
            this.btnDbSettings.Name = "btnDbSettings";
            this.btnDbSettings.Size = new System.Drawing.Size(102, 54);
            this.btnDbSettings.TabIndex = 2;
            this.btnDbSettings.Text = "DatabaseSettings";
            this.btnDbSettings.UseVisualStyleBackColor = false;
            this.btnDbSettings.Click += new System.EventHandler(this.btnDbSettings_Click);
            // 
            // btnMainServer
            // 
            this.btnMainServer.Location = new System.Drawing.Point(12, 79);
            this.btnMainServer.Name = "btnMainServer";
            this.btnMainServer.Size = new System.Drawing.Size(102, 54);
            this.btnMainServer.TabIndex = 2;
            this.btnMainServer.Text = "Main Server";
            this.btnMainServer.UseVisualStyleBackColor = true;
            this.btnMainServer.Click += new System.EventHandler(this.btnMainServer_Click);
            // 
            // btnClone3Settings
            // 
            this.btnClone3Settings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClone3Settings.Location = new System.Drawing.Point(12, 261);
            this.btnClone3Settings.Name = "btnClone3Settings";
            this.btnClone3Settings.Size = new System.Drawing.Size(102, 55);
            this.btnClone3Settings.TabIndex = 1;
            this.btnClone3Settings.Text = "Server 3";
            this.btnClone3Settings.UseVisualStyleBackColor = true;
            this.btnClone3Settings.Click += new System.EventHandler(this.btnClone3Settings_Click);
            // 
            // btnClone2Settings
            // 
            this.btnClone2Settings.Location = new System.Drawing.Point(12, 200);
            this.btnClone2Settings.Name = "btnClone2Settings";
            this.btnClone2Settings.Size = new System.Drawing.Size(102, 55);
            this.btnClone2Settings.TabIndex = 1;
            this.btnClone2Settings.Text = "Server 2";
            this.btnClone2Settings.UseVisualStyleBackColor = true;
            this.btnClone2Settings.Click += new System.EventHandler(this.btnClone2Settings_Click);
            // 
            // infoSizeProgressCopied
            // 
            this.infoSizeProgressCopied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoSizeProgressCopied.Location = new System.Drawing.Point(136, 12);
            this.infoSizeProgressCopied.MinimumSize = new System.Drawing.Size(225, 23);
            this.infoSizeProgressCopied.Name = "infoSizeProgressCopied";
            this.infoSizeProgressCopied.Size = new System.Drawing.Size(486, 23);
            this.infoSizeProgressCopied.Step = 1;
            this.infoSizeProgressCopied.TabIndex = 4;
            // 
            // infoProgressTotalFilesCopied
            // 
            this.infoProgressTotalFilesCopied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoProgressTotalFilesCopied.Location = new System.Drawing.Point(136, 56);
            this.infoProgressTotalFilesCopied.MinimumSize = new System.Drawing.Size(225, 23);
            this.infoProgressTotalFilesCopied.Name = "infoProgressTotalFilesCopied";
            this.infoProgressTotalFilesCopied.Size = new System.Drawing.Size(486, 23);
            this.infoProgressTotalFilesCopied.Step = 1;
            this.infoProgressTotalFilesCopied.TabIndex = 4;
            // 
            // infoSizeCopied
            // 
            this.infoSizeCopied.AutoSize = true;
            this.infoSizeCopied.Location = new System.Drawing.Point(136, 40);
            this.infoSizeCopied.Name = "infoSizeCopied";
            this.infoSizeCopied.Size = new System.Drawing.Size(0, 13);
            this.infoSizeCopied.TabIndex = 6;
            // 
            // infoTotalFilesCopied
            // 
            this.infoTotalFilesCopied.AutoSize = true;
            this.infoTotalFilesCopied.Location = new System.Drawing.Point(136, 82);
            this.infoTotalFilesCopied.Name = "infoTotalFilesCopied";
            this.infoTotalFilesCopied.Size = new System.Drawing.Size(0, 13);
            this.infoTotalFilesCopied.TabIndex = 6;
            // 
            // infoFilesOverriden
            // 
            this.infoFilesOverriden.AcceptsReturn = true;
            this.infoFilesOverriden.AcceptsTab = true;
            this.infoFilesOverriden.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoFilesOverriden.BackColor = System.Drawing.SystemColors.Menu;
            this.infoFilesOverriden.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoFilesOverriden.HideSelection = false;
            this.infoFilesOverriden.Location = new System.Drawing.Point(136, 107);
            this.infoFilesOverriden.Multiline = true;
            this.infoFilesOverriden.Name = "infoFilesOverriden";
            this.infoFilesOverriden.ReadOnly = true;
            this.infoFilesOverriden.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoFilesOverriden.Size = new System.Drawing.Size(486, 288);
            this.infoFilesOverriden.TabIndex = 8;
            // 
            // copyAllAsync
            // 
            this.copyAllAsync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.copyAllAsync_DoWork);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoSyncStatusProgress,
            this.toolStripStatusLabel1,
            this.infoSyncStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(130, 398);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoSyncStatusProgress
            // 
            this.infoSyncStatusProgress.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.infoSyncStatusProgress.Name = "infoSyncStatusProgress";
            this.infoSyncStatusProgress.Size = new System.Drawing.Size(300, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(69, 17);
            this.toolStripStatusLabel1.Text = "Sync status:";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // infoSyncStatusLabel
            // 
            this.infoSyncStatusLabel.Name = "infoSyncStatusLabel";
            this.infoSyncStatusLabel.Size = new System.Drawing.Size(67, 17);
            this.infoSyncStatusLabel.Text = "Not Started";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 420);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.infoFilesOverriden);
            this.Controls.Add(this.infoTotalFilesCopied);
            this.Controls.Add(this.infoSizeCopied);
            this.Controls.Add(this.infoProgressTotalFilesCopied);
            this.Controls.Add(this.infoSizeProgressCopied);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Balancer Syncronizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOverrideServers;
        private System.ComponentModel.BackgroundWorker checkDatabaseForFileChanges;
        private System.Windows.Forms.Button btnClone1Settings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClone3Settings;
        private System.Windows.Forms.Button btnClone2Settings;
        private System.Windows.Forms.ProgressBar infoSizeProgressCopied;
        private System.Windows.Forms.ProgressBar infoProgressTotalFilesCopied;
        private System.Windows.Forms.Label infoSizeCopied;
        private System.Windows.Forms.Label infoTotalFilesCopied;
        private System.Windows.Forms.TextBox infoFilesOverriden;
        private System.ComponentModel.BackgroundWorker copyAllAsync;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMainServer;
        private System.Windows.Forms.Button btnDbSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel infoSyncStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar infoSyncStatusProgress;
    }
}

