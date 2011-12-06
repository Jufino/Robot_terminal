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
    public partial class panel : Form
    {
        public panel()
        {
            InitializeComponent();
            //picture box
            robot_obr.Image = velkost_picture_box(robot_obr, 0);
            pictureBox1.Image = velkost_picture_box(pictureBox1, 0);
            //rec buttons
            back_button.Image = velkost_button(back_button, 10);
            live_button.Image = velkost_button(live_button, 10);
            next_button.Image = velkost_button(next_button, 10);
            rec_button.Image = velkost_button(rec_button, 10);
            pause_button.Image = velkost_button(pause_button, 10);
            save_button.Image = velkost_button(save_button, 10);
            open_button.Image = velkost_button(open_button, 10);
            play_button.Image = velkost_button(play_button, 10);
            kamera_rec.create_buffer();
            //group enabled------------------------
            group_automotion.Enabled = true;
            group_compass.Enabled = false;
            group_graphic_position.Enabled = false;
            group_IR_Sensors.Enabled = false;
            group_kicker.Enabled = false;
            group_robot_control.Enabled = false;
            group_espeak.Enabled = false;
            group_led.Enabled = false;
            //-------------------------------------
        }

        socket_com.socket robot_socket = new socket_com.socket();
        
        private void Open_socket_Click(object sender, EventArgs e)
        {
            if (robot_socket.open_socket(textBox_IP.Text, "1213") == false)
            {
                MessageBox.Show("Problem s otvorenim portu");
            }
            else
            {
                robot_socket.receive_timeout = 100;
                robot_socket.send_timeout = 100;
                robot_socket.sendcommanddata("smer", "0");
                robot_socket.sendcommanddata("rych", "200");
                robot_socket.sendcommanddata("prikaz", "L0ed");
                Graficka_aktualizacia.Enabled = true;
                Socket_aktualizacia.Enabled = true;
                //group enabled------------------------
                group_automotion.Enabled = true;
                group_compass.Enabled = true;
                group_graphic_position.Enabled = true;
                group_IR_Sensors.Enabled = true;
                group_kicker.Enabled = true;
                group_robot_control.Enabled = true;
                group_led.Enabled = true;
                //-------------------------------------
                Open_socket.Enabled = false;
                Close_socket.Enabled = true;
            }

        }
        
        private void Close_socket_Click(object sender, EventArgs e)
        {
            robot_socket.sendommand("vyp");     //odosle pocitacu nech sa vypne
            robot_socket.close_socket();        //ukonci spojenie
            //group enabled------------------------
            group_automotion.Enabled = false;
            group_compass.Enabled = false;
            group_graphic_position.Enabled = false;
            group_IR_Sensors.Enabled = false;
            group_kicker.Enabled = false;
            group_robot_control.Enabled = false;
            group_led.Enabled = false;
            group_espeak.Enabled = false;
            //-------------------------------------            
            Open_socket.Enabled = true;
        }
        
        int[] data = new int[20];
        int prijate_data = 0;
        private void Vypis_hodnoty(object sender, EventArgs e)
        {
            
            prijate_data++;
                Sens1.Text = data[0].ToString();                 //Senzor1
                Sens2.Text = data[1].ToString();                 //Senzor2
                Sens3.Text = data[2].ToString();                 //Senzor3
                Sens4.Text = data[3].ToString();                 //Senzor4
                Sens5.Text = data[4].ToString();                 //Senzor5
                Sens6.Text = data[5].ToString();                 //Senzor6
                Sens7.Text = data[6].ToString();                 //Senzor7
                Sens8.Text = data[7].ToString();                  //Senzor8

                //---------------------------------------------------------------
                if (data[8] != 17)
                    max_hod.Text = data[8].ToString();                      //Max hodnota Senzor
                else
                    max_hod.Text = "Null";
                //----------------------------------------------------------------
                Kompas_8bit_box.Text = (data[9] * 2).ToString();              //kompas 8bit
                //----------------------------------------------------------------
                if (data[10] == 1)                                          //senzor kicker
                    Kick_sens.Checked = true;
                else
                    Kick_sens.Checked = false;
                //----------------------------------------------------------------
                senzor_spolu.graf_spolu = odosli_graf_spolu(data, 10);
        }
        
        private void Graficka_aktualizacia_Tick(object sender, EventArgs e)
        {
            #region senzory_graficky
            switch (data[8])
            {
                case 1: 
                    Sens_0.Checked = true; 
                    break;
                case 2:
                    Sens_1.Checked = true;
                    break;
                case 3:
                    Sens_2.Checked = true;
                    break;
                case 4:
                    Sens_3.Checked = true;
                    break;
                case 5:
                    Sens_4.Checked = true;
                    break;
                case 6:
                    Sens_5.Checked = true;
                    break;
                case 7:
                    Sens_6.Checked = true;
                    break;
                case 8:
                    Sens_7.Checked = true;
                    break;
                case 9:
                    Sens_8.Checked = true;
                    break;
                case 10:
                    Sens_9.Checked = true;
                    break;
                case 11:
                    Sens_10.Checked = true;
                    break;
                case 12:
                    Sens_11.Checked = true;
                    break;
                case 13:
                    Sens_12.Checked = true;
                    break;
                case 14:
                    Sens_13.Checked = true;
                    break;
                case 15:
                    Sens_14.Checked = true;
                    break;
                case 16:
                    Sens_15.Checked = true;
                    break;
                case 17:
                    Sens_0.Checked = false;
                    Sens_1.Checked = false;
                    Sens_2.Checked = false;
                    Sens_3.Checked = false;
                    Sens_4.Checked = false;
                    Sens_5.Checked = false;
                    Sens_6.Checked = false;
                    Sens_7.Checked = false;
                    Sens_8.Checked = false;
                    Sens_9.Checked = false;
                    Sens_10.Checked = false;
                    Sens_11.Checked = false;
                    Sens_12.Checked = false;
                    Sens_13.Checked = false;
                    Sens_14.Checked = false;
                    Sens_15.Checked = false;
                    break;
            }
            #endregion
            #region kompas_graficky
            int r = 50;
            float uhol = data[9]*2;
            int pozicia_x = 80;
            int pozicia_y = 80;
            double rad;
            int x1;
            int y1;
            rad = (2 * Math.PI / 360) * uhol;
            System.Drawing.Graphics a = Kompas_graficky.CreateGraphics();
            a.Clear(group_compass.BackColor);
            a.DrawEllipse(new Pen(Color.FromArgb(0,147,221), 5), new Rectangle(pozicia_x - r, pozicia_y - r, r * 2, r * 2));
            y1 = Convert.ToInt16(pozicia_y + r * Math.Cos(rad)*(-1));
            x1 = Convert.ToInt16(pozicia_x + r * Math.Sin(rad));
            a.DrawLine(new Pen(Color.FromArgb(132,194,37), 4), new Point(pozicia_x, pozicia_y), new Point(x1, y1));
            #endregion
        }
        
        private void Socket_aktualizacia_Tick(object sender, EventArgs e)
        {
                string[] xdata = robot_socket.sendcommand_recvarray("data");
                if (xdata != null)
                {
                    for (int x = 0; x < xdata.Length; x++)
                    {
                        data[x] = Convert.ToInt16(xdata[x]);
                    }
                }
                else
                {
                        robot_socket.sendommand("reset");
                }
                this.Invoke(new EventHandler(Vypis_hodnoty));
        }
      
        #region---------------------ovladanie motorov robota--------------------------
        
        private void Hore_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "1");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Hore_vpravo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "2");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Vpravo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "3");
            Socket_aktualizacia.Enabled = true;
        }
      
        private void Dole_vpravo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "4");
            Socket_aktualizacia.Enabled = true;
        }
       
        private void Dole_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "5");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Dole_vlavo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "6");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Vlavo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "7");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Hore_vlavo_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "8");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void Stop_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("smer", "0");
            Socket_aktualizacia.Enabled = true;
        }
        
        private void zmena_rychlosti(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("rych", rychlost_num.Value.ToString());
            Socket_aktualizacia.Enabled = true;
        }
        
        #endregion
        #region --------------------espeak---------------------------

        private void precitaj_box_Click(object sender, EventArgs e)
        {
         //   robot_socket.precitaj_text(Easpeak_text.Text);

        }
        private void txttospeech_Click(object sender, EventArgs e)
        {
            otvor_txt.Filter = "Text (*.txt) |*.txt";
            otvor_txt.FileName = "Vloz text na prehratie";
            otvor_txt.ShowDialog();
            if (otvor_txt.FileName != "Vloz text na prehratie")
            {
                StreamReader s = File.OpenText(otvor_txt.FileName);
                Easpeak_text.Text = s.ReadToEnd();
                if (Easpeak_text.Text.Length >= 1000) 
                {
                    MessageBox.Show("Problem viac ako 999 znakov");
                }
                else
                {
                //    robot_socket.precitaj_text(Easpeak_text.Text);
                }
            }
        }
        #endregion
        //----------------------------------------------------------------

        //-------------------------------------- 
        private void Kick_button_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("prikaz", "kick");
            System.Threading.Thread.Sleep(500);
            Socket_aktualizacia.Enabled = true;
        }
        
        private void LED_vyp_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("prikaz", "LED0");
            Socket_aktualizacia.Enabled = true;
        }
       
        private void LED_zap_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("prikaz", "LED1");
            Socket_aktualizacia.Enabled = true;
        }
        //-------------------------------------- 
        #region nahravanie
        int poc;
       
        int pocdat = 11;
        
        int max_save = 3001;
        
        int[,] data_save = new int[3001, 20];
        
        jpgtogif.save_to_gif kamera_rec = new jpgtogif.save_to_gif();
       
        private void rec_timer_Tick(object sender, EventArgs e)
        {
            if (poc < max_save)
            {
                for (int x = 0; x != pocdat; x++)
                    data_save[poc, x] = data[x];
                time.Text = "0000 / " + poc.ToString();
                timeline.Value = poc;
                kamera.riadenie = 0;
                kamera_rec.add_image(kamera.aktual_picture, poc);
                poc++;
            }
        }
       
        private void play_timer_Tick(object sender, EventArgs e)
        {
            if (poc < max_save)
            {
                for (int x = 0; x != pocdat; x++)
                    data[x] = data_save[poc, x];
                timeline.Value = poc;
                time.Text = "0000 / " + poc.ToString();
                poc++;
                kamera.riadenie = 1;
                kamera.aktual_picture = kamera_rec.read_image(poc);
                this.Invoke(new EventHandler(Vypis_hodnoty));
            }
            else
            {
                poc = 0;
            }
        }
      //----------------------------
        private void play_button_Click(object sender, EventArgs e)
        {
            kamera.riadenie = 1;
            Socket_aktualizacia.Enabled = false;
            play_timer.Enabled = true;
        }
       
        private void rec_button_Click(object sender, EventArgs e)
        {
            kamera.riadenie = 0;
            rec_timer.Enabled = true;
        }
        
        private void next_button_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            if (poc < max_save)
            {
                play_timer.Enabled = false;
                rec_timer.Enabled = false;
                //info------------------------------------
                poc++;
                timeline.Value = poc;
                kamera.riadenie = 1;
                kamera.aktual_picture = kamera_rec.read_image(poc);
                time.Text = "0000 / " + poc.ToString();
                //----------------------------------------
                //Nacitavanie-----------------------------
                for (int x = 0; x != pocdat; x++)
                    data[x] = data_save[poc, x];
                //----------------------------------------
                this.Invoke(new EventHandler(Vypis_hodnoty));
            }
        }
        
        private void back_button_Click(object sender, EventArgs e)
        {
            if (poc != 0)
            {
                Socket_aktualizacia.Enabled = false;
                play_timer.Enabled = false;
                rec_timer.Enabled = false;
                //info------------------------------------
                poc--;
                timeline.Value = poc;
                kamera.riadenie = 1;
                kamera.aktual_picture = kamera_rec.read_image(poc);
                time.Text = "0000 / " + poc.ToString();
                //----------------------------------------
                //Nacitavanie-----------------------------
                for (int x = 0; x != pocdat; x++)
                    data[x] = data_save[poc, x];
                //----------------------------------------
                this.Invoke(new EventHandler(Vypis_hodnoty));
            }
        }
        
        private void pause_button_Click(object sender, EventArgs e)
        {
            kamera.riadenie = 1;
            play_timer.Enabled = false;
            rec_timer.Enabled = false;
            Socket_aktualizacia.Enabled = false;
        }
       
        private void zmena_timeline(object sender, MouseEventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            poc = Convert.ToInt32((float)(3000/timeline.Width+0.5) * e.X);
            if (poc > 3000)
            {
                poc = 3000;
            }
            //----------------------------------------
            timeline.Value = poc;
            kamera.riadenie = 1;
            kamera.aktual_picture = kamera_rec.read_image(poc);
            time.Text = "0000 / " + poc.ToString();
            if (rec_timer.Enabled == false)
            {
                //----------------------------------------
                //Nacitavanie-----------------------------
                for (int x = 0; x != pocdat; x++)
                    data[x] = data_save[poc, x];
                //----------------------------------------
                this.Invoke(new EventHandler(Vypis_hodnoty));
            }
        }
       
        private void live_button_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = true;
            rec_timer.Enabled = false;
            play_timer.Enabled = false;
            kamera.riadenie = 0;
        }
        
        private void save_button_Click(object sender, EventArgs e)
        {
            string[] line = new string[max_save];
            save_stream.Filter = "Stream (*.stream) |*.stream";
            save_stream.FileName = "Uloz stream";
            save_stream.ShowDialog();
            kamera_rec.bitmaps_to_gif(save_stream.FileName.Substring(0, save_stream.FileName.Length - 6) + "gif", kamera_rec.images );
            if (save_stream.FileName != "Uloz stream")
            {
                TextWriter stream_file = new StreamWriter(save_stream.FileName);
                for (int z = 0; z != max_save; z++)
                {
                    for (int x = 0; x != pocdat-1; x++)
                    {
                        line[z] += data_save[z, x] + " ";
                    }
                    line[z] += data_save[z, pocdat];
                    stream_file.WriteLine(line[z]);
                }
                stream_file.Close();
            }
        }
        
        private void open_button_Click(object sender, EventArgs e)
        {
            string[] line = new string[max_save];
            string[] data = new string[pocdat];
            char[] oddelovac = new char[pocdat];
            for (int i=0;i != pocdat-1;i++)
                    oddelovac[i] = ' ';
            otvor_stream.Filter = "Stream (*.stream) |*.stream";
            otvor_stream.FileName = "Otvor stream";
            otvor_stream.ShowDialog();
            if (otvor_stream.FileName != "Otvor stream")
            {
                TextReader stream_file = new StreamReader(otvor_stream.FileName);
                for (int z = 0; z != max_save; z++)
                {
                    line[z] = stream_file.ReadLine();
                    data = line[z].Split(oddelovac);
                    for (int x = 0; x != pocdat; x++)
                    {
                        data_save[z, x] = Convert.ToByte(data[x]);                       
                    }
                }
                stream_file.Close();
            }
        }
        
        #endregion
        //-------------------------------------- 
        #region grafy
        int[] odosli_graf_spolu(int[] data, int end)
        {
            int[] hodnoty = new int[end + 1];
            for (int x = 0; x != end; x++)
            {
                hodnoty[x] = data[x];
            }
            hodnoty[end] = prijate_data;
            return hodnoty;
        }
        
        graf_panel senzor_spolu = new graf_panel();      
        
        private void Graf_spolu_Click(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = true;
            senzor_spolu.Show();
        }
        
        private void graf_compass(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Compass = true;
            senzor_spolu.Show();
        }
        
        private void graf_maxV(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.MaxV = true;
            senzor_spolu.Show();
        }
        
        private void graf_S1(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor1 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S2(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor2 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S3(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor3 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S4(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor4 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S5(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor5 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S6(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor6 = true;
            senzor_spolu.Show();
        }
        
        private void graf_S7(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor7 = true;
            senzor_spolu.Show();
        }
      
        private void graf_S8(object sender, EventArgs e)
        {
            if (senzor_spolu.IsDisposed)
                senzor_spolu = new graf_panel();
            senzor_spolu.Text = "Graph sensors";
            senzor_spolu.all_checked = false;
            senzor_spolu.Senzor8 = true;
            senzor_spolu.Show();
        }
        #endregion
        //-------------------------------------- 
        private void gotoweb(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.prianicslovakia.tym.sk");
        }
     
        Gamepad gamepad = new Gamepad();
        
        string last = "100";
        
        private void Gamepad_timer_Tick(object sender, EventArgs e)
        {
            string now = gamepad.calc_gamepad(32511, 32767, 8);
            if (now != last)
            {
                switch (now)
                {
                    case "z_t_1":
                        if (rychlost_num.Value - 5 >= rychlost_num.Minimum)
                            rychlost_num.Value = rychlost_num.Value - 5; break;
                    case "z_t_2":
                        robot_socket.sendcommanddata("smer", "0");
                        robot_socket.sendcommanddata("prikaz", "kick");
                        System.Threading.Thread.Sleep(300); break;
                    case "z_t_3": robot_socket.sendcommanddata("prikaz", "LED0"); break;
                    case "z_t_4":
                        if (rychlost_num.Value + 5 <= rychlost_num.Maximum)
                            rychlost_num.Value = rychlost_num.Value + 5; break;
                    case "z_t_5": robot_socket.sendcommanddata("prikaz", "obch"); break;
                    case "z_t_6": robot_socket.sendcommanddata("prikaz", "LED1"); break;
                    case "z_t_7": robot_socket.sendcommanddata("smer", "9"); break;
                    case "z_t_8": robot_socket.sendcommanddata("smer", "10"); break;
                    default: robot_socket.sendcommanddata("smer", now); break;
                }
                if (now != "z_t_1" & now != "z_t_4" & now != "z_t_5")
                {
                    last = now;
                    Socket_aktualizacia.Enabled = true;
                }
                else if (now == "z_t_5")
                {
                    Socket_aktualizacia.Enabled = false;
                }
            }
        }

        #region kamera_panel
        
        kamera_panel kamera = new kamera_panel();

        bool kamera_status = false;
        
        private void kamera_start_Click(object sender, EventArgs e)
        {
            kamera.open_camera(textBox_IP.Text);
            if (kamera.IsDisposed)
                kamera = new kamera_panel();      
            kamera.Text = "Kamera_original";
            kamera.Show();
        }

        #endregion

        string iftrue(bool a)
        {
            if (a == true)
                return "1";
            else
                return "0";
        }

        private void zmena_control(object sender, EventArgs e)
            {
                if (radio_gamepad.Checked == true)
                {
                    gamepad.Pripoj_Joystick();
                    Gamepad_timer.Enabled = true;
                    group_robot_control.Enabled = false;
                }
                else
                {
                    Gamepad_timer.Enabled = false;
                    group_robot_control.Enabled = true;
                }
            }
      
        Image velkost_picture_box(PictureBox old,int posun)
            {
                return new Bitmap(old.Image, old.Width-posun, old.Height-posun); 
            }
        
        Image velkost_button(Button old, int posun)
            {
                return new Bitmap(old.Image, old.Width - posun, old.Height - posun);
            }
       
        private void nahodne_Tick(object sender, EventArgs e)
        {
            prijate_data++;
            System.Random a = new Random();
            for (int i = 0; i < 8; i++)
            {
                data[i] = a.Next(0, 255); 
            }
            
            data[8] = a.Next(0, 18);
            data[9] = a.Next(0, 180);
            data[10] = a.Next(0, 1);
            this.Invoke(new EventHandler(Vypis_hodnoty));       
        }

        private void start_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("prikaz", "L1ed");
            Socket_aktualizacia.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Socket_aktualizacia.Enabled = false;
            robot_socket.sendcommanddata("prikaz", "L0ed");
            System.Threading.Thread.Sleep(50);
            robot_socket.sendcommanddata("smer", "0");
            Socket_aktualizacia.Enabled = true;
        }

        private void start_test_Click(object sender, EventArgs e)
        {
            //group enabled------------------------
            group_automotion.Enabled = true;
            group_compass.Enabled = true;
            group_graphic_position.Enabled = true;
            group_IR_Sensors.Enabled = true;
            group_kicker.Enabled = true;
            group_robot_control.Enabled = true;
            group_led.Enabled = true;
            //-------------------------------------
            nahodne.Enabled = true;
            Graficka_aktualizacia.Enabled = true;

        }






    }
}
