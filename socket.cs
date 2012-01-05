using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace socket_com
{
    class socket
    {
        private string gafuso_code(string[] data_vstup)
        {
            string data_vystup = "";
            int size = data_vstup.Length;
            int poc_char_pocitadlo = 0;
            int byte_for_dat=1;
            int byte_for_char=1;
            for (int o = 8; size > System.Math.Pow(2, o); o = o + 8) byte_for_dat = o / 8;
            for (int o = 8; data_vstup.Max().Length > System.Math.Pow(2, o); o = o + 8) byte_for_char = o / 8;
            data_vystup += Convert.ToChar(byte_for_dat);
            data_vystup += Convert.ToChar(byte_for_char);
            try
            {
                for (int o = 0; o != byte_for_dat; o++)
                {
                    data_vystup += Convert.ToChar((size >> poc_char_pocitadlo) & 0xFF);
                    poc_char_pocitadlo += 8;
                }
                for (int i = 0; i != size; i++)
                {
                    poc_char_pocitadlo = 0;     
                    for (int o = 0; o != byte_for_char; o++)
                    {
                        data_vystup += Convert.ToChar((data_vstup[i].Length >> poc_char_pocitadlo) & 0xFF);
                        poc_char_pocitadlo += 8;
                    }
                    for (int z = 0; z != data_vstup[i].Length; z++)
                    {
                        data_vystup += data_vstup[i][z];
                    }
                    //--------------------------------------------------
                }
                return data_vystup;
            }
            catch
            {
                return null;
            }
        }
        private string[] gafuso_decode(string data_vstup)
        {
            int posun = 0;
            int size = 0;
            int poc_znakov = 0;
            int poc_char_pocitadlo = 0;
            try
            {
                int byte_for_dat = data_vstup[posun];
                int byte_for_char = data_vstup[posun+1];
                posun += 2;
                //nacita pocet dat--------------------
                for (int o = 0; o != byte_for_dat; o++)
                {
                    size += (data_vstup[posun] << poc_char_pocitadlo);
                    poc_char_pocitadlo += 8;
                    posun++;
                }
                string[] data_vystup = new string[size];
                //------------------------------------
                for (int i = 0; i != size; i++)
                {
                    //------------------------------------
                    poc_znakov = 0;
                    poc_char_pocitadlo = 0;
                    //nacita pocet znakov v nasledujucom data
                    for (int o = 0; o != byte_for_char; o++)
                    {
                        poc_znakov += (data_vstup[posun] << poc_char_pocitadlo);
                        poc_char_pocitadlo += 8;
                        posun++;
                    }
                    //prijma znaky----------------------
                    for (int z = 0; z != poc_znakov; z++)
                    {
                        data_vystup[i] += data_vstup[posun];
                        posun++;
                    }
                    //------------------------------------------
                }
                return data_vystup;
            }
            catch
            {
                return null;
            }
        }
        //-----------------------------------------------------------------
        private Socket socket_client;
        //-----------------------------------------------------------------
        int data_recv_buffer = 300000;

        public bool socket_open(String IP, String Port)
        {
            try
            {
                socket_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int alPort = System.Convert.ToInt16(Port, 10);
                System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(IP);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                socket_client.Connect(remoteEndPoint);
                return true;

            }
            catch
            {
                return false;
            }

        }

        public bool socket_close()
        {
            try
            {
                socket_client.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string socket_recv_ascii()
        {
            int receivedBytesLen = 0;
            try
            {
                byte[] buffer = new byte[data_recv_buffer];
                receivedBytesLen = socket_client.Receive(buffer);
                return System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytesLen);
            }
            catch
            {
                return null;
            }
        }

        private byte[] socket_recv_bytes()
        {
            byte[] buffer = new byte[data_recv_buffer];
            try
            {
                socket_client.Receive(buffer);
                return buffer;
            }
            catch
            {
                return null;
            }
        }

        private bool socket_send_string(Object objData)
        {
            try
            {
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                socket_client.Send(byData);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void gafuso_send_data(string data)
        {
            string[] pomocny = new string[1];
            pomocny[0] = data;
            socket_send_string(gafuso_code(pomocny));
        }
 
        public void gafuso_send_array(string[] data)
        {
            socket_send_string(gafuso_code(data));
        }

        public string[] gafuso_recv_array()
        {
            return gafuso_decode(socket_recv_ascii());
        }

        int picturewidth = 320;
        int pictureheight = 160;
        Bitmap picture;

        //nieje vyskusane
        public Bitmap recv_picture()
        {
            try
            {
                byte[] data_prijem = socket_recv_bytes();
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(data_prijem);
                picture = new Bitmap(img, picturewidth, pictureheight);
            }
            catch { }
            return picture;
        }

        public int data_buffer
        {
            get
            {
                return data_recv_buffer;
            }
            set
            {
                data_recv_buffer = value;
            }
        }

        public int picture_width
        {
            get
            {
                return picturewidth;
            }
            set
            {
                picturewidth = value;
            }
        }
       
        public int receive_timeout
        {
            get
            {
                return socket_client.ReceiveTimeout;
            }
            set
            {
                socket_client.ReceiveTimeout = value;
            }
        }
       
        public int send_timeout
        {
            get
            {
                return socket_client.SendTimeout;
            }
            set
            {
                socket_client.SendTimeout = value;
            }
        }
      
        public int picture_height
        {
            get
            {
                return pictureheight;
            }
            set
            {
                pictureheight = value;
            }
        }
    }
}
