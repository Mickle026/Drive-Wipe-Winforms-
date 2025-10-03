namespace Drive_Wipe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            progressBar = new ProgressBar();
            lblStatus = new Label();
            btnPauseResume = new Button();
            btnCancel = new Button();
            lblSpeed = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label7 = new Label();
            label6 = new Label();
            btnSecureDelete = new Button();
            pictureBoxDropTarget = new PictureBox();
            tabPage4 = new TabPage();
            btnSecureDeleteFolder = new Button();
            label8 = new Label();
            pictureBox1 = new PictureBox();
            tabPage2 = new TabPage();
            panelDiskUsage = new Panel();
            lblSummary = new Label();
            label5 = new Label();
            cmbDriveSelector = new ComboBox();
            label1 = new Label();
            numPasses = new NumericUpDown();
            btnWipeFreeSpace = new Button();
            tabPage3 = new TabPage();
            btnStopScheduler = new Button();
            lblSchedulerStatus = new Label();
            btnStartScheduler = new Button();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            numIntervalValue = new NumericUpDown();
            cmbIntervalType = new ComboBox();
            dtpStartTime = new DateTimePicker();
            tabPage5 = new TabPage();
            txtLog = new TextBox();
            chkCreateShortcut = new CheckBox();
            chkEnableDropTarget = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDropTarget).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPasses).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIntervalValue).BeginInit();
            tabPage5.SuspendLayout();
            SuspendLayout();
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 35);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(508, 23);
            progressBar.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 9);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status:";
            // 
            // btnPauseResume
            // 
            btnPauseResume.ForeColor = SystemColors.ControlText;
            btnPauseResume.Location = new Point(170, 468);
            btnPauseResume.Name = "btnPauseResume";
            btnPauseResume.Size = new Size(197, 32);
            btnPauseResume.TabIndex = 7;
            btnPauseResume.Text = "Pause/Resume";
            btnPauseResume.UseVisualStyleBackColor = true;
            btnPauseResume.Click += btnPauseResume_Click;
            // 
            // btnCancel
            // 
            btnCancel.ForeColor = SystemColors.ControlText;
            btnCancel.Location = new Point(434, 468);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(82, 32);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(69, 9);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(0, 15);
            lblSpeed.TabIndex = 9;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Location = new Point(12, 64);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(508, 387);
            tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            tabPage1.BorderStyle = BorderStyle.FixedSingle;
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(btnSecureDelete);
            tabPage1.Controls.Add(pictureBoxDropTarget);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(500, 359);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "File";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(288, 6);
            label7.Name = "label7";
            label7.Size = new Size(81, 15);
            label7.TabIndex = 4;
            label7.Text = "Browse to file:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new Point(6, 6);
            label6.Name = "label6";
            label6.Size = new Size(261, 17);
            label6.TabIndex = 3;
            label6.Text = "Drop File On This Image To Shred/Secure Delete";
            label6.Click += label6_Click;
            // 
            // btnSecureDelete
            // 
            btnSecureDelete.Location = new Point(288, 24);
            btnSecureDelete.Name = "btnSecureDelete";
            btnSecureDelete.Size = new Size(191, 33);
            btnSecureDelete.TabIndex = 1;
            btnSecureDelete.Text = "Secure Delete File";
            btnSecureDelete.UseVisualStyleBackColor = true;
            btnSecureDelete.Click += btnSecureDelete_Click_1;
            // 
            // pictureBoxDropTarget
            // 
            pictureBoxDropTarget.Image = (Image)resources.GetObject("pictureBoxDropTarget.Image");
            pictureBoxDropTarget.Location = new Point(6, 6);
            pictureBoxDropTarget.Name = "pictureBoxDropTarget";
            pictureBoxDropTarget.Size = new Size(261, 345);
            pictureBoxDropTarget.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxDropTarget.TabIndex = 2;
            pictureBoxDropTarget.TabStop = false;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(btnSecureDeleteFolder);
            tabPage4.Controls.Add(label8);
            tabPage4.Controls.Add(pictureBox1);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(500, 359);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Folder";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnSecureDeleteFolder
            // 
            btnSecureDeleteFolder.Location = new Point(297, 39);
            btnSecureDeleteFolder.Name = "btnSecureDeleteFolder";
            btnSecureDeleteFolder.Size = new Size(191, 33);
            btnSecureDeleteFolder.TabIndex = 5;
            btnSecureDeleteFolder.Text = "Secure Delete Folder";
            btnSecureDeleteFolder.UseVisualStyleBackColor = true;
            btnSecureDeleteFolder.Click += btnSecureDeleteFolder_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BorderStyle = BorderStyle.FixedSingle;
            label8.Location = new Point(6, 6);
            label8.Name = "label8";
            label8.Size = new Size(276, 17);
            label8.TabIndex = 4;
            label8.Text = "Drop Folder On This Image To Shred/Secure Delete";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(6, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(276, 345);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // tabPage2
            // 
            tabPage2.BorderStyle = BorderStyle.FixedSingle;
            tabPage2.Controls.Add(panelDiskUsage);
            tabPage2.Controls.Add(lblSummary);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(cmbDriveSelector);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(numPasses);
            tabPage2.Controls.Add(btnWipeFreeSpace);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(500, 359);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Drive";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelDiskUsage
            // 
            panelDiskUsage.Location = new Point(22, 16);
            panelDiskUsage.Name = "panelDiskUsage";
            panelDiskUsage.Size = new Size(254, 243);
            panelDiskUsage.TabIndex = 18;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Location = new Point(76, 262);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(71, 15);
            lblSummary.TabIndex = 17;
            lblSummary.Text = "lblSummary";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(362, 16);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 16;
            label5.Text = "Drive:";
            // 
            // cmbDriveSelector
            // 
            cmbDriveSelector.FormattingEnabled = true;
            cmbDriveSelector.Location = new Point(362, 34);
            cmbDriveSelector.Name = "cmbDriveSelector";
            cmbDriveSelector.Size = new Size(121, 23);
            cmbDriveSelector.TabIndex = 15;
            cmbDriveSelector.SelectedIndexChanged += cmbDriveSelector_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(362, 83);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 13;
            label1.Text = "Passes:";
            // 
            // numPasses
            // 
            numPasses.Location = new Point(362, 101);
            numPasses.Name = "numPasses";
            numPasses.Size = new Size(73, 23);
            numPasses.TabIndex = 12;
            // 
            // btnWipeFreeSpace
            // 
            btnWipeFreeSpace.Location = new Point(137, 319);
            btnWipeFreeSpace.Name = "btnWipeFreeSpace";
            btnWipeFreeSpace.Size = new Size(235, 32);
            btnWipeFreeSpace.TabIndex = 11;
            btnWipeFreeSpace.Text = "Wipe Free Space";
            btnWipeFreeSpace.UseVisualStyleBackColor = true;
            btnWipeFreeSpace.Click += btnWipeFreeSpace_Click_1;
            // 
            // tabPage3
            // 
            tabPage3.BorderStyle = BorderStyle.FixedSingle;
            tabPage3.Controls.Add(btnStopScheduler);
            tabPage3.Controls.Add(lblSchedulerStatus);
            tabPage3.Controls.Add(btnStartScheduler);
            tabPage3.Controls.Add(label4);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(numIntervalValue);
            tabPage3.Controls.Add(cmbIntervalType);
            tabPage3.Controls.Add(dtpStartTime);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(500, 359);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Scheduler";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnStopScheduler
            // 
            btnStopScheduler.Location = new Point(308, 76);
            btnStopScheduler.Name = "btnStopScheduler";
            btnStopScheduler.Size = new Size(166, 23);
            btnStopScheduler.TabIndex = 8;
            btnStopScheduler.Text = "Stop scheduling";
            btnStopScheduler.UseVisualStyleBackColor = true;
            btnStopScheduler.Click += btnStopScheduler_Click;
            // 
            // lblSchedulerStatus
            // 
            lblSchedulerStatus.AutoSize = true;
            lblSchedulerStatus.Location = new Point(25, 249);
            lblSchedulerStatus.Name = "lblSchedulerStatus";
            lblSchedulerStatus.Size = new Size(62, 15);
            lblSchedulerStatus.TabIndex = 7;
            lblSchedulerStatus.Text = "Scheduler ";
            // 
            // btnStartScheduler
            // 
            btnStartScheduler.Location = new Point(308, 39);
            btnStartScheduler.Name = "btnStartScheduler";
            btnStartScheduler.Size = new Size(166, 23);
            btnStartScheduler.TabIndex = 6;
            btnStartScheduler.Text = "Start scheduling";
            btnStartScheduler.UseVisualStyleBackColor = true;
            btnStartScheduler.Click += btnStartScheduler_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 9);
            label4.Name = "label4";
            label4.Size = new Size(132, 15);
            label4.TabIndex = 5;
            label4.Text = "Pick the exact start time";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 144);
            label3.Name = "label3";
            label3.Size = new Size(215, 15);
            label3.TabIndex = 4;
            label3.Text = "Define how many minutes/days/weeks ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 76);
            label2.Name = "label2";
            label2.Size = new Size(115, 15);
            label2.TabIndex = 3;
            label2.Text = "Choose interval type";
            // 
            // numIntervalValue
            // 
            numIntervalValue.Location = new Point(25, 173);
            numIntervalValue.Name = "numIntervalValue";
            numIntervalValue.Size = new Size(200, 23);
            numIntervalValue.TabIndex = 2;
            // 
            // cmbIntervalType
            // 
            cmbIntervalType.FormattingEnabled = true;
            cmbIntervalType.Location = new Point(25, 101);
            cmbIntervalType.Name = "cmbIntervalType";
            cmbIntervalType.Size = new Size(200, 23);
            cmbIntervalType.TabIndex = 1;
            // 
            // dtpStartTime
            // 
            dtpStartTime.Location = new Point(25, 39);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.Size = new Size(200, 23);
            dtpStartTime.TabIndex = 0;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(txtLog);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(500, 359);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Log Viewer";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Location = new Point(3, 14);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(488, 311);
            txtLog.TabIndex = 14;
            // 
            // chkCreateShortcut
            // 
            chkCreateShortcut.AutoSize = true;
            chkCreateShortcut.Location = new Point(16, 453);
            chkCreateShortcut.Name = "chkCreateShortcut";
            chkCreateShortcut.Size = new Size(117, 19);
            chkCreateShortcut.TabIndex = 11;
            chkCreateShortcut.Text = "Desktop Shortcut";
            chkCreateShortcut.UseVisualStyleBackColor = true;
            chkCreateShortcut.CheckedChanged += chkCreateShortcut_CheckedChanged_1;
            // 
            // chkEnableDropTarget
            // 
            chkEnableDropTarget.AutoSize = true;
            chkEnableDropTarget.Location = new Point(16, 481);
            chkEnableDropTarget.Name = "chkEnableDropTarget";
            chkEnableDropTarget.Size = new Size(134, 19);
            chkEnableDropTarget.TabIndex = 12;
            chkEnableDropTarget.Text = "Desktop Drop Target";
            chkEnableDropTarget.UseVisualStyleBackColor = true;
            chkEnableDropTarget.CheckedChanged += chkEnableDropTarget_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(532, 512);
            Controls.Add(chkEnableDropTarget);
            Controls.Add(chkCreateShortcut);
            Controls.Add(tabControl1);
            Controls.Add(lblSpeed);
            Controls.Add(btnCancel);
            Controls.Add(btnPauseResume);
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Secure File/Drive Wipe";
            Load += Form1_Load;
            Leave += Form1_Leave;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDropTarget).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPasses).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIntervalValue).EndInit();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ProgressBar progressBar;
        private Label lblStatus;
        private Button btnPauseResume;
        private Button btnCancel;
        private Label lblSpeed;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnSecureDelete;
        private TabPage tabPage2;
        private TextBox txtLog;
        private Label label1;
        private NumericUpDown numPasses;
        private Button btnWipeFreeSpace;
        private TabPage tabPage3;
        private Button btnStartScheduler;
        private Label label4;
        private Label label3;
        private Label label2;
        private NumericUpDown numIntervalValue;
        private ComboBox cmbIntervalType;
        private DateTimePicker dtpStartTime;
        private Button btnStopScheduler;
        private Label lblSchedulerStatus;
        private Label label5;
        private ComboBox cmbDriveSelector;
        private PictureBox pictureBoxDropTarget;
        private Label label6;
        private CheckBox chkCreateShortcut;
        private Label label7;
        private CheckBox chkEnableDropTarget;
        private TabPage tabPage4;
        private Button btnSecureDeleteFolder;
        private Label label8;
        private PictureBox pictureBox1;
        private TabPage tabPage5;
        private Label lblSummary;
        private Panel panelDiskUsage;
    }
}
