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
        public static int wysokosc, dlugosc, G_marg, D_marg, L_marg, P_marg;
        public static string nazwaformatki;
        

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        

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

            if (UserControl1.czydodaj == false)
            {
                textbox_nazwaformatki.Text = UserControl1.formatka;
                textbox_wysokosc.Text = UserControl1.wysokosc.ToString();
                textbox_dlugosc.Text = UserControl1.dlugosc.ToString();
                textbox_Dmarg.Text = UserControl1.D_marg.ToString();
                textbox_Gmarg.Text = UserControl1.G_marg.ToString();
                textbox_Pmarg.Text = UserControl1.P_marg.ToString();
                textbox_Lmarg.Text = UserControl1.L_marg.ToString();
            }
             
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            bool results = CzyWszystkoOk();
            if (results == true)
            {
                if (UserControl1.czydodaj == true)
                {
                    //MessageBox.Show("Możesz wstawić metodę dodającą rekord do bazy");
                    var baza = new Base(path);
                    baza.DodajDaneDoTabeli();
                    Window.GetWindow(this).Close();
                }

                else
                {
                   // MessageBox.Show("Możesz wstawić metodę zmieniającą rekord w bazie");
                    var baza = new Base(path);
                    baza.UpdateDaneWTabeli();
                    Window.GetWindow(this).Close();
                }
            }

        }

        /// <summary>
        /// Sprawdza poprawność wprowadzonych danych
        /// </summary>
        /// <returns></returns>
        private bool CzyWszystkoOk()
        {
            bool Czyok = true;
            nazwaformatki = textbox_nazwaformatki.Text;
            bool results_wysokosc = int.TryParse(textbox_wysokosc.Text, out wysokosc);
            bool results_długosc = int.TryParse(textbox_dlugosc.Text, out dlugosc);
            bool results_Gmarg = int.TryParse(textbox_Gmarg.Text, out G_marg);
            bool results_Dmarg = int.TryParse(textbox_Dmarg.Text, out D_marg);
            bool results_Lmarg = int.TryParse(textbox_Lmarg.Text, out L_marg);
            bool results_Pmarg = int.TryParse(textbox_Pmarg.Text, out P_marg);

            if (nazwaformatki.Length < 1)
            {
                MessageBox.Show("Nie podałeś nazwy dla formatki","PI-INFO"); Czyok = false;
            }
            else if (results_wysokosc == false || (results_wysokosc == true & wysokosc <= 0))
            {
                    MessageBox.Show("Wysokość musi być liczbą naturalną większą od 0", "PI-INFO"); Czyok = false;
            }                
            else if (results_długosc == false || (results_długosc == true & dlugosc <= 0))
            {
                MessageBox.Show("Długość musi być liczbą naturalną większą od 0","PI-INFO"); Czyok = false;
            }
            else if (results_Gmarg == false || (results_Gmarg == true & (G_marg <= 0 || G_marg > 15)))
            {
                MessageBox.Show("Górny margines musi być liczbą naturalną większą od 0 i mniejszą od 16", "PI-INFO"); Czyok = false;
            }
            else if (results_Dmarg == false || (results_Dmarg == true & (D_marg <= 0 || D_marg > 15)))
            {
                MessageBox.Show("Dolny margines musi być liczbą naturalną większą od 0 i mniejszą od 16", "PI-INFO"); Czyok = false;
            }
            else if (results_Pmarg == false || (results_Pmarg == true & (P_marg <= 0 || P_marg > 15)))
            {
                MessageBox.Show("Prawy margines musi być liczbą naturalną większą od 0 i mniejszą od 16", "PI-INFO"); Czyok = false;
            }
            else if (results_Lmarg == false || (results_Lmarg == true & (L_marg < 15 || L_marg > 30)))
            {
                MessageBox.Show("Lewy margines musi być liczbą naturalną większą od 15 i mniejszą od 30", "PI-INFO"); Czyok = false;
            }
            return Czyok;
        }
    }
}
