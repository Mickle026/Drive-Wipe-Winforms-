using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drive_Wipe
{
    public partial class DropTargetForm : Form
    {
        private Form1 mainForm; // Add a reference to the main form
        private Size originalSize;
        private Point pictureBoxOriginalLocation;
        private PictureBox pictureBoxDropTarget;

        public DropTargetForm(Form1 form1) // Accept Form1 instance in the constructor
        {
            InitializeComponent();
            mainForm = form1 ?? throw new ArgumentNullException(nameof(form1));
            Application.AddMessageFilter(new MouseMessageFilter(this));

            this.mainForm = form1; // Assign the passed Form1 instance to the field
            

            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = 110; // Set the width of the form
            this.Height = 180; // Set the height of the form
            originalSize = this.Size; // Store initial size

            this.StartPosition = FormStartPosition.Manual; // Set the start position to manual
                                                           // Fix for CS8602: Ensure Screen.PrimaryScreen is not null before accessing its properties
            if (Screen.PrimaryScreen != null)
            {
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 200, 50); // Position the form at the top right corner
            }
            else
            {
                // Fallback to a default location if PrimaryScreen is null
                this.Location = new Point(0, 0);
            }

            this.BackColor = Color.LightGray; // Set the background color of the form
            this.Opacity = 0.8; // Set the opacity of the form

            this.TransparencyKey = Color.LightGray; // Make the background color transparent
            this.Text = "Secure Shred"; // Set the title of the form
            this.ShowIcon = false; // Hide the icon in the title bar
            this.MaximizeBox = false; // Disable the maximize button
            this.MinimizeBox = false; // Disable the minimize button
            this.FormClosing += (s, e) => e.Cancel = true; // Prevent the form from being closed
            this.ControlBox = false; // Hide the control box (close, minimize, maximize buttons)
            this.TopLevel = true; // Ensure the form is a top-level window
            this.AutoScaleMode = AutoScaleMode.None; // Disable automatic scaling
            this.AutoSize = false; // Disable auto-sizing of the form

            this.TopMost = true; // Always on top
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;

            // Ensure Screen.PrimaryScreen is not null before accessing its properties
            if (Screen.PrimaryScreen != null)
            {
                //this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 200, 50);
                this.Location = new Point(
                                            Screen.PrimaryScreen.WorkingArea.Width - 110, // Keep it on the right
                                            Screen.PrimaryScreen.WorkingArea.Height - this.Height // Move to the bottom
                                        );
            }
            else
            {
                // Fallback to a default location if PrimaryScreen is null
                this.Location = new Point(0, 0);
            }

            // PictureBox setup
            pictureBoxDropTarget = new PictureBox
            {
                Size = new Size(100, 140),
                Image = Image.FromStream(new MemoryStream(Properties.Resources.Shred)), // Convert byte[] to Image
                Location = new Point(0, 0), // Adjust as needed
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent, // Make the background transparent
                Cursor = Cursors.Hand, // Change cursor to hand when hovering over the PictureBox
                // Set the PictureBox properties

                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle,
                AllowDrop = true
            };
            


            pictureBoxDropTarget.DragEnter += PictureBox_DragEnter;
            pictureBoxDropTarget.DragDrop += PictureBox_DragDrop;
            // Add event handlers for mouse enter and leave to show/hide the title bar
            pictureBoxDropTarget.MouseEnter += DropTargetForm_MouseEnter;
            pictureBoxDropTarget.MouseLeave += DropTargetForm_MouseLeave;
            // Add the PictureBox to the form
            this.Controls.Add(pictureBoxDropTarget);

            pictureBoxOriginalLocation = pictureBoxDropTarget.Location; // Store original position

            // Add event handlers for mouse enter and leave to show/hide the title bar
            this.MouseEnter += DropTargetForm_MouseEnter;
            this.MouseLeave += DropTargetForm_MouseLeave;
            
        }
        private void DropTargetForm_MouseEnter(object? sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Show title bar
            this.Size = originalSize; // Reset size after title bar appears
            pictureBoxDropTarget.Location = pictureBoxOriginalLocation;
        }
        private void DropTargetForm_MouseLeave(object? sender, EventArgs e)
        {
            Point cursorPos = PointToClient(Cursor.Position);
            if (!this.ClientRectangle.Contains(cursorPos))
            {
                this.FormBorderStyle = FormBorderStyle.None; // Hide title bar
                this.Size = originalSize; // Reset size after title bar appears
                pictureBoxDropTarget.Location = pictureBoxOriginalLocation;
            }
        }
        private void PictureBox_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) // Ensure e.Data is not null
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PictureBox_DragDrop(object? sender, DragEventArgs e)
        {
            // Safely retrieve the data and handle potential null values
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedItems)
                {
                    foreach (string item in droppedItems)
                    {
                        if (Directory.Exists(item))
                            Form1.SecureDeleteFolder(item, 3, mainForm); // Use the instance of Form1
                        else if (System.IO.File.Exists(item))
                            mainForm.SecureDeleteFile(item, 3); // Use the instance of Form1
                    }
                }
            }
        }

        private void HideTitleBar()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = originalSize;
            pictureBoxDropTarget.Location = pictureBoxOriginalLocation;
        }

        // Custom message filter to track global mouse movement
        public class MouseMessageFilter : IMessageFilter
        {
            private DropTargetForm dropTargetForm;

            public MouseMessageFilter(DropTargetForm form)
            {
                dropTargetForm = form;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x200) // WM_MOUSEMOVE
                {
                    Point cursorPos = Cursor.Position;
                    Rectangle formBounds = dropTargetForm.Bounds;

                    if (!formBounds.Contains(cursorPos))
                    {
                        if (dropTargetForm.IsHandleCreated)
                        {
                            dropTargetForm.BeginInvoke((Action)(() => dropTargetForm.HideTitleBar()));
                        }
                    }
                }
                return false;
            }
        }
    }
    
}
