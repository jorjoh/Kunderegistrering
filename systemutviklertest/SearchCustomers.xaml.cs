using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Xps.Packaging;
using MySql.Data.MySqlClient;


namespace systemutviklertest
{
    /// <summary>
    /// Interaction logic for SearchCustomers.xaml
    /// </summary>
    public partial class SearchCustomers : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        string cs = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
        string selectedCustomerString;
        MySqlConnection dbconn;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds;
        String connectionString;
        MySqlCommandBuilder cb;
        public SearchCustomers()
        {
            InitializeComponent();
            searchCustomersLabel.Content = "Søk i registrerte kunder fra databasen";
            showcustomer.Content = "Velg kunde og få ut informasjon";
            fillListComboBox();
        }

        private void showAllCustomers(object sender, RoutedEventArgs e)
        {
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(connectionString);
            dbconn.Open(); ;
            string sql = "SELECT * FROM kunde;";
            cmd = new MySqlCommand(sql, dbconn);
            da = new MySqlDataAdapter(cmd);
            cb = new MySqlCommandBuilder(da);
            DataTable dt = new DataTable("kunde");
            da.Fill(dt);
            allCutomersDataGrid.ItemsSource = dt.DefaultView;
            da.Update(dt);
        }

        private void searchButton(object sender, RoutedEventArgs e)
        {
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(connectionString);
            dbconn.Open();
            string sql = "SELECT* FROM kunde WHERE firmanavn LIKE  '%" + sok.Text + "%' OR adresse LIKE '%" + sok.Text + "%' OR postadresse LIKE '%" + sok.Text + "%';";
            cmd = new MySqlCommand(sql, dbconn);
            da = new MySqlDataAdapter(cmd);
            cb = new MySqlCommandBuilder(da);
            DataTable dt = new DataTable("kunde");
            da.Fill(dt);
            allCutomersDataGrid.ItemsSource = dt.DefaultView;
            da.Update(dt);
        }

        private void fillListComboBox()
        {
          try
            {
                connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
                dbconn = new MySqlConnection(connectionString);
                dbconn.Open();

                string sql = "SELECT * FROM kunde;";
                MySqlCommand cmd = new MySqlCommand(sql, dbconn);
                MySqlDataReader dataReaderComboBox = cmd.ExecuteReader();
                while (dataReaderComboBox.Read())
                {
                   string kunde = dataReaderComboBox.GetString(1);
                    comboBox.Items.Add(kunde);

                }

                dbconn.Close();
            }

            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                throw;
            }
        }

        private void showSelectedCustomer(object sender, RoutedEventArgs e)
        {
            searchCustomersLabel.Content = " ";
            
            selectedCustomerString = comboBox.Text;
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(connectionString);
            dbconn.Open(); ;

            string sql = "SELECT * FROM kunde WHERE firmanavn ='" + selectedCustomerString + "'; ";
            MySqlCommand cmd = new MySqlCommand(sql, dbconn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            DataTable dt = new DataTable("kunde");
            da.Fill(dt);
            selectedCustomerDataGrid.ItemsSource = dt.DefaultView;
            da.Update(dt);
        }
       

        private void printPdf(object sender, RoutedEventArgs e)
        {
            // Setter opp printdialog
            PrintDialog printDlg = new PrintDialog();
            SearchCustomers uc = new SearchCustomers();
            //Skriver ut det visuelle i det aktuelle vinduet
            printDlg.PrintVisual(uc, "User Control Printing.");

            // Viser printdialogen
            printDlg.ShowDialog();

            // Kaller på PrintDocument metoden for å sende dokumentet til printer
            printDlg.PrintVisual(selectedCustomerDataGrid, "Kunde: " + selectedCustomerString);
        }

        private void showAllCustomerClick(object sender, RoutedEventArgs e)
        {
            // Setter opp printdialogen
            PrintDialog printDlg = new PrintDialog();
            SearchCustomers uc = new SearchCustomers();
            printDlg.PrintVisual(uc, "User Control Printing.");

            // Viser printdialogen
            printDlg.ShowDialog();

            // Kaller på PrintDocument metoden for å sende dokumentet til printer
            printDlg.PrintVisual(allCutomersDataGrid, "Liste over alle kunder");
        }

        private void backToStart(object sender, RoutedEventArgs e)
        {
            SearchCustomers searchCustomers = new SearchCustomers();
            searchCustomers.Owner = this;
            Hide();
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            welcome.Show();
        }
    }
}
