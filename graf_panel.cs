using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace robot
{
    public partial class graf_panel : Form
    {
        public graf_panel()
        {
            InitializeComponent();
        }

        public bool Senzor1
        {
            set { S0_check.Checked = value; }
        }
        public bool Senzor2
        {
            set { S1_check.Checked = value; }
        }
        public bool Senzor3
        {
            set { S2_check.Checked = value; }
        }
        public bool Senzor4
        {
            set { S3_check.Checked = value; }
        }
        public bool Senzor5
        {
            set { S4_check.Checked = value; }
        }
        public bool Senzor6
        {
            set { S5_check.Checked = value; }
        }
        public bool Senzor7
        {
            set { S6_check.Checked = value; }
        }
        public bool Senzor8
        {
            set { S7_check.Checked = value; }
        }
        public bool Compass
        {
            set { compass_check.Checked = value; }
        }
        public bool MaxV
        {
            set {  maxV_check.Checked = value; }
        }
        public bool all_checked
        {
            set { 
                S0_check.Checked = value;
                S1_check.Checked = value;
                S2_check.Checked = value;
                S3_check.Checked = value;
                S4_check.Checked = value;
                S5_check.Checked = value;
                S6_check.Checked = value;
                S7_check.Checked = value;
                compass_check.Checked = value;
                maxV_check.Checked = value;
            }
        }

        public int[] graf_spolu
        {
            set
            {
                pictureBox1.Image = graph_10hod(pictureBox1.Size.Width, pictureBox1.Size.Height, value, 30, 2,value[10]);
            }
        }
        int[,] buffer = new int[10,51];
        int velkost_buffer = 50;
        Bitmap graph_10hodx(int width, int height, int[] data,int rozlisenie,int zvecsi_y,int recvdat)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            height -= 20;
            width -= 20;
            #region os_y
            g.DrawLine(new Pen(Color.Red, 2), new Point(40, 0), new Point(40, height));
            for (int z = 0; z < height; z = z + rozlisenie)
            {
                g.DrawString((z*zvecsi_y).ToString(), new Font("Times New Roman", 10), Brushes.Red, new PointF(0, height - z-10));
            }
            #endregion
            #region os_x
            g.DrawLine(new Pen(Color.Red, 2), new Point(40, height), new Point(width + 20, height));
            g.DrawString((recvdat - 50).ToString(), new Font("Times New Roman", 10), Brushes.Red, new PointF(20, height+5));
            g.DrawString((recvdat - 25).ToString(), new Font("Times New Roman", 10), Brushes.Red, new PointF(width/2, height + 5));
            g.DrawString((recvdat).ToString(), new Font("Times New Roman", 10), Brushes.Red, new PointF(width-10, height + 5));
            #endregion
            //-------------------------
            int last_x;
            int last_y;
            int x;
            int y;
            //-------------------------
            for (int z = 0; z < data.Length-1; z++)
            {
                x = 0;
                for (int i = 0; i < 50; i++)
                    buffer[z, i] = buffer[z, i + 1];
                buffer[z, 50] = data[z];
                for (int i = 0; i < 50; i++)
                {
                    last_x = x;
                    last_y = height - Convert.ToInt32(buffer[z, i]) / zvecsi_y;
                    x = x + (width / 51);
                    y = height - Convert.ToInt32(buffer[z, i + 1]) / zvecsi_y;
                    switch(z){
                        case 0: if (S0_check.Checked == true) { g.DrawLine(new Pen(Color.Black, 2), new Point(last_x+40, last_y), new Point(x+40, y));        }   break;
                        case 1: if (S1_check.Checked == true) { g.DrawLine(new Pen(Color.Green, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 2: if (S2_check.Checked == true) { g.DrawLine(new Pen(Color.Gray, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 3: if (S3_check.Checked == true) { g.DrawLine(new Pen(Color.Gold, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 4: if (S4_check.Checked == true) { g.DrawLine(new Pen(Color.Red, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 5: if (S5_check.Checked == true) { g.DrawLine(new Pen(Color.Purple, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 6: if (S6_check.Checked == true) { g.DrawLine(new Pen(Color.YellowGreen, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 7: if (S7_check.Checked == true) { g.DrawLine(new Pen(Color.Blue, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 8: if (maxV_check.Checked == true) { g.DrawLine(new Pen(Color.SaddleBrown, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                        case 9: if (compass_check.Checked == true) { g.DrawLine(new Pen(Color.Teal, 2), new Point(last_x + 40, last_y), new Point(x + 40, y)); } break;
                    }
                }
            }
            return bmp;
        }
        Bitmap graph_10hod(int width, int height, int[] data, int rozlisenie, int zvecsi_y, int recvdat)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            height -= 40;
            width -= 40;
            #region os_y
            g.DrawLine(new Pen(Color.Red, 2), new Point(40, 0), new Point(40, height));
            #endregion
            #region os_x
            g.DrawLine(new Pen(Color.Red, 2), new Point(40, height), new Point(width + 40, height));
            #endregion
            //-------------------------
            for (int z = 0; z < data.Length - 1; z++)
            {
                int x=30;
                int y=0;
                for (int i = 0; i < velkost_buffer; i++)
                    buffer[z, i] = buffer[z, i + 1];
                buffer[z, velkost_buffer] = data[z];
                for (int i = 0; i < velkost_buffer; i++)
                {
                    #region x_zlozka
                    x = x + (width / velkost_buffer+1);
                    int x_start = x;
                    int x_end = x + (width / velkost_buffer + 1);
                    #endregion
                    #region y_zlozka
                    y = height - buffer[z, i + 1] / zvecsi_y;
                    int y_start = height - buffer[z, i] / zvecsi_y;
                    int y_end = y;
                    #endregion
                    #region vykreslovanie
                    switch (z)
                    {
                        case 0: if (S0_check.Checked == true) { g.DrawLine(new Pen(Color.Black, 2), new Point(x_start, y_start ), new Point(x_end, y_end)); } break;
                        case 1: if (S1_check.Checked == true) { g.DrawLine(new Pen(Color.Green, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 2: if (S2_check.Checked == true) { g.DrawLine(new Pen(Color.Gray, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 3: if (S3_check.Checked == true) { g.DrawLine(new Pen(Color.Gold, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 4: if (S4_check.Checked == true) { g.DrawLine(new Pen(Color.Red, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 5: if (S5_check.Checked == true) { g.DrawLine(new Pen(Color.Purple, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 6: if (S6_check.Checked == true) { g.DrawLine(new Pen(Color.YellowGreen, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 7: if (S7_check.Checked == true) { g.DrawLine(new Pen(Color.Blue, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 8: if (maxV_check.Checked == true) { g.DrawLine(new Pen(Color.SaddleBrown, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                        case 9: if (compass_check.Checked == true) { g.DrawLine(new Pen(Color.Teal, 2), new Point(x_start, y_start), new Point(x_end, y_end)); } break;
                    }
                    #endregion
                }
            }
            return bmp;
        }   
    }
}
