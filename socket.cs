using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace socket_com
{
    /// <summary>
    /// kniznica urcena pre komunikaciu z beagle cez klastrovy protokol
    /// </summary>
    class socket
    {
        //staticke funkcie - funkcie pre dekodovanie
        static int byte_for_dat = 1;		//2 na bit_for_dat = pocet dat ktore sa daju preniest
        static int byte_for_char = 1;		//2 na bit_for_char = pocet znakov v jednotlivych datach
        /// <summary>
        /// coding data protocol
        /// </summary>
        /// <param name="data_vstup">data na kodovanie typu string[]</param>
        private string code(string[] data_vstup)
        {
            string data_vystup = "";
            int size = data_vstup.Length;
            //urcuje pocet dat-----------------------
            int poc_char_pocitadlo = 0;
            try
            {
                for (int o = 0; o != byte_for_dat; o++)
                {
                    data_vystup += Convert.ToChar((size >> poc_char_pocitadlo) & 0xFF);
                    poc_char_pocitadlo += 8;
                }
                //----------------------------------------
                for (int i = 0; i != size; i++)
                {
                    //pocet znakov v nasledujucom data------------------
                    poc_char_pocitadlo = 0;
                    for (int o = 0; o != byte_for_char; o++)
                    {
                        data_vystup += Convert.ToChar((data_vstup[i].Length >> poc_char_pocitadlo) & 0xFF);
                        poc_char_pocitadlo += 8;
                    }
                    //vkladanie data do znakov--------------------------
                    for (int z = 0; z != data_vstup[i].Length; z++)
                    {
                        data_vystup += data_vstup[i][z];	//vlozi hodnotu znaku
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
        //dekoduje data zakodovane v klastrovom protokole       
        /// <summary>
        /// decoding data protocol
        /// </summary>
        /// <param name="data_vstup">vstupne data na dekodovanie - typ string</param>
        private string[] decode(string data_vstup)
        {
            int posun = 0;
            int size = 0;
            int poc_znakov = 0;
            int poc_char_pocitadlo = 0;
            try
            {
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
        int data_recv_buffer = 1000;
        /// <summary>
        /// open data_buffer
        /// </summary>
        /// <param name="IP">ip adresa serveru</param>
        /// <param name="Port">cislo portu serveru</param>
        public bool open_socket(String IP, String Port)
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
            catch (SocketException se)
            {
                return false;
            }

        }

        /// <summary>
        /// close data_buffer
        /// </summary>
        public bool close_socket()
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
        /// <summary>
        /// receive data from data_buffer, output ascii
        /// </summary>
        private string recv_ascii()
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
        /// <summary>
        /// receive data from data_buffer, output byte array
        /// </summary>
        private byte[] recv_byte()
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
        /// <summary>
        /// send data to data_buffer
        /// </summary>
        /// <param name="objData">data</param>
        private bool send_data(Object objData)
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
        //---------------------------------------------------------------
        //*************************Funkcie******************************/
        //---------------------------------------------------------------
        /// <summary>
        /// send command and data
        /// </summary>
        /// <param name="command">command slovo pre server</param>
        /// <param name="data">dalsie data pre server</param>
        public void sendcommanddataarray(string riadiace, string[] data)
        {
            string velkost;
            string[] pomocny = new string[data.Length + 1];
            pomocny[0] = riadiace;
            for (int x = 1; x != pomocny.Length; x++)
            {
                pomocny[x] = data[x - 1];
            }
            string buffer = code(pomocny);
            if (buffer.Length < 10)
                velkost = "0000" + buffer.Length.ToString();
            else if (buffer.Length < 100)
                velkost = "000" + buffer.Length.ToString();
            else if (buffer.Length < 1000)
                velkost = "00" + buffer.Length.ToString();
            else if (buffer.Length < 10000)
                velkost = "0" + buffer.Length.ToString();
            else
                velkost = buffer.Length.ToString();
            send_data(velkost);
            send_data(buffer);
        }
        /// <summary>
        /// send command and data
        /// </summary>
        public void sendcommanddata(string riadiace, string data)
        {
            string velkost;
            string[] pomocny = new string[2];
            pomocny[0] = riadiace;
            pomocny[1] = data;
            string buffer = code(pomocny);
            if (buffer.Length < 10)
                velkost = "0000" + buffer.Length.ToString();
            else if (buffer.Length < 100)
                velkost = "000" + buffer.Length.ToString();
            else if (buffer.Length < 1000)
                velkost = "00" + buffer.Length.ToString();
            else if (buffer.Length < 10000)
                velkost = "0" + buffer.Length.ToString();
            else
                velkost = buffer.Length.ToString();
            send_data(velkost);
            send_data(buffer);
        }
        /// <summary>
        /// send data array with code
        /// </summary>
        public void send_data_array(string[] data)
        {
            string velkost;
            string buffer = code(data);
            if (buffer.Length < 10)
                velkost = "0000" + buffer.Length.ToString();
            else if (buffer.Length < 100)
                velkost = "000" + buffer.Length.ToString();
            else if (buffer.Length < 1000)
                velkost = "00" + buffer.Length.ToString();
            else if (buffer.Length < 10000)
                velkost = "0" + buffer.Length.ToString();
            else
                velkost = buffer.Length.ToString();
            send_data(velkost);
            send_data(buffer);
        }
        //---------------------------------------------------------------
        /// <summary>
        /// send command and data, receive string array
        /// </summary>
        public string[] sendcommanddata_recvarray(string riadiace, string[] data)
        {
            string velkost;
            string[] pomocny = new string[data.Length + 1];
            pomocny[0] = riadiace;
            for (int x = 1; x != pomocny.Length; x++)
            {
                pomocny[x] = data[x - 1];
            }
            string buffer = code(pomocny);
            if (buffer.Length < 10)
                velkost = "0000" + buffer.Length.ToString();
            else if (buffer.Length < 100)
                velkost = "000" + buffer.Length.ToString();
            else if (buffer.Length < 1000)
                velkost = "00" + buffer.Length.ToString();
            else if (buffer.Length < 10000)
                velkost = "0" + buffer.Length.ToString();
            else
                velkost = buffer.Length.ToString();
            send_data(velkost);
            send_data(buffer);
            string buffer_prijem = recv_ascii();
            //----------------------------------
            return decode(buffer_prijem);
        }
        //---------------------------------------------------------------
        /// <summary>
        /// send command
        /// </summary>
        public void sendommand(string riadiace)
        {
            string velkost;
            string[] pomocny = new string[2];
            pomocny[0] = riadiace;
            pomocny[1] = "ahoj";
            string buffer = code(pomocny);
            if (buffer.Length < 10)
                velkost = "0000" + buffer.Length.ToString();
            else if (buffer.Length < 100)
                velkost = "000" + buffer.Length.ToString();
            else if (buffer.Length < 1000)
                velkost = "00" + buffer.Length.ToString();
            else if (buffer.Length < 10000)
                velkost = "0" + buffer.Length.ToString();
            else
                velkost = buffer.Length.ToString();
            send_data(velkost);
            send_data(buffer);
        }
        //---------------------------------------------------------------
        /// <summary>
        /// send command and receive string
        /// </summary>
        /// <param name="command">command slovo pre server</param>
        public string sendcommand_recvstring(string riadiace)
        {
            sendommand(riadiace);
            //----------------------------------
            string buffer_prijem = recv_ascii();
            //----------------------------------
            return decode(buffer_prijem)[0];
        }
        //---------------------------------------------------------------
        /// <summary>
        /// send command and receive string array
        /// </summary>
        /// <param name="command">command slovo pre server</param>
        public string[] sendcommand_recvarray(string command)
        {
            sendommand(command);
            //----------------------------------
            string buffer_prijem = recv_ascii();
            //----------------------------------
            return decode(buffer_prijem);
        }
        //---------------------------------------------------------------
        int picturewidth = 320;
        int pictureheight = 160;
        private int obr_recv_buffer = 300000;
        Bitmap picture;
        /// <summary>
        /// receive picture_height
        /// </summary>
        public Bitmap recv_picture_original(string name_obr)
        {
            byte[] data_prijem = new byte[obr_recv_buffer];
            sendommand(name_obr);
            try
            {
                socket_client.Receive(data_prijem);
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(data_prijem);
                picture = new Bitmap(img, picturewidth, pictureheight);
            }
            catch { }
            return picture;
        }
        /// <summary>
        /// how data char you can send by one data 2 on byte_for_char*8=how char
        /// </summary>
        public int byte_for_char_protocol
        {
            get
            {
                return byte_for_char;
            }
            set
            {
                byte_for_char = value;
            }
        }

        /// <summary>
        /// how data you can send 2 on byte_for_byte*8=how data
        /// </summary>
        public int byte_for_dat_protocol
        {
            get
            {
                return byte_for_dat;
            }
            set
            {
                byte_for_dat = value;
            }
        }

        /// <summary>
        /// how max size have receive data
        /// </summary>
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

        /// <summary>
        /// width picture
        /// </summary>
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
        /// <summary>
        /// height picture
        /// </summary>
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
        /// <summary>
        /// how max size have picture
        /// </summary>
        public int obr_buffer
        {
            get
            {
                return obr_recv_buffer;
            }
            set
            {
                obr_recv_buffer = value;
            }
        }
        //---------------------------------------------------------------
        //**************************************************************/
    }
}
