using System;
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

namespace systemutviklertest
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        // Override metode som avslutter programmet når du trekker "x"'en
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        public Welcome()
        {
            InitializeComponent();
        }

        private void register_button(object sender, RoutedEventArgs e)
        {
            // Henter opp registrering vinduet
            MainWindow reg = new MainWindow();
            // Viser selve registrerings vinduet
            reg.Show();
            // Lagger en intans av klassen/vinduet welcome og gjemmer det
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            Hide();
        }

        private void searchcustomers(object sender, RoutedEventArgs e)
        {
            // Lager en instans av vinduet "sok" og viser det
            SearchCustomers sok = new SearchCustomers();
            sok.Show();
            // Gjemmer velkomstskjermen
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            Hide();
        }
    }
}
