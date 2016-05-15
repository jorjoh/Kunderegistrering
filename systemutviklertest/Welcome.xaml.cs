using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri("D:\\Documents\\visual studio 2015\\Projects\\systemutviklertest\\wpfbackground.jpg", UriKind.Absolute));
            this.Background = myBrush;
            
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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Deletecustomer deletecustomer = new Deletecustomer();
            deletecustomer.Show();
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            Hide();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Changedata changedata = new Changedata();
            changedata.Show();
            Welcome welcome = new Welcome();
            welcome.Owner = this;
            Hide();
        }
    }
}
