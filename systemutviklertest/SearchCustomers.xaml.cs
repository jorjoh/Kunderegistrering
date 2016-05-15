using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public SearchCustomers()
        {
            InitializeComponent();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("D:\\Documents\\visual studio 2015\\Projects\\systemutviklertest\\Resources\\wpfbackground.jpg", UriKind.Absolute));
            this.Background = myBrush;
            // Definerer tekst til labels programatisk
            searchCustomersLabel.Content = "Søk i registrerte kunder fra databasen";
            showcustomer.Content = "Velg kunde og få ut informasjon";
            // Fyrer av fliicomboboxlist-metoden
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
            // Connectionstring til databasen
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            // Databasevariabel som inneholder connectionstring
            dbconn = new MySqlConnection(connectionString);
            // åpner databasetilkoblingen
            dbconn.Open();
            // Sql-string som søker baser på søketekst i tekstboksen
            string sql = "SELECT* FROM kunde WHERE firmanavn LIKE  '%" + sok.Text + "%' OR adresse LIKE '%" + sok.Text + "%' OR postadresse LIKE '%" + sok.Text + "%';";
            // MySqlCommand som tar sql og dbconn som argument
            cmd = new MySqlCommand(sql, dbconn);
            // Datadapter som kjører sql
            da = new MySqlDataAdapter(cmd);
            // Commandbuilder som bygger enten SELECT, INSERT, UPDATE, DELETE setninger
            cb = new MySqlCommandBuilder(da);
            // Velger tabellen kunde
            DataTable dt = new DataTable("kunde");
            // Fyller opp dataadapteren med datatable
            da.Fill(dt);
            // Setter datagrid sitt innhold = det den får ut av tabellen
            allCutomersDataGrid.ItemsSource = dt.DefaultView;
            // Oppdaterer dt med dataAdapter
            da.Update(dt);
        }
        // Metode for å fylle opp comboboxen
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

        private void showSelectedCustomer(object sender, RoutedEventArgs e)
        {
            //Tømmer searchCustomerslabel sitt innhold
            searchCustomersLabel.Content = " ";
            // Setter valgt kunde sin tekststreng likt comboboksen sin tekst
            selectedCustomerString = comboBox.Text;
            // Connectionstring mot databasen
            connectionString = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            // Variabel som inneholder connectionStringen
            dbconn = new MySqlConnection(connectionString);
            //Åpner databasentilkoblingen
            dbconn.Open();
            //SQL string som kjøres i databasen
            string sql = "SELECT * FROM kunde WHERE firmanavn ='" + selectedCustomerString + "'; ";
            //
            cmd = new MySqlCommand(sql, dbconn);
            // Dataadapteret utfører kommandoer
            da = new MySqlDataAdapter(cmd);
            // CommandBuilder, med utgangspunkt i en SELECT-setning genererer tilhørende kommandoer for innsetting, oppdatering, og sletting.
            cb = new MySqlCommandBuilder(da);
            // Definerer ny datatabell og velger feltet kunde fra databasen
            DataTable dt = new DataTable("kunde");
            // Forteller dataadapteren til å fylle datasettet med dt
            da.Fill(dt);
            // Seter Datagridet sin datasource til tabellen fra databasem
            selectedCustomerDataGrid.ItemsSource = dt.DefaultView;
            // Kjører en update som spinner ut informasjonen
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
            // Avlsutter søkevindet og gjemmer det
            SearchCustomers searchCustomers = new SearchCustomers();
            searchCustomers.Owner = this;
            Hide();
            //Henter opp igjen velkomstvinduet
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            welcome.Show();
        }
    }
}
