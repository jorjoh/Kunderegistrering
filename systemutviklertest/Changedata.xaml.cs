﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace systemutviklertest
{
    /// <summary>
    /// Interaction logic for Changedata.xaml
    /// </summary>
    public partial class Changedata : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        string selectedCustomerString;
        /*Globale database varriabler*/
        //string cs = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
        MySqlConnection dbconn;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        // DataSet ds;
        String connectionString;
        MySqlCommandBuilder cb;
        /*Slutt på gloable databasevariabler*/
        public Changedata()
        {
            InitializeComponent();
            /*Setter egendefinert bakgrunnsbilde til applikasjonen*/
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("D:\\Documents\\visual studio 2015\\Projects\\systemutviklertest\\Resources\\wpfbackground.jpg", UriKind.Absolute));
            this.Background = myBrush;
            /*Slutt på bakgrunnsbilde*/
            fillListComboBox();
        }
        private void fillListComboBox()
        {
            try
            {
                // Definerer connectionStringen
                connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
                // Database variabel som inneholder connectionstringen
                dbconn = new MySqlConnection(connectionString);
                // Åpner databasetilkoblingen
                dbconn.Open();
                // SQL-spørring
                string sql = "SELECT * FROM kunde;";
                // SqlCommand som tar string og tilkobling som argument
                MySqlCommand cmd = new MySqlCommand(sql, dbconn);
                // Datareader som brukes til å skrive ut alt i tabellen kunder"
                MySqlDataReader dataReaderComboBox = cmd.ExecuteReader();
                // While true = skriv ut alle kunder
                while (dataReaderComboBox.Read())
                {
                    // Lager string som får kolonnen med kunder fra databasen
                    string kunde = dataReaderComboBox.GetString(1);
                    // Legger til elementene i comboBoxen
                    comboBox.Items.Add(kunde);

                }
                // Stenger databasetilkoblingen
                dbconn.Close();
            }

            catch (Exception)
            {
                // Skriver en feilmelding til console hvis noe går sikkerlig "rett vest"
                Console.WriteLine("Something went wrong");
                throw;
            }
        }

        private void ComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            try
            {
                // Definerer connectionStringen
                connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
                // Database variabel som inneholder connectionstringen
                dbconn = new MySqlConnection(connectionString);
                // Åpner databasetilkoblingen
                dbconn.Open();
                // SQL-spørring
                string sql = "SELECT * FROM kunde WHERE firmanavn ='" + comboBox.Text + "';";
                // SqlCommand som tar string og tilkobling som argument
                MySqlCommand cmd = new MySqlCommand(sql, dbconn);
                // Datareader som brukes til å skrive ut alt i tabellen kunder"
                MySqlDataReader dataReaderComboBox = cmd.ExecuteReader();
                // While true = skriv ut alle kunder
                while (dataReaderComboBox.Read())
                {
                    // Lager string som får kolonnen med kunder fra databasen
                    string idstring = dataReaderComboBox.GetInt32(0).ToString();
                    string firmanavnsting = dataReaderComboBox.GetString(1);
                    string adressestring = dataReaderComboBox.GetString(2);
                    string poststedstring = dataReaderComboBox.GetString(3);
                    string telefonint = dataReaderComboBox.GetInt32(4).ToString();
                    string faxint = dataReaderComboBox.GetInt32(5).ToString();
                    string kontaktpersonstring = dataReaderComboBox.GetString(7);

                    id.Text = idstring;
                    firmanavn.Text = firmanavnsting;
                    adresse.Text = adressestring;
                    postadresse.Text = poststedstring;
                    telefon.Text = telefonint;
                    fax.Text = faxint;
                    contactperson.Text = kontaktpersonstring;

                }
                // Stenger databasetilkoblingen
                dbconn.Close();
            }

            catch (Exception)
            {
                // Skriver en feilmelding til console hvis noe går sikkerlig "rett vest"
                Console.WriteLine("Something went wrong");
                throw;
            }
        }

        private void updatecustomer_Click(object sender, RoutedEventArgs e)
        {
            // Setter valgt kunde sin tekststreng likt comboboksen sin tekst
            selectedCustomerString = comboBox.Text;
            // Connectionstring mot databasen
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            // Variabel som inneholder connectionStringen
            try
            {
               dbconn.Open();
                string sql = "UPDATE kunde SET firmanavn ='" + this.firmanavn.Text + "', adresse ='" + this.adresse.Text +
                             "', postadresse ='" + this.postadresse.Text + "', telefon ='" + this.telefon.Text +
                             "', fax ='" + this.fax.Text + "', kontaktperson ='" + contactperson.Text + "' WHERE idkunde ='" + id.Text+"'";
                cmd = new MySqlCommand(sql, dbconn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Query successful, rows updated");
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            Changedata changeData = new Changedata();
            changeData.Owner = this;
            Hide();
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            welcome.Show();
        }
    }
}
