using System;
using System.Drawing;
using System.IO;
using IronOcr;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace sugarloaf_screen_capture {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            try { display_text_pass.Text = File.ReadAllText("../../config/sugarloaf_screen_display_text_pass.txt"); } catch { }
            try { display_text_false.Text = File.ReadAllText("../../config/sugarloaf_screen_display_text_false.txt"); } catch { }
            try { ROI_text.Text = File.ReadAllText("../../config/sugarloaf_screen_roi_text.txt"); } catch { }
        }

        Form fff;
        Graphics g;
        Pen myPen;
        bool flag_pen = false;
        Point StartPoint, StopPoint;
        Point roi_point;
        Size roi_size;
        private void button1_Click(object sender, EventArgs e) {
            this.Hide();
            System.Threading.Thread.Sleep(100);
            fff = new Form();
            fff.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            fff.Location = new Point(0, 0);
            fff.ControlBox = false;
            fff.TransparencyKey = Color.Red;
            fff.BackColor = Color.Red;
            //fff.Opacity = 0.01;
            fff.Cursor = Cursors.Cross;
            g = fff.CreateGraphics();
            myPen = new Pen(Color.Blue);
            myPen.Width = 2;
            fff.MouseDown += new MouseEventHandler(MouseDown_display);
            fff.MouseMove += new MouseEventHandler(mouseMove_display);
            fff.MouseUp += new MouseEventHandler(mouseUp_display);
            fff.Show();
        }
        private void button2_Click(object sender, EventArgs e) {
            this.Hide();
            System.Threading.Thread.Sleep(100);
            fff = new Form();
            fff.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            fff.Location = new Point(0, 0);
            fff.ControlBox = false;
            fff.TransparencyKey = Color.Red;
            fff.BackColor = Color.Red;
            //fff.Opacity = 0.01;
            fff.Cursor = Cursors.Cross;
            g = fff.CreateGraphics();
            myPen = new Pen(Color.Blue);
            myPen.Width = 2;
            fff.MouseDown += new MouseEventHandler(MouseDown_roi);
            fff.MouseMove += new MouseEventHandler(mouseMove_roi);
            fff.MouseUp += new MouseEventHandler(mouseUp_roi);
            fff.Show();
        }
        private void mouseMove_display(object sender, MouseEventArgs e) {
            if (flag_pen != true) return;
            g.Clear(Color.Red);
            int pointMin_x = ((StartPoint.X + Control.MousePosition.X) - Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(StartPoint.X - Control.MousePosition.X), 2)))) / 2;
            int pointMin_y = ((StartPoint.Y + Control.MousePosition.Y) - Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(StartPoint.Y - Control.MousePosition.Y), 2)))) / 2;
            g.DrawRectangle(myPen, pointMin_x - 10, pointMin_y - 10, Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(Control.MousePosition.X - StartPoint.X), 2))) + 4, Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(Control.MousePosition.Y - StartPoint.Y), 2)) + 4));
        }
        private void MouseDown_display(object sender, MouseEventArgs e) {
            StartPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            File.WriteAllText("../../config/sugarloaf_screen_point_start_display_x.txt", Control.MousePosition.X.ToString());
            File.WriteAllText("../../config/sugarloaf_screen_point_start_display_y.txt", Control.MousePosition.Y.ToString());
            flag_pen = true;
        }
        private void mouseUp_display(object sender, MouseEventArgs e) {
            File.WriteAllText("../../config/sugarloaf_screen_point_stop_display_x.txt", Control.MousePosition.X.ToString());
            File.WriteAllText("../../config/sugarloaf_screen_point_stop_display_y.txt", Control.MousePosition.Y.ToString());
            flag_pen = false;
            fff.Close();
            System.Threading.Thread.Sleep(100);
            this.Show();
        }
        private void mouseMove_roi(object sender, MouseEventArgs e) {
            if (flag_pen != true) return;
            g.Clear(Color.Red);
            int pointMin_x = ((StartPoint.X + Control.MousePosition.X) - Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(StartPoint.X - Control.MousePosition.X), 2)))) / 2;
            int pointMin_y = ((StartPoint.Y + Control.MousePosition.Y) - Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(StartPoint.Y - Control.MousePosition.Y), 2)))) / 2;
            g.DrawRectangle(myPen, pointMin_x - 10, pointMin_y - 10, Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(Control.MousePosition.X - StartPoint.X), 2))) + 4, Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(Control.MousePosition.Y - StartPoint.Y), 2)) + 4));
        }
        private void MouseDown_roi(object sender, MouseEventArgs e) {
            StartPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            File.WriteAllText("../../config/sugarloaf_screen_point_start_roi_x.txt", Control.MousePosition.X.ToString());
            File.WriteAllText("../../config/sugarloaf_screen_point_start_roi_y.txt", Control.MousePosition.Y.ToString());
            flag_pen = true;
        }
        private void mouseUp_roi(object sender, MouseEventArgs e) {
            Image img;
            File.WriteAllText("../../config/sugarloaf_screen_point_stop_roi_x.txt", Control.MousePosition.X.ToString());
            File.WriteAllText("../../config/sugarloaf_screen_point_stop_roi_y.txt", Control.MousePosition.Y.ToString());
            roi_size = new Size(Convert.ToInt32(Math.Sqrt(Math.Pow(StartPoint.X - Control.MousePosition.X, 2))), Convert.ToInt32(Math.Sqrt(Math.Pow(StartPoint.Y - Control.MousePosition.Y, 2))));
            using (Bitmap bitmap = new Bitmap(roi_size.Width, roi_size.Height)) {
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.CopyFromScreen(StartPoint, Point.Empty, bitmap.Size);
                }
                img = new Bitmap((Image)bitmap);
            }
            img.Save("../../config/image_ref.png", ImageFormat.Png);
            flag_pen = false;
            fff.Close();
            System.Threading.Thread.Sleep(100);
            this.Show();
        }

        Form display = new Form();
        Label label = new Label();
        private void runToolStripMenuItem_Click(object sender, EventArgs e) {
            double a = 0, b = 0, x = 0, y = 0;
            try { a = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_start_display_x.txt")); } catch { }
            try { x = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_start_display_y.txt")); } catch { }
            try { b = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_stop_display_x.txt")); } catch { }
            try { y = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_stop_display_y.txt")); } catch { }
            display.Show();
            display.FormBorderStyle = FormBorderStyle.None;
            display.Location = new Point(Convert.ToInt32(((a + b) - Math.Sqrt(Math.Pow((a - b), 2))) / 2), Convert.ToInt32(((x + y) - Math.Sqrt(Math.Pow((x - y), 2))) / 2));
            display.Size = new Size(Convert.ToInt32(Math.Sqrt(Math.Pow(a - b, 2))), Convert.ToInt32(Math.Sqrt(Math.Pow(x - y, 2))));
            FontFamily fontFamily = new FontFamily("Arial");
            label.Font = new Font(fontFamily, 40, FontStyle.Bold, GraphicsUnit.Pixel);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 0);
            label.Size = new Size(display.Size.Width, display.Size.Height);
            label.Text = display_text_pass.Text;
            display.Controls.Add(label);

            try { a = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_start_roi_x.txt")); } catch { }
            try { x = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_start_roi_y.txt")); } catch { }
            try { b = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_stop_roi_x.txt")); } catch { }
            try { y = Convert.ToInt32(File.ReadAllText("../../config/sugarloaf_screen_point_stop_roi_y.txt")); } catch { }
            roi_point.X = Convert.ToInt32(((a + b) - Math.Sqrt(Math.Pow((a - b), 2))) / 2);
            roi_point.Y = Convert.ToInt32(((x + y) - Math.Sqrt(Math.Pow((x - y), 2))) / 2);
            roi_size = new Size(Convert.ToInt32(Math.Sqrt(Math.Pow(a - b, 2))), Convert.ToInt32(Math.Sqrt(Math.Pow(x - y, 2))));

            image_sup = new Bitmap(Image.FromFile("../../config/image_ref.png"));

            //Application.Idle += loop;
            backgroundWorker1.RunWorkerAsync();
        }
        private void display_text_Click(object sender, EventArgs e) {
            while (true) {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ข้อความ", "text", display_text_pass.Text, 500, 300);
                if (input == "") return;
                try {
                    File.WriteAllText("../../config/sugarloaf_screen_display_text_pass.txt", input);
                    try { display_text_pass.Text = File.ReadAllText("../../config/sugarloaf_screen_display_text_pass.txt"); } catch { }
                } catch (Exception) {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
        }
        private void display_text_false_Click(object sender, EventArgs e) {
            while (true) {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ข้อความ", "text", display_text_false.Text, 500, 300);
                if (input == "") return;
                try {
                    File.WriteAllText("../../config/sugarloaf_screen_display_text_false.txt", input);
                    try { display_text_false.Text = File.ReadAllText("../../config/sugarloaf_screen_display_text_false.txt"); } catch { }
                } catch (Exception) {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
        }

        private void ROI_text_Click(object sender, EventArgs e) {
            while (true) {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ข้อความ", "text", ROI_text.Text, 500, 300);
                if (input == "") return;
                try {
                    File.WriteAllText("../../config/sugarloaf_screen_roi_text.txt", input);
                    try { ROI_text.Text = File.ReadAllText("../../config/sugarloaf_screen_roi_text.txt"); } catch { }
                } catch (Exception) {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
        }

        Bitmap image;
        Bitmap image_sup;

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            while (true) {
                using (Bitmap bitmap = new Bitmap(roi_size.Width, roi_size.Height)) {
                    using (Graphics g = Graphics.FromImage(bitmap)) {
                        g.CopyFromScreen(roi_point, Point.Empty, bitmap.Size);
                    }
                    image = new Bitmap((Image)bitmap);
                }
                pictureBox1.Image = image;
                Color img1, img2;
                bool flag_img = true;
                for (int i = 0; i < image.Width; i += 1) {
                    for (int j = 0; j < image.Height; j += 1) {
                        img1 = image_sup.GetPixel(i, j);
                        img2 = image.GetPixel(i, j);
                        if (img1 != img2) {
                            i = image.Width;
                            flag_img = false;
                            break;
                        }
                    }
                }
                if (flag_img) {
                    backgroundWorker1.ReportProgress(0);
                } else {
                    backgroundWorker1.ReportProgress(1);
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == 0) {
                display.BackColor = Color.Lime;
                label.Text = display_text_pass.Text;
            } else if (e.ProgressPercentage == 1){
                display.BackColor = Color.Red;
                label.Text = display_text_false.Text;
            }
        }

        private void loop(object sender, EventArgs e) {
            using (Bitmap bitmap = new Bitmap(roi_size.Width, roi_size.Height)) {
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.CopyFromScreen(roi_point, Point.Empty, bitmap.Size);
                }
                image = new Bitmap((Image)bitmap);
            }
            pictureBox1.Image = image;
            Color img1, img2;
            bool flag_img = true;
            for (int i = 0; i < image.Width; i += 1) {
                for (int j = 0; j < image.Height; j += 1) {
                    img1 = image_sup.GetPixel(i, j);
                    img2 = image.GetPixel(i, j);
                    if (img1 != img2) {
                        i = image.Width;
                        flag_img = false;
                        break;
                    }
                }
            }
            if (flag_img) {
                display.BackColor = Color.Lime;
                label.Text = display_text_pass.Text;
            } else {
                display.BackColor = Color.Red;
                label.Text = display_text_false.Text;
            }
            //AdvancedOcr Ocr = new AdvancedOcr() {
            //    CleanBackgroundNoise = true,
            //    EnhanceContrast = true,
            //    EnhanceResolution = true,
            //    Language = IronOcr.Languages.English.OcrLanguagePack,
            //    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
            //    ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
            //    DetectWhiteTextOnDarkBackgrounds = false,
            //    RotateAndStraighten = true,
            //    ReadBarCodes = false,
            //    ColorDepth = 4
            //};
            //OcrResult Results = Ocr.Read(image);
            //if (Results.ToString().Contains(ROI_text.Text)) {
            //    display.BackColor = Color.Lime;
            //    label.Text = display_text_pass.Text;
            //} else {
            //    display.BackColor = Color.Red;
            //    label.Text = display_text_false.Text;
            //}
        }
    }
}
