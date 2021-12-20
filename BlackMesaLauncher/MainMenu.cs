using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace BlackMesaLauncher
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, System.EventArgs e)
        {
            if (Debugger.IsAttached) // if debugger attached, roll images every 3 seconds
            {
                label4.Visible = true;
                label4.Text = "Debugger attached.";
                Thread t = new Thread(RotateImages);
                t.Start();
            }
            else // otherwise, pick one randomly and stick with it
            {
                Random rnd = new Random();
                int image = rnd.Next(1, 8); // random number between 1 and 7
                switch (image)
                {
                    case 1:
                        BackgroundImage = Properties.Resources.img1;
                        break;
                    case 2:
                        BackgroundImage = Properties.Resources.img2;
                        break;
                    case 3:
                        BackgroundImage = Properties.Resources.img3;
                        break;
                    case 4:
                        BackgroundImage = Properties.Resources.img4;
                        break;
                    case 5:
                        BackgroundImage = Properties.Resources.img5;
                        break;
                    case 6:
                        BackgroundImage = Properties.Resources.img6;
                        break;
                    case 7:
                        BackgroundImage = Properties.Resources.img7;
                        break;
                    default:
                        BackgroundImage = Properties.Resources.img1; // fallback
                        break;
                }
            }

            decimal clockSpeed = 0;
            var searcher = new ManagementObjectSearcher(
                "select MaxClockSpeed from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                clockSpeed = Convert.ToDecimal(0.001f * (uint)item["MaxClockSpeed"]); // gets processor ghz
            }
            var cores = Environment.ProcessorCount;
            decimal ram = 0;
            ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
            foreach (ManagementObject Mobject in Search.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                ram = Convert.ToDecimal(Ram_Bytes / 1073741824);
            }

            if (ram < 6)
            {
                MessageBox.Show("Your RAM is below the minimum system requirements (at least 6 GB).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label4.Visible = true;
                label4.Text = "Warning: RAM below minimum system requirements (at least 6 GB)";
                label4.ForeColor = Color.Red;
            }
            else if (ram < 8)
            {
                label4.Visible = true;
                label4.Text = "Warning: RAM below recommended system requirements (at least 8 GB)";
                label4.ForeColor = Color.Yellow;
            }

            if (clockSpeed < 2.6m)
            {
                MessageBox.Show("Your CPU is below the minimum system requirements (at least 2.6 GHz).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label4.Visible = true;
                label4.Text = "Warning: CPU below minimum system requirements (at least 2.6 GHz)";
                label4.ForeColor = Color.Red;
            }
            else if (clockSpeed < 3.2m)
            {
                label4.Visible = true;
                label4.Text = "Warning: CPU below recommended system requirements (at least 3.2 GHz)";
                label4.ForeColor = Color.Yellow;
            }

            if (cores < 2) // Below minimum system requirements
            {
                MessageBox.Show("Your CPU is below the minimum system requirements (at least 2 cores).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label4.Visible = true;
                label4.Text = "Warning: CPU below minimum system requirements (at least 2 cores)";
                label4.ForeColor = Color.Red;
            }
            else if (cores < 4) // Below recommended system requirments
            {
                label4.Visible = true;
                label4.Text = "Warning: CPU below recommended system requirements (at least 4 cores)";
                label4.ForeColor = Color.Yellow;
            }

            if (!File.Exists("./BMLOptions.json"))
            {
                File.Create("./BMLOptions.json").Dispose();
            }

            var width = Screen.PrimaryScreen.Bounds.Width.ToString();
            var height = Screen.PrimaryScreen.Bounds.Height.ToString();
            File.WriteAllText("./BMLOptions.json", "{\"WorkshopAddons\":true,\"CustomFolder\":false,\"OldUI\":false,\"ScreenWidth\":" +
                                                   width +
                                                   ",\"ScreenHeight\":" + 
                                                   height +
                                                   ",\"LaunchArgs\":\"\"}");
            var originalText = label4.Text;
            var originalColor = label4.ForeColor;
            workshopAddons.Checked = Settings.LoadWA();
            oldUI.Checked = Settings.LoadUI();
            numericUpDown1.Value = Settings.LoadSW();
            numericUpDown2.Value = Settings.LoadSH();
            richTextBox1.Text = Settings.LoadLA();
            if (originalText != "I show up when something happens.")
            {
                label4.Text = originalText;
            }
            else
            {
                label4.Text = "";
            }
            // bypassing unsaved changes thing
            label4.ForeColor = originalColor;

            if (!Directory.Exists("./bms/custom") && !Directory.Exists("./bms/custom-disabled"))
            {
                customFolder.Enabled = false;
                customFolder.Checked = false;
            }
            else if (Directory.Exists("./bms/custom-disabled"))
            {
                customFolder.Enabled = true;
                customFolder.Checked = false;
            }
            else if (Directory.Exists("./bms/custom"))
            {
                customFolder.Enabled = true;
                customFolder.Checked = true;
            }

        }

        private void RotateImages()
        {
            while (true)
            {
                BackgroundImage = Properties.Resources.img1;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img2;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img3;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img4;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img5;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img6;
                Thread.Sleep(3000); // 3 seconds
                BackgroundImage = Properties.Resources.img7;
                Thread.Sleep(3000); // 3 seconds

            }
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Black;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor = Color.White;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Black;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.ForeColor = Color.White;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.ForeColor = Color.Black;
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0); // closes up all remaining loose ends
            Process.GetCurrentProcess().Kill(); // in case it's somehow still alive
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.ForeColor = Color.White;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.ForeColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Contains("-w") || // checks for settings that are already switches in launcher
                richTextBox1.Text.Contains("-h") ||
                richTextBox1.Text.Contains("-workshop_disable") || 
                richTextBox1.Text.Contains("-oldgameui") ||
                richTextBox1.Text.Contains("-newgameui"))
            {
                label4.Visible = true;
                label4.Text = "Arguments conflict with launcher settings. Cannot save.";
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.Visible = true;
                label4.Text = "Saving settings to JSON...";
                label4.ForeColor = Color.White;
                Settings.Save(workshopAddons.Checked, oldUI.Checked, Convert.ToInt32(numericUpDown1.Value),
                    Convert.ToInt32(numericUpDown2.Value), richTextBox1.Text);

                label4.Text = "Checking for custom folder...";
                if (Directory.Exists("./bms/custom") || Directory.Exists("./bms/custom-disabled"))
                {
                    label4.Text = "Checking custom folder status...";
                    if (customFolder.Checked == true)
                    {
                        if (Directory.Exists("./bms/custom-disabled"))
                        {
                            label4.Text = "Moving custom folder...";
                            Directory.Move("./bms/custom-disabled", "./bms/custom");
                        }
                    }
                    else if (customFolder.Checked == false)
                    {
                        if (Directory.Exists("./bms/custom"))
                        {
                            label4.Text = "Moving custom folder...";
                            Directory.Move("./bms/custom", "./bms/custom-disabled");
                        }
                    }

                }
                else
                {
                    label4.Text = "No custom folder, ignoring.";
                }
                label4.Text = "Saved settings successfully.";
            }
        }

        private void SomethingChanged(object sender, EventArgs e)
        {
            label4.Text = "You have unsaved changes!";
            label4.Visible = true;
            label4.ForeColor = Color.Yellow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label4.Text == "You have unsaved changes!")
            {
                if (MessageBox.Show("You have unsaved changes.\nLaunching will use the settings of your last save.\nDo you want to continue?", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string arguments = $"-workshop_disable {ConvertToInt(Settings.LoadWA(), true)} -oldgameui {ConvertToInt(Settings.LoadUI(), false)} -w {Settings.LoadSW()} -h {Settings.LoadSH()} {Settings.LoadLA()}";
                    Process.Start("bms.exe", arguments);
                }
            }
            else
            {
                string arguments = $"-workshop_disable {ConvertToInt(Settings.LoadWA(), true)} -oldgameui {ConvertToInt(Settings.LoadUI(), false)} -w {Settings.LoadSW()} -h {Settings.LoadSH()} {Settings.LoadLA()}";
                Process.Start("bms.exe", arguments);
            }
        }

        private int ConvertToInt(bool boolean, bool reverse)
        {
            if (reverse)
            {
                if (boolean == true)
                {
                    return 0;
                }
                else if (boolean == false)
                {
                    return 1;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            else
            {
                if (boolean == true)
                {
                    return 1;
                }
                else if (boolean == false)
                {
                    return 0;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
        }
    }
}