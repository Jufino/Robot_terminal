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
    
        
        socket_com.socket kamera = new socket_com.socket();

        Image obrazok;

        private void kamera_Tick(object sender, EventArgs e)
        {
            kamera_timer.Enabled = false;
            if (riadenie_povel == 0)
            {
                kamera.gafuso_send_data("img");
                obrazok = kamera.recv_picture();
            }
            pictureBox1.Image = obrazok;
            kamera_timer.Enabled = true;
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
        string ip_kamera = "192.168.0.2";
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            kamera.socket_open(ip_kamera, "1212");
            kamera_timer.Enabled = true;
            pictureBox1.Size = this.Size;
            kamera.picture_width = pictureBox1.Size.Width - 40;
            kamera.picture_height = pictureBox1.Size.Height - 60;
        }
        public string Ip_adress_kamera
        {
            get
            {
                return ip_kamera;
            }
            set
            {
                ip_kamera = value;
            }
        }
    }
}
