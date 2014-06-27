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
            this.checkDatabaseForFileChanges = new System.ComponentModel.BackgroundWorker();
            this.btnClone1Settings = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDbSettings = new System.Windows.Forms.Button();
            this.btnMainServer = new System.Windows.Forms.Button();
            this.btnClone4Settings = new System.Windows.Forms.Button();
            this.btnBackgroundSync = new System.Windows.Forms.Button();
            this.btnClone3Settings = new System.Windows.Forms.Button();
            this.btnClone2Settings = new System.Windows.Forms.Button();
            this.infoProgressTotalFilesCopied = new System.Windows.Forms.ProgressBar();
            this.infoTotalFilesCopied = new System.Windows.Forms.Label();
            this.infoFilesOverriden = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoSyncStatusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.infoSyncStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkDatabaseForFileChanges
            // 
            this.checkDatabaseForFileChanges.WorkerSupportsCancellation = true;
            this.checkDatabaseForFileChanges.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkDatabaseForFileChanges_DoWork);
            // 
            // btnClone1Settings
            // 
            this.btnClone1Settings.Location = new System.Drawing.Point(6, 79);
            this.btnClone1Settings.Name = "btnClone1Settings";
            this.btnClone1Settings.Size = new System.Drawing.Size(106, 55);
            this.btnClone1Settings.TabIndex = 3;
            this.btnClone1Settings.Text = "Server 1";
            this.btnClone1Settings.UseVisualStyleBackColor = true;
            this.btnClone1Settings.Click += new System.EventHandler(this.btnClone1Settings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDbSettings);
            this.groupBox1.Controls.Add(this.btnMainServer);
            this.groupBox1.Controls.Add(this.btnClone4Settings);
            this.groupBox1.Controls.Add(this.btnClone1Settings);
            this.groupBox1.Controls.Add(this.btnBackgroundSync);
            this.groupBox1.Controls.Add(this.btnClone3Settings);
            this.groupBox1.Controls.Add(this.btnClone2Settings);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 292);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // btnDbSettings
            // 
            this.btnDbSettings.BackColor = System.Drawing.SystemColors.Control;
            this.btnDbSettings.Location = new System.Drawing.Point(6, 19);
            this.btnDbSettings.Name = "btnDbSettings";
            this.btnDbSettings.Size = new System.Drawing.Size(106, 54);
            this.btnDbSettings.TabIndex = 1;
            this.btnDbSettings.Text = "DatabaseSettings";
            this.btnDbSettings.UseVisualStyleBackColor = false;
            this.btnDbSettings.Click += new System.EventHandler(this.btnDbSettings_Click);
            // 
            // btnMainServer
            // 
            this.btnMainServer.Location = new System.Drawing.Point(118, 19);
            this.btnMainServer.Name = "btnMainServer";
            this.btnMainServer.Size = new System.Drawing.Size(106, 54);
            this.btnMainServer.TabIndex = 2;
            this.btnMainServer.Text = "Main Server";
            this.btnMainServer.UseVisualStyleBackColor = true;
            this.btnMainServer.Click += new System.EventHandler(this.btnMainServer_Click);
            // 
            // btnClone4Settings
            // 
            this.btnClone4Settings.Location = new System.Drawing.Point(118, 139);
            this.btnClone4Settings.Name = "btnClone4Settings";
            this.btnClone4Settings.Size = new System.Drawing.Size(106, 55);
            this.btnClone4Settings.TabIndex = 6;
            this.btnClone4Settings.Text = "Server 4";
            this.btnClone4Settings.UseVisualStyleBackColor = true;
            this.btnClone4Settings.Click += new System.EventHandler(this.btnClone4Settings_Click);
            // 
            // btnBackgroundSync
            // 
            this.btnBackgroundSync.Location = new System.Drawing.Point(6, 200);
            this.btnBackgroundSync.Name = "btnBackgroundSync";
            this.btnBackgroundSync.Size = new System.Drawing.Size(218, 55);
            this.btnBackgroundSync.TabIndex = 5;
            this.btnBackgroundSync.Text = "Sync via Server Root Paths";
            this.btnBackgroundSync.UseVisualStyleBackColor = true;
            this.btnBackgroundSync.Click += new System.EventHandler(this.btnBackgroundSync_Click);
            // 
            // btnClone3Settings
            // 
            this.btnClone3Settings.Location = new System.Drawing.Point(6, 139);
            this.btnClone3Settings.Name = "btnClone3Settings";
            this.btnClone3Settings.Size = new System.Drawing.Size(106, 55);
            this.btnClone3Settings.TabIndex = 5;
            this.btnClone3Settings.Text = "Server 3";
            this.btnClone3Settings.UseVisualStyleBackColor = true;
            this.btnClone3Settings.Click += new System.EventHandler(this.btnClone3Settings_Click);
            // 
            // btnClone2Settings
            // 
            this.btnClone2Settings.Location = new System.Drawing.Point(118, 80);
            this.btnClone2Settings.Name = "btnClone2Settings";
            this.btnClone2Settings.Size = new System.Drawing.Size(106, 55);
            this.btnClone2Settings.TabIndex = 4;
            this.btnClone2Settings.Text = "Server 2";
            this.btnClone2Settings.UseVisualStyleBackColor = true;
            this.btnClone2Settings.Click += new System.EventHandler(this.btnClone2Settings_Click);
            // 
            // infoProgressTotalFilesCopied
            // 
            this.infoProgressTotalFilesCopied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoProgressTotalFilesCopied.Location = new System.Drawing.Point(242, 12);
            this.infoProgressTotalFilesCopied.MinimumSize = new System.Drawing.Size(225, 23);
            this.infoProgressTotalFilesCopied.Name = "infoProgressTotalFilesCopied";
            this.infoProgressTotalFilesCopied.Size = new System.Drawing.Size(581, 23);
            this.infoProgressTotalFilesCopied.Step = 1;
            this.infoProgressTotalFilesCopied.TabIndex = 4;
            // 
            // infoTotalFilesCopied
            // 
            this.infoTotalFilesCopied.AutoSize = true;
            this.infoTotalFilesCopied.Location = new System.Drawing.Point(239, 38);
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
            this.infoFilesOverriden.Location = new System.Drawing.Point(242, 69);
            this.infoFilesOverriden.Multiline = true;
            this.infoFilesOverriden.Name = "infoFilesOverriden";
            this.infoFilesOverriden.ReadOnly = true;
            this.infoFilesOverriden.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoFilesOverriden.Size = new System.Drawing.Size(581, 198);
            this.infoFilesOverriden.TabIndex = 8;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoSyncStatusProgress,
            this.toolStripStatusLabel1,
            this.infoSyncStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(233, 270);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoSyncStatusProgress
            // 
            this.infoSyncStatusProgress.Margin = new System.Windows.Forms.Padding(7, 3, 1, 3);
            this.infoSyncStatusProgress.Name = "infoSyncStatusProgress";
            this.infoSyncStatusProgress.Size = new System.Drawing.Size(300, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(135, 17);
            this.toolStripStatusLabel1.Text = "Background sync status:";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // infoSyncStatusLabel
            // 
            this.infoSyncStatusLabel.Name = "infoSyncStatusLabel";
            this.infoSyncStatusLabel.Size = new System.Drawing.Size(67, 17);
            this.infoSyncStatusLabel.Text = "Not Started";
            this.infoSyncStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 292);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.infoFilesOverriden);
            this.Controls.Add(this.infoTotalFilesCopied);
            this.Controls.Add(this.infoProgressTotalFilesCopied);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Balancer Syncronizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker checkDatabaseForFileChanges;
        private System.Windows.Forms.Button btnClone1Settings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClone3Settings;
        private System.Windows.Forms.Button btnClone2Settings;
        private System.Windows.Forms.ProgressBar infoProgressTotalFilesCopied;
        private System.Windows.Forms.Label infoTotalFilesCopied;
        private System.Windows.Forms.TextBox infoFilesOverriden;
        private System.Windows.Forms.Button btnMainServer;
        private System.Windows.Forms.Button btnDbSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel infoSyncStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar infoSyncStatusProgress;
        private System.Windows.Forms.Button btnClone4Settings;
        private System.Windows.Forms.Button btnBackgroundSync;
    }
}

