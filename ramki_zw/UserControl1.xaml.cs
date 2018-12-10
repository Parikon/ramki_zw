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
        public static string path;
        public static int wybor = -1;
        public static string formatka;        
        public static int wysokosc;
        public static int dlugosc;
        public static int D_marg;
        public static int G_marg;
        public static int L_marg;
        public static int P_marg;
        public static bool czydodaj;


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
                baza.DodajTabeleJesliNieIstnieje();
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
                    baza.SetData();
                    dt = baza.PobierzDaneTabeli(baza.nazwatabeli);
                }
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Program zostanie zamknięty.", "PI-INFO");
                Window.GetWindow(this).Close();
            }
        }

        private void Button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            czydodaj = true;
            Okno_wyboru win2 = new Okno_wyboru();
            win2.ShowDialog();
            Odswiez();

        }

        private void Odswiez()
        {
            var baza = new Base(path);
            DataTable dt = baza.PobierzDaneTabeli(baza.nazwatabeli);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void Button_zmiana_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wybor = dataGrid.SelectedIndex;
                czydodaj = false;                
                if (wybor != -1)
                {
                    Okno_wyboru win2 = new Okno_wyboru();
                    win2.Title = "PI 2019 MIT EDITION - Zmień";
                    win2.ShowDialog();
                    Odswiez();
                }
                else
                {
                    MessageBox.Show("Nie wybraleś rekordu do zmiany", "PI-INFO");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PI-INFO");
                //Window.GetWindow(this).Close();
            }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                wybor = dataGrid.SelectedIndex;
                if (wybor != -1)
                {
                    DataRowView dR = (DataRowView)dataGrid.SelectedItem;
                    formatka = dR[0].ToString();
                    wysokosc = Convert.ToInt32(dR[1]);
                    dlugosc = Convert.ToInt32(dR[2]);
                    G_marg = Convert.ToInt32(dR[3]);
                    D_marg = Convert.ToInt32(dR[4]);
                    L_marg = Convert.ToInt32(dR[5]);
                    P_marg = Convert.ToInt32(dR[6]);

                   // MessageBox.Show(formatka.ToString() + " " + wysokosc.ToString() + " " + dlugosc.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PI-INFO");
                //Window.GetWindow(this).Close();
            }
        }

        private void Button_usun_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                var result = MessageBox.Show("Czy na pewno chcesz usunąć formatkę o nawie "+ UserControl1.formatka+"?", "PI-INFO", MessageBoxButton.OKCancel);
                //MessageBox.Show(result.ToString());
                if (result.ToString() == "OK")
                {

                    var baza = new Base(path);
                    baza.UsunDaneZTabeli();
                    DataTable dt = baza.PobierzDaneTabeli(baza.nazwatabeli);
                    dataGrid.ItemsSource = dt.DefaultView;
                }
                
                //MessageBox.Show("Możesz wstawić metodę usuwającą rekord z bazy");
               
            }
            else
            {
                MessageBox.Show("Najpierw wskaż co mam usunąć", "PI-INFO");
            }
        }
    }
}
