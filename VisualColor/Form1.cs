using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace VisualColor
{
    public partial class Form1 : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);


        public Form1()
        {
            InitializeComponent();
            UpdateFormRegion();

            this.TopMost = true;

            panel1.MouseDown += new MouseEventHandler(Panel_MouseDown);
            panel1.MouseMove += new MouseEventHandler(Panel_MouseMove);
            panel1.MouseUp += new MouseEventHandler(Panel_MouseUp);

            panel3.Visible = false;
            panel2.Visible = false;


        }


        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void UpdateFormRegion()
        {
            GraphicsPath path = new GraphicsPath();
            int cornerRadius = 18; 

            path.AddArc(new Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90);
            path.AddArc(new Rectangle(this.Width - cornerRadius, 0, cornerRadius, cornerRadius), 270, 90);
            path.AddArc(new Rectangle(this.Width - cornerRadius, this.Height - cornerRadius, cornerRadius, cornerRadius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - cornerRadius, cornerRadius, cornerRadius), 90, 90);
            path.CloseAllFigures();

            this.Region = new Region(path);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "VisualColor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 10;
            timer.Tick += (s, ev) =>
            {

                if (Control.MouseButtons == MouseButtons.Left)
                {
                    timer.Stop();

                    GetCursorPos(out Point p);

                    IntPtr hdc = GetWindowDC(IntPtr.Zero);
                    uint pixel = GetPixel(hdc, p.X, p.Y);
                    ReleaseDC(IntPtr.Zero, hdc);

                    Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                                                 (int)(pixel & 0x0000FF00) >> 8,
                                                 (int)(pixel & 0x00FF0000) >> 16);

                    if (checkBox1.Checked)
                    {
                        label2.Text = $"{color.R}; {color.G}; {color.B}";
                    }
                    else if (checkBox3.Checked)
                    {
                        label2.Text = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                    }
                    if (checkBox2.Checked)
                    {
                        label2.Text = $"{color.R}, {color.G}, {color.B}";
                    }

                    label2.BackColor = color;
                }

            };
            timer.Start();
        }


        private void label2_Click(object sender, EventArgs e)
        {

            Clipboard.SetText(label2.Text);
            toolTip1.Show("Copied!", label2, 0, 15, 500); 
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
            else if (!checkBox2.Checked && !checkBox3.Checked)
            {
                checkBox1.Checked = true; 
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
            else if (!checkBox1.Checked && !checkBox3.Checked)
            {
                checkBox2.Checked = true; 
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
            else if (!checkBox1.Checked && !checkBox2.Checked)
            {
                checkBox3.Checked = true; 
            }
        }



        private void label4_Click(object sender, EventArgs e)
        {

            string url = "https://github.com/teomanasrintalih";


            System.Diagnostics.Process.Start(url);
        }

    }
}
