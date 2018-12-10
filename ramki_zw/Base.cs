using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ramki_zw
{
    class Base
    {
        // zmienna path nie jest potrzebna w zadnej z metod
        // można ją spokojnie przenieść do konstruktora jako zmienną prywatną
        // ale może się przyda więc zostaje
        private string path;
        // do połączenia wystarczy db_con ustawiony raz w konstruktorze 
        private SQLiteConnection db_con;
        public string nazwatabeli = "ramki_dane";
        private string nazwatabeliwersja = "wersja_ramkidane";


        /// <summary>
        /// Konstruktor klasy. Pobiera ścieżkę do bazy.
        /// </summary>
        /// <param name="_path"></param>

        public Base(string _path)
        {
            this.path = _path + "\\Bazy";
            string sciezkaBaza = this.path + "\\pi.sqlite";
            this.db_con = new SQLiteConnection($"Data Source = {sciezkaBaza}; Version = 3");
        }

        //public interface IBase
       // {
       //     bool AddBaseIfNotExist();
       //     int GetBaseVersion();
        //}
        /// <summary>
        /// Tworzy bazę jeśli nie istnieje i tabelę projekt_lista
        /// </summary>
        public void DodajTabeleJesliNieIstnieje()
        {

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = System.Data.CommandType.Text;
            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + nazwatabeli + " ([Format]  Varchar(50), [Wysokość]  INT, [Długość] INT, [G-marg] INT, [D-marg] INT, [L-marg] INT, [P-marg] INT)";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + nazwatabeliwersja + " ([Numer wersji]  INT)";
            db_cmd.ExecuteNonQuery();
            this.db_con.Close();
        }

        //kasuj grid prety
        public void UsunDaneZTabeli()
        {
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "delete from  " + nazwatabeli + " where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            db_con.Close();
        }

        public void DodajDaneDoTabeli()
        {
            try
            {
                this.db_con.Open();
                SQLiteCommand db_cmd = db_con.CreateCommand();
                db_cmd.CommandType = CommandType.Text;
                db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('" + UserControl2.nazwaformatki + "','"
                                                                      + UserControl2.wysokosc + "','"
                                                                      + UserControl2.dlugosc + "','"
                                                                      + UserControl2.G_marg + "','"
                                                                      + UserControl2.D_marg + "','"
                                                                      + UserControl2.L_marg + "','"
                                                                      + UserControl2.P_marg + "')";
                db_cmd.ExecuteNonQuery();
                db_con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PI-INFO");
                //Window.GetWindow(this).Close();
            }
        }

        public void ZmienDaneWTabeli()
        {            
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            string kolumna = "Format";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.nazwaformatki + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "Wysokość";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.wysokosc + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "Długość";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.dlugosc + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "G-marg";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.G_marg + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "D-marg";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.D_marg + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "L-marg";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.L_marg + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            kolumna = "P-marg";
            db_cmd.CommandText = "update " + nazwatabeli + " set [" + kolumna + "] = '" + UserControl2.P_marg + "' where [Format] = '" + UserControl1.formatka + "'";
            db_cmd.ExecuteNonQuery();
            db_con.Close();
        }

        public void SetData()
        {
            db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A4','210','297','25','5','5','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A4_pion','297','210','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A3','297','420','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A3_pion','420','297','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A2','420','594','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A2_pion','594','420','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A1','594','841','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A1_pion','841','594','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A0','841','1189','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "INSERT INTO " + nazwatabeli + " VALUES ('A0_pion','1189','841','5','5','25','5')";
            db_cmd.ExecuteNonQuery();
            db_con.Close();
        }

        public int GetTableVersion()

        {
            int wersja = 1;

            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "select * from " + nazwatabeliwersja;
            db_cmd.ExecuteNonQuery();
            var dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(db_cmd);
            da.Fill(dt);
            int wierszy = dt.Rows.Count;
            if (wierszy == 0)
            {
                db_cmd.CommandText = "INSERT INTO " + nazwatabeliwersja + " VALUES ('" + wersja + "')";
                db_cmd.ExecuteNonQuery();
                this.db_con.Close();
            }
            else
            {
                this.db_con.Close();
                DataRow dtr = dt.Rows[0];
                wersja = Convert.ToInt16(dtr[0]);
            }

            return wersja; // metoda zwróci wersję
        }

        public DataTable PobierzDaneTabeli(string nazwa_tabeli)
        {
            var dt = new DataTable();
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "select * from " + nazwa_tabeli + "";
            db_cmd.ExecuteNonQuery();
            SQLiteDataAdapter da = new SQLiteDataAdapter(db_cmd);
            da.Fill(dt);
            this.db_con.Close();
            return dt;
        }

    }
}
