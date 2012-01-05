using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace robot
{
    public partial class kamera_panel : Form
    {
        public kamera_panel()
        {
            InitializeComponent();
        }
        
        public void open_camera(string ip_kamera)
        {
                kamera.socket_open(ip_kamera,"1212");
                kamera.receive_timeout = 10;
                kamera.send_timeout = 10;
                kamera_timer.Enabled = true;
                fps_timer.Enabled = true;
        }
        
        socket_com.socket kamera = new socket_com.socket();

        Image obrazok;
        int fps=0;

        private void kamera_Tick(object sender, EventArgs e)
        {
            pictureBox1.Size = this.Size;
            kamera.picture_width = pictureBox1.Size.Width-40;
            kamera.picture_height = pictureBox1.Size.Height-60;
            if (riadenie_povel == 0) obrazok = kamera.recv_picture();
            pictureBox1.Image = obrazok;
            fps++;
        }
        
        public Image aktual_picture
        {
            get
            {
                return obrazok;
            }
            set
            {
                obrazok = value;
            }
        }

        int riadenie_povel = 0;

        public int riadenie
        {
            set
            {
                riadenie_povel = value;
            }
            get
            {
                return riadenie_povel;
            }
        }

        private void vypnute_okno(object sender, FormClosedEventArgs e)
        {
            kamera.socket_close();
        }

        private void fps_timer_Tick(object sender, EventArgs e)
        {
            this.Text = (fps*10).ToString();
            fps = 0;
        }
    }
}
