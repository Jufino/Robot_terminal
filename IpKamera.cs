using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace robot
{
    class IpKamera
    {
        Socket kamera_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        public static byte[] ToByteArray128(string StringToConvert)
        {
            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[128];
            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }
            for (int i = CharArray.Length; i != 128; i++)
            {
                ByteArray[i] = Convert.ToByte('x');
            }
            return ByteArray;
        }
        public static byte[] ToByteArray(string StringToConvert)
        {
            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length];
            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }
            return ByteArray;
        }
        public void Odosli_StringArrayRiadiace(string riadiace, string[] data)
        {
            byte[] data_odosli = new byte[128];
            string datax;
            int pocet_dat = 1;
            string pocet_dat_length;
            string riadiace_length;
            if (data != null) pocet_dat = data.Length + 1;
            pocet_dat_length = pocet_dat.ToString();
            if (pocet_dat_length.Length == 1) pocet_dat_length = "0" + pocet_dat_length;
            riadiace_length = riadiace.Length.ToString();
            if (riadiace_length.Length == 1) riadiace_length = "0" + riadiace_length;
            datax = pocet_dat_length + riadiace_length + riadiace;
            if (data != null)
            {
                for (int i = 0; i != data.Length; i++)
                {
                    pocet_dat_length = data[i].Length.ToString();
                    if (pocet_dat_length.Length == 1) pocet_dat_length = "0" + pocet_dat_length;
                    datax = datax + pocet_dat_length;
                    datax = datax + data[i];
                }
            }
            data_odosli = ToByteArray128(datax);
            try
            {
                kamera_socket.Send(data_odosli);
            }
            catch { }
        }
        public void Odosli_StringRiadiace(string riadiace, string data)
        {
            byte[] data_odosli = new byte[128];
            string datax;
            string riadiace_length;
            string pocet_dat_length;
            riadiace_length = riadiace.Length.ToString();
            if (riadiace_length.Length == 1) riadiace_length = "0" + riadiace_length;
            datax = "02" + riadiace_length + riadiace;
            if (data != null)
            {
                pocet_dat_length = data.Length.ToString();
                if (pocet_dat_length.Length == 1) pocet_dat_length = "0" + pocet_dat_length;
                datax = datax + pocet_dat_length;
                datax = datax + data;
            }
            data_odosli = ToByteArray128(datax);
            try
            {
                kamera_socket.Send(data_odosli);
            }
            catch { }
        }
        public void Odosli_OnlyString(string data)
        {
            byte[] data_odosli = new byte[data.Length];
            data_odosli = ToByteArray(data);
            kamera_socket.Send(data_odosli);
        }            

        public void pripoj(string adresa)
        {
            kamera_socket.Connect(adresa, 1212);
        }
        public void odpoj()
        {
            kamera_socket.Close();
        }
        Bitmap bitmap1 = new Bitmap(360, 280);
        public Bitmap prijem_obrazok(string name_obr)
        {
            byte[] data_prijem = new byte[300000];
            Odosli_StringRiadiace(name_obr, "0");
            try
            {
                kamera_socket.Receive(data_prijem);
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(data_prijem);
                bitmap1 = new Bitmap(img, 360, 280);
            }
            catch { }
            return bitmap1;
        }
        public string prijmi_data_allstring()
        {
            Odosli_StringRiadiace("data_odosli", null);
            return prijem_ascii();
        }
        public string[] prijmi_data_string(int pocet)
        {
            char[] a = new char[pocet];
            string[] data_prijem_x = new string[pocet];
            for (int i = 0; i != pocet; i++)
            {
                a[i] = '\n';
            }
            Odosli_StringRiadiace("data_odosli", null);
            data_prijem_x = prijem_ascii().Split(a);
            return data_prijem_x;
        }
        public int[] prijmi_data_int(int pocet)
        {
            char[] prechod = new char[pocet];
            int[] data_int = new int[pocet];
            string[] data_prijem_x = new string[pocet];
            for (int i = 0; i != pocet; i++)
            {
                prechod[i] = '\n';
            }
            Odosli_StringRiadiace("data_odosli", null);
            data_prijem_x = prijem_ascii().Split(prechod);
            for (int i = 0; i != pocet; i++)
            {
                data_int[i] = Convert.ToInt16(data_prijem_x[i]);
            }
            return data_int;
        }
        string prijem_ascii()
        {
            int receivedBytesLen = 0;
            byte[] buffer = new byte[150];
            if (kamera_socket.Connected == true)
            {
                try
                {
                    receivedBytesLen = kamera_socket.Receive(buffer);
                    return System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytesLen);
                }
                catch
                {
                    return "-1";
                }
            }
            return System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytesLen);
        }
        public int[] hod(int x_val, int y_val)
        {
            int[] data = new int[3];
            string[] datax = new string[2];
            x_val = x_val / 2;
            y_val = y_val / 2;
            datax[0] = x_val.ToString();
            datax[1] = y_val.ToString();
            Odosli_StringArrayRiadiace("hodnota", datax);
            Odosli_OnlyString("1");
            data[0] = Convert.ToInt16(prijem_ascii());
            Odosli_OnlyString("2");
            data[1] = Convert.ToInt16(prijem_ascii());
            Odosli_OnlyString("3");
            data[2] = Convert.ToInt16(prijem_ascii());
            return data;
        }
    }
}
