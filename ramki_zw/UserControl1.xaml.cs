using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    /// Logika interakcji dla klasy UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        string path;

        public UserControl1()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            dataGrid.IsReadOnly = true;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                
                path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location));
                //ikona okna
                string sciezka = path + "\\pi_icon32x32.ico";
                Uri iconUri = new Uri(sciezka, UriKind.RelativeOrAbsolute);
                if (File.Exists(sciezka) == true) Window.GetWindow(this).Icon = BitmapFrame.Create(iconUri);

                Bitmap bm = Resource1.pi_icon_png;
                image_rysuj_ramke.Source = Tools.Konwersja_bitmap_bitmapimage_png(bm);

                var baza = new Base(path);
                baza.AddBaseIfNotExist();
                if (baza.GetTableVersion() != 1)
                {
                    // tutaj dałbym konwerter na starej tabeli na nową, ale narazie nie mamy takiego konwertera więc program tego nie zrobi
                    MessageBox.Show("Nieprawidłowa wersja tabeli. Konwersja niemożliwa w tej wersji programu.Nastąpi zamknięcie programu", "PI-INFO");
                    Window.GetWindow(this).Close();
                }

                DataTable dt = baza.PobierzDaneTabeli(baza.nazwatabeli);
                int wierszy = dt.Rows.Count;
                if (wierszy == 0)
                {
                    baza.GetData();
                    baza.PobierzDaneTabeli(baza.nazwatabeli);
                }
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Program zostanie zamknięty.", "PI-INFO");
                Window.GetWindow(this).Close();
            }
        }
    }
}
