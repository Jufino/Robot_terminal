using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace jpgtogif
{
    class save_to_gif
    {
        //1 dielik = 19ms
        //pre kameru 3 = 60 ms
        //private byte[] Delay = { 3, 0 };
        byte cas=3;
        int buffer=3001;
        public Image[] images;
        //--------------------------------
        public void bitmaps_to_gif(string GifPath, Image[] images1)
        {
            byte[] GifAnimation = { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 };
            byte[] Delay = { cas, 0 };
            MemoryStream MS = new MemoryStream();
            BinaryWriter BW = new BinaryWriter(new FileStream(GifPath, FileMode.Create));
            images1[0].Save(MS, ImageFormat.Gif);
            byte[] B = MS.ToArray();
            B[10] = (byte)(B[10] & 0X78);
            BW.Write(B, 0, 13);
            BW.Write(GifAnimation);
            B[785] = Delay[0];
            B[786] = Delay[1];
            B[798] = (byte)(B[798] | 0X87);
            BW.Write(B, 781, 18);
            BW.Write(B, 13, 768);
            BW.Write(B, 799, B.Length - 800);
            for (int I = 1; I < images1.Length; I++)
            {
                MS.SetLength(0);
                try
                {
                    images1[I].Save(MS, ImageFormat.Gif);
                    B = MS.ToArray();
                    //-----------------------------------
                    B[785] = Delay[0];
                    B[786] = Delay[1];
                    B[798] = (byte)(B[798] | 0X87);
                    BW.Write(B, 781, 18);
                    BW.Write(B, 13, 768);
                    BW.Write(B, 799, B.Length - 800);
                    //-----------------------------------
                }
                catch {
                    I = images1.Length;
                }
            }
            BW.Write(B[B.Length - 1]);
            BW.Close();
            MS.Dispose();
        }
        public void create_buffer()
        {
            images = new Image[buffer];
        }
        public void add_image(Image addimage, int pozicia)
        {
            images[pozicia] = addimage;
        }
        public Image read_image(int pozicia)
        {
            return images[pozicia];
        }
    }
}
