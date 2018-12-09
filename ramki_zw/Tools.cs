using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ramki_zw
{
    public class Tools
    {
        public static BitmapImage Konwersja_bitmap_bitmapimage_png(Bitmap bm)
        {
            var memory = new MemoryStream();
            bm.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = memory;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            return bmp;
        }        
    }
}
