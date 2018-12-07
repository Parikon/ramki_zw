using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ramki_zw
{
    public partial class Okno_ramki : Window
    {
        
            public Okno_ramki(int szer, int wys)
            {
                this.Width = szer;
                this.Height = wys;
                this.MaxHeight = wys;
                this.MinHeight = wys;
                this.MaxWidth = szer;
                this.MinWidth = szer;
                UserControl1 kon = new UserControl1();
                this.AddChild(kon);
                this.Title = "PI 2019 MIT EDITION";
                this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                this.VerticalContentAlignment = VerticalAlignment.Stretch;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                //this.Topmost = true;
            }       

    }
}
