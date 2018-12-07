using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ramki_zw
{
    

    /// <summary>
    /// Logika interakcji dla klasy UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        string path;

        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location));
            //ikona okna
            string sciezka = path + "\\pi_icon32x32.ico";
            Uri iconUri = new Uri(sciezka, UriKind.RelativeOrAbsolute);
            if (File.Exists(sciezka) == true) Window.GetWindow(this).Icon = BitmapFrame.Create(iconUri);
        }
    }
}
