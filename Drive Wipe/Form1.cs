using System.Security.Cryptography;
using System.Management;
using System.Diagnostics;
using IWshRuntimeLibrary;
using System.IO;
using Color = System.Drawing.Color;
using System.Data;

namespace Drive_Wipe
{
    public partial class Form1 : Form
    {

        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool isPaused = false;
        private System.Threading.Timer? wipeTimer;
        private DateTime scheduledStartTime;
        private TimeSpan wipeInterval;
        private object numWipeInterval = 3;
        private string? wipingDrive = null;
        const int gridSize = 30;
        const int squareSize = 8;
        int passCount = 0;
        long totalSquares = gridSize * gridSize;
        private long writtenBytes = 0; // Track wipe progress
        private long wipedSpace = 0; // Tracks amount of space wiped
        long totalSpace;
        long freeSpace;
        long usedSpace;
        long cusedspace;
        private string? currentWipingDrive = null;
        long safeFreeSpaceThreshold = 5_000_000_000; // Reserve 5GB of free space

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
            panelDiskUsage.Paint += panelDiskUsage_Paint;

            // Use the `SetStyle` method of the `Form1` class instead of directly calling it on `panelDiskUsage`.
            SetPanelStyle(panelDiskUsage);
        }

        // Helper method to set styles for the panel
        private void SetPanelStyle(Panel panel)
        {
            panel.GetType().InvokeMember("SetStyle",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null, panel, new object[] { ControlStyles.OptimizedDoubleBuffer, true });

            panel.GetType().InvokeMember("SetStyle",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null, panel, new object[] { ControlStyles.AllPaintingInWmPaint, true });

            panel.GetType().InvokeMember("SetStyle",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null, panel, new object[] { ControlStyles.UserPaint, true });
        }
        private void Form_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) // Added null check for e.Data
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop)!; // Use null-forgiving operator
                foreach (string item in droppedItems)
                {
                    if (Directory.Exists(item))
                        SecureDeleteFolder(item, 3, this);
                    else if (System.IO.File.Exists(item))
                        SecureDeleteFile(item, 3);
                }
            }
            else
            {
                LogMessage("No valid data was dropped.");
            }
        }
        private void chkCreateShortcut_CheckedChanged(object sender, EventArgs e)
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SecureDelete.lnk");

            if (chkCreateShortcut.Checked)
            {
                CreateDesktopShortcut(); // Create shortcut
            }
            else
            {
                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath); // Remove shortcut
                    LogMessage("Desktop shortcut removed successfully.");
                }
            }
        }
        private void chkCreateShortcut_CheckedChanged_1(object sender, EventArgs e)
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SecureDelete.lnk");

            if (chkCreateShortcut.Checked)
            {
                CreateDesktopShortcut(); // Create shortcut
            }
            else
            {
                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath); // Remove shortcut
                    LogMessage("Desktop shortcut removed successfully.");
                }
            }
        }
        private void CreateDesktopShortcut()
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SecureDelete.lnk");
            string appPath = Application.ExecutablePath;

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = appPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(appPath);
            shortcut.Description = "Drag files here for secure deletion";
            shortcut.Save();

            LogMessage("Desktop shortcut created successfully.");
        }

        private void btnSecureDelete_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SecureDeleteFile(openFileDialog.FileName, (int)numPasses.Value);
            }
        }
        private void btnSecureDeleteFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;

                if (Directory.Exists(selectedFolder))
                {
                    SecureDeleteFolder(selectedFolder, 3, this); // Securely delete all files in folder
                }
            }
        }
        private void btnWipeFreeSpace_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                ObfuscateFreeSpace(folderDialog.SelectedPath, 90);
            }
        }

        public async void SecureDeleteFile(string filePath, int passes)
        {
            await Task.Run(() =>
            {
                try
                {
                    DateTime startTime = DateTime.Now;
                    long fileSize = new FileInfo(filePath).Length;
                    byte[] randomData = new byte[fileSize];
                    passCount = 0;

                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                    {
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            for (int i = 0; i < passes; i++)
                            {
                                passCount++;
                                rng.GetBytes(randomData);
                                fs.Seek(0, SeekOrigin.Begin);
                                fs.Write(randomData, 0, randomData.Length);
                                fs.Flush();

                                TimeSpan elapsed = DateTime.Now - startTime;
                                double bytesPerSecond = fileSize * (i + 1) / elapsed.TotalSeconds;
                                double estimatedRemaining = (passes - (i + 1)) * (fileSize / bytesPerSecond);
                                lblSpeed.Invoke((Action)(() => lblSpeed.Text = $"Speed: {bytesPerSecond / 1024 / 1024:F2} MB/s"));

                                progressBar.Invoke((Action)(() => progressBar.Value = i + 1));
                                lblStatus.Invoke((Action)(() => lblStatus.Text = $"Pass {i + 1}/{passes} - ETA: {TimeSpan.FromSeconds(estimatedRemaining):mm\\:ss}"));

                                // Invoke LogMessage properly to update the UI live
                                Invoke((Action)(() => LogMessage($"Pass {i + 1}: Overwritten {fileSize} bytes. Speed: {bytesPerSecond / 1024 / 1024:F2} MB/s")));
                            }
                        }
                    }

                    System.IO.File.Delete(filePath);
                    lblStatus.Invoke((Action)(() => lblStatus.Text = "File securely deleted."));
                }
                catch (Exception ex)
                {
                    // Handle exceptions and update the UI accordingly
                    lblStatus.Invoke((Action)(() => lblStatus.Text = $"Error: {ex.Message}"));
                    LogMessage($"Error securely deleting file: {ex.Message}");
                    return; // Exit the method if an error occurs

                }
      
            });
        }

        public static async void SecureDeleteFolder(string folderPath, int passes, Form1 mainForm)
        {
            await Task.Run(() =>
            {
                DateTime startTime = DateTime.Now;

                if (!Directory.Exists(folderPath))
                {
                    mainForm.LogMessage($"Folder not found: {folderPath}");
                    return;
                }

                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    mainForm.SecureDeleteFile(file, passes);
                }

                // Once all files are securely deleted, remove the folder itself
                try
                {
                    Directory.Delete(folderPath, true); // Delete empty folder
                    mainForm.Invoke((Action)(() => mainForm.lblStatus.Text = "Folder securely deleted."));
                    mainForm.LogMessage($"Folder securely deleted: {folderPath}");
                }
                catch (Exception ex)
                {
                    mainForm.LogMessage($"Error deleting folder: {ex.Message}");
                }
            });
        }
        private long wipeSquares = 0;
        private async void ObfuscateFreeSpace(string selectedDrive, int maxUsagePercentage, int Passes = 3)
        {
            wipingDrive = selectedDrive; // Set the drive being wiped

            await Task.Run(() =>
            {
                try
                {
                    DateTime startTime = DateTime.Now; // Track the start time
                    int passes = 3; // Default to 3 overwrite passes, adjust as needed

                    string? rootPath = Path.GetPathRoot(selectedDrive);
                    if (string.IsNullOrEmpty(rootPath))
                    {
                        lblStatus.Invoke((Action)(() => lblStatus.Text = "Error: Invalid folder path."));
                        LogMessage("Invalid folder path provided.");
                        return;
                    }

                    DriveInfo drive = new DriveInfo(rootPath);
                    long freeSpace = drive.AvailableFreeSpace;
                    long targetUsage = (freeSpace * maxUsagePercentage) / 100;

                    // Prevent execution if free space is below safe limits
                    if (freeSpace < 500_000_000) // Less than 500MB remaining
                    {
                        lblStatus.Invoke((Action)(() => lblStatus.Text = "Error: Insufficient free space for wiping"));
                        LogMessage("Wipe aborted: Not enough free space.");
                        return;
                    }

                    string tempDir = Path.Combine(selectedDrive, "~WipeTemp~");
                    Directory.CreateDirectory(tempDir);

                    progressBar.Invoke((Action)(() =>
                    {
                        int maxProgress = (int)Math.Min(targetUsage / (50 * 1024 * 1024), int.MaxValue);
                        progressBar.Value = 0;
                        progressBar.Maximum = maxProgress; // Adjusted to prevent overflow
                    }));


                    for (int pass = 1; pass <= passes; pass++) // Perform multiple overwrite passes
                    {
                        LogMessage($"Starting pass {pass} for disk wipe.");
                        long writtenBytes = 0; // Reset written bytes for each pass

                        lblStatus.Invoke((Action)(() => lblStatus.Text = $"Pass {pass}/{passes} - Writing to free space..."));

                        while (writtenBytes < targetUsage)
                        {
                            // Check for cancellation request
                            if (cts.Token.IsCancellationRequested)
                            {
                                LogMessage("Operation cancelled. Cleaning up temporary files.");
                                Directory.Delete(tempDir, true);  // Cleanup temp files
                                lblStatus.Invoke((Action)(() => lblStatus.Text = "Cancelled - Cleanup Complete"));
                                LogMessage("Cancelled - Cleanup Complete");
                                lblSummary.Invoke((Action)(() => UpdateSummaryLabel())); // Updates UI and Label
                                wipingDrive = null; // Reset wiping drive when canceled
                                return;
                            }

                            // Check for pause
                            while (isPaused)
                            {
                                Thread.Sleep(500); // Wait while paused
                            }

                            // ✅ **Prevent running out of disk space**
                            if (freeSpace - Interlocked.Read(ref writtenBytes) < safeFreeSpaceThreshold)
                            {
                                LogMessage("Stopping wipe: Preventing total disk usage.");
                                return; // Stop before running out of space
                            }

                            // ✅ **Write progressively larger files**
                            string tempFile = Path.Combine(tempDir, Path.GetRandomFileName());
                            long fileSize = Math.Min(500_000_000, freeSpace / 10); // Up to 500MB or 10% of free space
                            byte[] randomData = new byte[fileSize];

                            RandomNumberGenerator.Fill(randomData);

                            using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None, 65536, FileOptions.WriteThrough))
                            using (BufferedStream bs = new BufferedStream(fs))
                            {
                                bs.Write(randomData, 0, randomData.Length);
                            }

                            // ✅ **Thread-safe progress tracking**
                            Interlocked.Add(ref writtenBytes, fileSize);
                            Interlocked.Add(ref wipedSpace, fileSize); // Track separately

                            // ✅ **Update progress bar dynamically**
                            progressBar.Invoke((Action)(() =>
                            {
                                int newProgressValue = (int)Math.Min(Interlocked.Read(ref writtenBytes) / (50 * 1024 * 1024), progressBar.Maximum);
                                progressBar.Value = newProgressValue;

                                double progressPercent = (double)newProgressValue / progressBar.Maximum * 100;
                                lblStatus.Invoke((Action)(() => lblStatus.Text = $"Pass {pass}/{passes} - {progressPercent:F2}% completed"));
                            }));

                            // ✅ **Ensure wipe visualization updates correctly**
                            if (selectedDrive == wipingDrive)
                            {
                                wipeSquares = (long)Math.Round(totalSquares * (progressBar.Value / (double)progressBar.Maximum));
                            }

                            lblSummary.Invoke((Action)(() => UpdateSummaryLabel())); // Updates UI and Label
                            LogMessage($"Pass {pass}: Wrote {fileSize} bytes to {tempFile}");
                        }

                        LogMessage($"Pass {pass} completed.");
                        Directory.Delete(tempDir, true); // Remove wipe files before next pass
                        Directory.CreateDirectory(tempDir); // Recreate temp folder for the next pass
                    }

                    Directory.Delete(tempDir, true);
                    lblStatus.Invoke((Action)(() => lblStatus.Text = "Disk free space obfuscation complete!"));
                    GenerateReport("Disk Free Space Wipe", targetUsage, DateTime.Now - startTime, passes);
                    wipingDrive = null; // Reset once wipe is complete
                }
                catch (UnauthorizedAccessException)
                {
                    lblStatus.Invoke((Action)(() => lblStatus.Text = "Error: Permission Denied."));
                    LogMessage("Permission denied on target folder.");
                }
                catch (Exception ex)
                {
                    lblStatus.Invoke((Action)(() => lblStatus.Text = $"Error: {ex.Message}"));
                    LogMessage($"Unexpected error: {ex.Message}");
                }
            });

            cmbDriveSelector.Enabled = true; // Re-enable drive selection after wipe
            numPasses.Enabled = true; // Re-enable passes selection after wipe
            btnWipeFreeSpace.Enabled = true; // Re-enable button after wipe
        }

        private void LogMessage(string message)
        {
            string logFilePath = Path.Combine(Application.StartupPath, $"wipe_log_{DateTime.Now:yyyy-MM-dd}.txt");

            // Ensure the file exists before checking its size
            if (!System.IO.File.Exists(logFilePath))
            {
                System.IO.File.WriteAllText(logFilePath, "Log initialized.\n"); // Create the file
            }

            if (new FileInfo(logFilePath).Length > 5 * 1024 * 1024) // Rotate if > 5MB
            {
                logFilePath = Path.Combine(Application.StartupPath, $"wipe_log_{DateTime.Now:yyyy-MM-dd_HH-mm}.txt");
            }

            System.IO.File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\r\n");

            // Ensure real-time update inside the log viewer
            txtLog.Invoke((Action)(() => txtLog.AppendText($"{DateTime.Now}: {message}\r\n")));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numPasses.Value = 3; // Default overwrite passes
            numIntervalValue.Value = 7; // Default interval set to 7 days

            // Initialize ComboBox with interval options
            cmbIntervalType.Items.AddRange(new string[] { "Minutes", "Hours", "Days", "Weeks", "Months" });
            cmbIntervalType.SelectedIndex = 2; // Default selection to 'Days'

            cmbDriveSelector.Items.Clear();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) // Only list accessible drives
                {
                    cmbDriveSelector.Items.Add(drive.Name);
                }
            }

            if (cmbDriveSelector.Items.Count > 0)
                cmbDriveSelector.SelectedIndex = 0; // Default to first available drive

            pictureBoxDropTarget.AllowDrop = true; // Enable drag & drop
            pictureBoxDropTarget.DragEnter += new DragEventHandler(PictureBox_DragEnter);
            pictureBoxDropTarget.DragDrop += new DragEventHandler(PictureBox_DragDrop);


        }

        private void PictureBox_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void PictureBox_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop)!; // Use null-forgiving operator
                foreach (string item in droppedItems)
                {
                    if (Directory.Exists(item)) // Folder detected
                        SecureDeleteFolder(item, 3, this);
                    else if (System.IO.File.Exists(item)) // File detected
                        SecureDeleteFile(item, 3);
                }
            }
            else
            {
                LogMessage("No valid data was dropped.");
            }
        }

        private void GenerateReport(string operationType, long totalBytes, TimeSpan elapsedTime, int passes)
        {
            string reportPath = $"wipe_report_{DateTime.Now:yyyy-MM-dd_HH-mm}.txt";

            string reportContent = $@"
                    SECURE DELETE REPORT
                    ---------------------
                    Operation: {operationType}
                    Start Time: {DateTime.Now.Add(-elapsedTime)}
                    End Time: {DateTime.Now}
                    Total Data Processed: {totalBytes / 1024 / 1024} MB
                    Wipe Duration: {elapsedTime.TotalSeconds:F2} sec
                    Overwrite Passes: {passes}
                    Average Speed: {(totalBytes / elapsedTime.TotalSeconds) / 1024 / 1024:F2} MB/s
                    ";

            System.IO.File.WriteAllText(reportPath, reportContent);
            LogMessage($"Report generated: {reportPath}");
            LogMessage(reportContent);
            MessageBox.Show("Wipe process complete! Detailed report saved.");
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            string action = isPaused ? "Paused" : "Resumed";
            lblStatus.Text = action;
            LogMessage($"User action: {action} wipe operation");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the wipe process?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                cts.Cancel();
                cmbDriveSelector.Enabled = true; // Re-enable drive selection after wipe
                numPasses.Enabled = true; // Re-enable passes selection after wipe
                btnWipeFreeSpace.Enabled = true; // Re-enable button after wipe
                lblStatus.Text = "Wipe process cancelled.";
                LogMessage("Wipe process cancelled by user.");
                UpdateSummaryLabel();

            }
        }

        private bool SetupScheduler()
        {
            if (cmbIntervalType.SelectedItem == null)
            {
                MessageBox.Show("Please select an interval type.");
                return false;
            }

            scheduledStartTime = dtpStartTime.Value;
            int intervalValue = (int)numIntervalValue.Value; // Get user-defined interval

            // Convert selected interval type to a usable TimeSpan
            switch (cmbIntervalType.SelectedItem.ToString())
            {
                case "Minutes": wipeInterval = TimeSpan.FromMinutes(intervalValue); break;
                case "Hours": wipeInterval = TimeSpan.FromHours(intervalValue); break;
                case "Days": wipeInterval = TimeSpan.FromDays(intervalValue); break;
                case "Weeks": wipeInterval = TimeSpan.FromDays(intervalValue * 7); break;
                case "Months": wipeInterval = TimeSpan.FromDays(intervalValue * 30); break;
                default:
                    MessageBox.Show("Invalid interval type.");
                    return false;
            }

            TimeSpan initialDelay = scheduledStartTime - DateTime.Now;

            if (initialDelay.TotalMilliseconds < 0)
            {
                MessageBox.Show("Start time must be in the future.");
                lblSchedulerStatus.Text = "Scheduler Not Running.";
                LogMessage("Scheduler setup failed: Start time was in the past.");

                return false; // Exit function safely

            }

            wipeTimer = new System.Threading.Timer((e) => ScheduledWipe(), null, initialDelay, wipeInterval);
            lblSchedulerStatus.Text = $"Scheduler Running: First wipe at {scheduledStartTime}, repeats every {intervalValue} {cmbIntervalType.SelectedItem}";
            LogMessage($"Scheduler started: First wipe at {scheduledStartTime}, repeating every {wipeInterval}.");
            return true; // Indicate successful setup
        }

        private void ScheduledWipe()
        {
            LogMessage($"Scheduled wipe started at {DateTime.Now}.");
            SecureDeleteFile("C:\\SensitiveData.txt", (int)numPasses.Value);
            LogMessage($"Next wipe in {wipeInterval}.");
        }
        private void btnStartScheduler_Click(object sender, EventArgs e)
        {
            bool isRunning = SetupScheduler();
            if (isRunning)
            {
                lblSchedulerStatus.Text = $"Scheduler Running: First wipe at {dtpStartTime.Value}, repeats every {numIntervalValue.Value} {cmbIntervalType.SelectedItem}";
            }
            else
            {
                lblSchedulerStatus.Text = "Scheduler Not Running.";
            }
        }
        private void btnStopScheduler_Click(object sender, EventArgs e)
        {
            if (wipeTimer != null)
            {
                wipeTimer.Dispose(); // Stop the scheduled timer
                wipeTimer = null;
                lblSchedulerStatus.Text = "Scheduler Stopped.";
                LogMessage("Scheduled wipes stopped by user.");
            }
            else
            {
                MessageBox.Show("Scheduler is not running.", "Stop Scheduler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWipeFreeSpace_Click_1(object sender, EventArgs e)
        {
            passCount = 0;

            if (cmbDriveSelector.SelectedItem == null)
            {
                MessageBox.Show("Please select a drive.");
                return;
            }

            string? selectedDrive = cmbDriveSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDrive))
            {
                MessageBox.Show("Selected drive is invalid.");
                return;
            }
            string driveType = DetectDriveType(selectedDrive);

            if (string.IsNullOrEmpty(driveType) || driveType == "Unknown")
            {
                MessageBox.Show("Unable to determine drive type. Please check the drive and try again.");
                return;
            }

            cmbDriveSelector.Enabled = false; // Disable drive selection during wipe
            numPasses.Enabled = false; // Disable passes selection during wipe
            LogMessage($"Selected drive: {selectedDrive} ({driveType}) for wipe operation.");
            btnWipeFreeSpace.Enabled = false; // Disable button to prevent multiple clicks during wipe

            if (driveType == "USB")
            {
                DialogResult result = MessageBox.Show(
                    $"The selected drive ({selectedDrive}) is a USB drive. Are you sure you want to proceed with wiping free space?",
                    "Confirm USB Wipe",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    LogMessage($"Wipe operation canceled by user for USB drive: {selectedDrive}");
                    return; // Exit function
                }
            }


            if (driveType == "HDD")
            {
                // Proceed with wipe (overwrite free space, TRIM, etc.)
                LogMessage($"Starting wipe on {selectedDrive} ({driveType}).");
                ObfuscateFreeSpace(selectedDrive, 90, 3); // 90% Wipe / DoD 3-pass method
            }
            else if (driveType == "SSD")
            {

                // Proceed with wipe (overwrite free space, TRIM, etc.)
                LogMessage($"Starting wipe on {selectedDrive} ({driveType}).");
                ExecuteTrim(selectedDrive); // ATA Secure Erase for NAND flash wipe
            }
            else
            {
                MessageBox.Show("Drive type unknown. Using Single Pass wipe method.");

                // Proceed with wipe (overwrite free space, TRIM, etc.)
                LogMessage($"Starting wipe on {selectedDrive} ({driveType}).");
                ObfuscateFreeSpace(selectedDrive, 90, 1); // Default single-pass wipe
            }

        }

        private void btnSecureDelete_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SecureDeleteFile(openFileDialog.FileName, (int)numPasses.Value);
            }
        }

        private string DetectDriveType(string driveLetter)
        {
            DriveInfo drive = new DriveInfo(driveLetter);

            if (!drive.IsReady)
                return "Unknown"; // Skip inaccessible drives

            string driveType = "Unknown";

            if (drive.DriveType == DriveType.Removable)
                driveType = "USB";
            else
            {
                string query = "SELECT MediaType FROM Win32_DiskDrive";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject disk in searcher.Get())
                    {
                        string mediaType = disk["MediaType"]?.ToString() ?? "Unknown";

                        if (mediaType.Contains("Solid State"))
                            driveType = "SSD";
                        else if (mediaType.Contains("Fixed hard disk") || mediaType.Contains("HDD"))
                            driveType = "HDD";
                        else
                            driveType = "Unknown";
                    }
                }
            }

            LogMessage($"Drive Detected: {driveLetter} is a {driveType}.");

            return driveType;
        }
        private void ExecuteTrim(string driveLetter)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to run TRIM on {driveLetter}? This process will make deleted data unrecoverable.",
                "Confirm TRIM",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                LogMessage($"TRIM operation on {driveLetter} was canceled by the user.");
                return; // Exit without executing TRIM
            }

            string command = $"powershell.exe -Command \"Optimize-Volume -DriveLetter {driveLetter} -ReTrim -Verbose\"";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                process.WaitForExit();

                // Capture output for logging
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode == 0)
                {
                    LogMessage($"TRIM operation completed successfully on {driveLetter}.");
                    MessageBox.Show($"TRIM completed successfully on {driveLetter}.", "TRIM Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    LogMessage($"Error executing TRIM: {error}");
                    MessageBox.Show($"TRIM failed on {driveLetter}. Error: {error}", "TRIM Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        private DropTargetForm? dropTargetForm;

        private void chkEnableDropTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableDropTarget.Checked)
            {
                if (dropTargetForm == null || dropTargetForm.IsDisposed)
                {
                    dropTargetForm = new DropTargetForm(this); // Pass current Form1 instance
                    dropTargetForm.Show();
                }
            }
            else
            {
                if (dropTargetForm != null && !dropTargetForm.IsDisposed)
                {
                    dropTargetForm.Close();
                    dropTargetForm.Dispose(); // Properly release resources
                    dropTargetForm = null; // Clear reference to prevent reuse of disposed object
                }
            }
        }
       
        private void cmbDriveSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            passCount = 0;
            wipedSpace = 0; // Reset wiped space when changing drives

            if (string.IsNullOrEmpty(cmbDriveSelector.Text))
            {
                MessageBox.Show("Please select a valid drive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedDrive = cmbDriveSelector.Text;

            // Reset wipe progress ONLY if a different drive is selected
            if (selectedDrive != currentWipingDrive)
            {
                wipeSquares = 0; // Clears previous wipe data
                panelDiskUsage.Invalidate();
                panelDiskUsage.Refresh();
            }

            UpdateSummaryLabel();
        }

        private void UpdateSummaryLabel()
        {

            string selectedDrive = cmbDriveSelector.Text;
            currentWipingDrive = selectedDrive; // Store selected drive


            DriveInfo drive = new DriveInfo(selectedDrive);
            totalSpace = drive.TotalSize;
            freeSpace = drive.AvailableFreeSpace;
            if(passCount == 0)
            {
                usedSpace = totalSpace - freeSpace; // Calculate used space only if no passes have been made
                cusedspace = usedSpace;
            }
            else
            {
                usedSpace = wipedSpace; // Use wiped space as the used space after wiping
            }


            // Trigger UI update
            panelDiskUsage.Invalidate();
            panelDiskUsage.Refresh();

            string summaryText = $"Total Space: {totalSpace / (1024 * 1024 * 1024)} GB\n" +
                                 $"Used Space: {cusedspace / (1024 * 1024 * 1024)} GB\n" +
                                 $"Free Space: {freeSpace / (1024 * 1024 * 1024)} GB";

            // ✅ **Ensure UI updates happen safely on the main thread**
            if (lblSummary.InvokeRequired)
            {
                lblSummary.Invoke((Action)(() => lblSummary.Text = summaryText));
            }
            else
            {
                lblSummary.Text = summaryText;
            }
        }

#if false
        private void panelDiskUsage_Paint(object? sender, PaintEventArgs e)
        {
            long usedSquares = (long)Math.Round(totalSquares * (usedSpace / (double)totalSpace));

            Graphics g = e.Graphics;
            int usedCount = 0;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    // Choose color based on usage percentage
                    Color squareColor = usedCount < usedSquares ? Color.IndianRed : Color.LightGreen;

                    // Fill square
                    g.FillRectangle(new SolidBrush(squareColor), x * squareSize, y * squareSize, squareSize, squareSize);

                    // Draw black grid lines (1-pixel border)
                    g.DrawRectangle(Pens.Black, x * squareSize-1, y * squareSize-1, squareSize, squareSize);

                    usedCount++;
                }
            }
        }
#endif
        private void panelDiskUsage_Paint(object? sender, PaintEventArgs e)
        {
            Color fillColor = Color.Yellow;
            Color usedSpaceColor = Color.IndianRed;

            if (currentWipingDrive == null || currentWipingDrive != cmbDriveSelector.Text)
            {
                wipeSquares = 0; // Reset wipe progress if a different drive is selected
            }
            if (numPasses.Value < 1)
            {
                MessageBox.Show("Please select at least one pass for the wipe operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long totalSquares = gridSize * gridSize;  // Total squares in the grid
            long usedSquares = 0;
            // select color based on numPasses value
            if (passCount == 0)
            {
                usedSquares = (long)Math.Round(totalSquares * (usedSpace / (double)(usedSpace + freeSpace)));
            }
            else if (passCount == 1)
            {
                fillColor = Color.Yellow; // Single pass
            }
            else if (passCount == 2)
            {
                fillColor = Color.Orange; // Two passes
            }
            else if (passCount >= 3)
            {
                fillColor = Color.SkyBlue; // Three or more passes
            }

            Graphics g = e.Graphics;
            // ✅ Map wipe progress percentage (0-100%) to wipe squares
            //long wipeSquaresCurrent = (long)Math.Round((totalSquares - usedSquares) * ((double)writtenBytes / freeSpace));
            long wipeSquaresCurrent = (long)Math.Round((totalSquares - usedSquares) * ((double)wipedSpace / freeSpace));

            int count = 0;
            // Draw the grid of squares
            for (int y = 0; y < gridSize; y++)
            // Loop through each row
            {
                for (int x = 0; x < gridSize; x++)
                // Loop through each column
                {
                    // Determine the color for the current square
                    // Use the wipe progress color if passes have been made, otherwise use used space color
                    Color wipeProgressColor = passCount > 0 ? Color.DodgerBlue : usedSpaceColor; 
                    // Calculate the color based on wipe progress
                    Color squareColor = count < usedSquares ? wipeProgressColor :
                                        count < (usedSquares + wipeSquaresCurrent) ? fillColor :
                                        Color.LightGreen;
                    // Fill the square with the determined color
                    g.FillRectangle(new SolidBrush(squareColor), x * squareSize, y * squareSize, squareSize, squareSize);
                    // Draw a black border around the square
                    g.DrawRectangle(Pens.Black, x * squareSize, y * squareSize, squareSize, squareSize);

                    count++;
                }
            }
        }
        

        private void Form1_Leave(object sender, EventArgs e)
        {
            cts.Cancel();
            cmbDriveSelector.Enabled = true; // Re-enable drive selection after wipe
            numPasses.Enabled = true; // Re-enable passes selection after wipe
            btnWipeFreeSpace.Enabled = true; // Re-enable button after wipe
            lblStatus.Text = "Wipe process cancelled.";
            LogMessage("Wipe process cancelled by closing application.");
        }
    }

}
