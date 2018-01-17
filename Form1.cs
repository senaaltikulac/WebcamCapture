using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WebcamCapture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } 
        private FilterInfoCollection webcam;  // bilgisayar içinde bulunan kameraları tutuyor
        private VideoCaptureDevice cam;  // bizim seçeceğimiz aygıtı tutuyor.
        private void Form1_Load(object sender, EventArgs e)
        {
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo item in webcam) // var olan aygıtları combobox a ekliyoruz.
            {
                comboBox1.Items.Add(item.Name);

            }
            comboBox1.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SaveFileDialog swf = new SaveFileDialog(); // çekilen resmi kaydediyoruz.
            swf.Filter = "(*jpg | *.jpg| *.bmp | *.bmp";

            DialogResult dialog=swf.ShowDialog();
            if(dialog ==DialogResult.OK)
            {
                pictureBox1.Image.Save(swf.FileName);
            }
        }
        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();//kameradan alınan görüntüyü picturebox a atar
            pictureBox1.Image = bmp; // kameradaki görüntüyü ekler
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (cam.IsRunning)
            {
                cam.Stop();
                pictureBox1.Image = null; // picturebox ı boşaltır
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString); //cam e comboboxtan seçilmiş olanı kameraya atar
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame); // 
            cam.Start(); // kamerayı başlatır.
        }
    }
}
