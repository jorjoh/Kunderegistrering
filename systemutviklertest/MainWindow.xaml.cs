using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;


namespace systemutviklertest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        string cs = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
        MySqlConnection dbconn;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds;
        String myconnectionstring;
        MySqlCommandBuilder cb;
        public MainWindow()
        {

            InitializeComponent();
            contactpersoninfoheader.Content = "Oversikt over registrerte kontaktpersoner";
            myconnectionstring = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            /*Connection information && variabels*/
            dbconn = new MySqlConnection(myconnectionstring);
            dbconn.Open(); ;
            string sql = "SELECT * FROM kunde;";
            //string sql = "INSERT INTO kunde (firmanavn, adresse, postadresse, telefon, fax) VALUES ('" + firmanavn.Text + "','" + adresse.Text + "','" + postadresse.Text + "','" + telefon.Text + "','" + telefon.Text+"');";
            MySqlCommand cmd = new MySqlCommand(sql, dbconn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            ds = new DataSet("kunde");
            da.Fill(ds, "kunde");
            /*End of informations && variabels*/
            filllistboxcustomertype();
            filllistboxcustomercontactperson();
        }

        private void filllistboxcustomertype()
        {
            listBox.Items.Clear();
            string kundetype;
            try
            {
                
                string sql = "SELECT * FROM kundetype;";
                MySqlCommand cmd = new MySqlCommand(sql, dbconn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                MySqlDataReader datareaderlistbox = null;
                ds = new DataSet("kundetype");
                da.Fill(ds, "kundetype");
                datareaderlistbox = cmd.ExecuteReader();
                while (datareaderlistbox.Read())
                {
                    kundetype = datareaderlistbox.GetString("type");
                    listBox.Items.Add(kundetype);
                   
                }
                
                datareaderlistbox.Close();
            }
            
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                MessageBox.Show("Something went wrong");
                throw;
            }
        }

        private void filllistboxcustomercontactperson()
        {
            contactpersons.Items.Clear();
           
            try
            {
                string sql = "SELECT * FROM kontaktpersoner;";
                MySqlCommand cmd = new MySqlCommand(sql, dbconn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                MySqlDataReader datareaderlistbox = null;
                ds = new DataSet("kontaktpersoner");
                da.Fill(ds, "kontaktpersoner");
                datareaderlistbox = cmd.ExecuteReader();
                while (datareaderlistbox.Read())
                {
                  string  kontaktpersonlastname = datareaderlistbox.GetString("etternavn");
                  string kontaktpersonfirstname = datareaderlistbox.GetString("fornavn");
                    contactpersons.Items.Add(kontaktpersonlastname + "," + kontaktpersonfirstname);

                }

                datareaderlistbox.Close();
            }

            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                MessageBox.Show("Something went wrong");
                throw;
            }
        }
      

        private void reg_customer(object sender, RoutedEventArgs e)
        {
            if (firmanavn.Text.Contains(" ") && adresse.Text == " " && postadresse.Text == " " && telefon.Text == "" && fax.Text == "")
            {
                MessageBox.Show("Du har ikke fylt ut alle feltene");
            }
            else
            {
                  if (listBox.SelectedItems.Count < 1)
            {
                MessageBox.Show("Nothing selected.");
                return;
            }
            // Konvertrer objekt til string for kundetyper
            StringBuilder sb = new StringBuilder();
            foreach (object objItem in listBox.SelectedItems)
            {
                if (sb.Length > 0) sb.Append("\n");
                sb.Append(objItem.ToString());
            }
            textBox1.Text = ("You selected:\n\n" + sb.ToString());

            StringBuilder sbcontactperson = new StringBuilder();
            foreach (object objItem2 in contactpersons.SelectedItems)
            {
                if (sbcontactperson.Length > 0) sbcontactperson.Append("\n");
                sbcontactperson.Append(objItem2.ToString());
            }
            textBox1.Text = ("You selected:\n\n" + sb.ToString());


            myconnectionstring = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(myconnectionstring);
            dbconn.Open(); ;
            string sql = "SELECT * FROM kunde;";
            MySqlCommand cmd = new MySqlCommand(sql, dbconn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            ds = new DataSet("kunde");
            da.Fill(ds, "kunde");
            try
            {
                DataRow nrad;
                nrad = ds.Tables["kunde"].NewRow();
                nrad["firmanavn"] = firmanavn.Text;
                nrad["adresse"] = adresse.Text;
                nrad["postadresse"] = postadresse.Text;
                nrad["telefon"] = telefon.Text;
                nrad["fax"] = fax.Text;
                nrad["kundetype"] = sb.ToString(); 
                nrad["kontaktperson"] = sbcontactperson.ToString(); 
                ds.Tables["kunde"].Rows.Add(nrad);
                da.Update(ds, "kunde");
                succsess.Content = "Rader insatt";
            }

            catch (Exception s)
            {
                Console.WriteLine(s.Message);
                MessageBox.Show("En feil oppstod: " + s.Message);
            }

            dbconn.Close();
            }
          
        }

        private void reg_contactperson(object sender, RoutedEventArgs e)
        {
            myconnectionstring = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(myconnectionstring);
            dbconn.Open(); ;

            string sql = "SELECT * FROM kontaktpersoner;";
            //string sql = "INSERT INTO kunde (firmanavn, adresse, postadresse, telefon, fax) VALUES ('" + firmanavn.Text + "','" + adresse.Text + "','" + postadresse.Text + "','" + telefon.Text + "','" + telefon.Text+"');";
            MySqlCommand cmd = new MySqlCommand(sql, dbconn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            ds = new DataSet("kontaktpersoner");
            da.Fill(ds, "kontaktpersoner");

            try
            {
                DataRow nrad;
                nrad = ds.Tables["kontaktpersoner"].NewRow();
                nrad["tittel"] = titel.Text;
                nrad["etternavn"] = etternavn.Text;
                nrad["fornavn"] = fornavn.Text;
                nrad["direktetelefon"] = direktetelefon.Text;
                nrad["mobiltelefon"] = mobiltelefon.Text;
                nrad["epost"] = epost.Text;
                ds.Tables["kontaktpersoner"].Rows.Add(nrad);
                da.Update(ds, "kontaktpersoner");
                succsess.Content = "Rader insatt";

            }
            catch (Exception s)
            {
                Console.WriteLine(s.Message);
                MessageBox.Show("Kontanktperson er allerede registrert");
            }
         
           // dbconn.Close();
        }
      
        private void reg_kundetype(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(checkBox1.Text);
            myconnectionstring = "Database=gatship; Data Source=localhost;User=root;Password=0DfTAZ;";
            dbconn = new MySqlConnection(myconnectionstring);
            dbconn.Open(); ;

            string sql = "SELECT * FROM kundetype;";
            //string sql = "INSERT INTO kunde (firmanavn, adresse, postadresse, telefon, fax) VALUES ('" + firmanavn.Text + "','" + adresse.Text + "','" + postadresse.Text + "','" + telefon.Text + "','" + telefon.Text+"');";
            MySqlCommand cmd = new MySqlCommand(sql, dbconn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            ds = new DataSet("kundetype");
            da.Fill(ds, "kundetype");

            try
            {
                DataRow nrad;
                nrad = ds.Tables["kundetype"].NewRow();
                nrad["type"] = customertype.Text;
                ds.Tables["kundetype"].Rows.Add(nrad);
                da.Update(ds, "kundetype");
                succsess.Content = "Rader insatt";
                
            }

            catch (Exception s)
            {
                Console.WriteLine(s.Message);
                MessageBox.Show("Kundetype er allerede registrert");
            }
        }

        private void abort(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Owner = this;
            Hide();
            Welcome welcome = new Welcome();
            welcome.Show();
           
        }

        private void filllistboxwithcustomers(object sender, RoutedEventArgs e)
        {
            filllistboxcustomertype();
           
        }

        private void updatecontactpersons_Click(object sender, RoutedEventArgs e)
        {
            filllistboxcustomercontactperson();
        }
    }

}
