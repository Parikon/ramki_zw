using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ramki_zw
{
    public partial class Okno_wyboru : Window
    {
        public Okno_wyboru()
        {
            UserControl2 mojakontrolka = new UserControl2();            
            this.Title = "PI 2019 MIT EDITION - Dodaj";
            int w = 600;
            int h = 220;
            this.Width = w;
            this.Height = h;
            this.MinWidth = w;
            this.MinHeight = h;
            this.MaxWidth = w;
            this.MaxHeight = h;
            this.AddChild(mojakontrolka);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.ThreeDBorderWindow;

        }
    }
}
