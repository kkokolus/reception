using System.Data.Entity;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Baza.Klasy;
using System.Data;
using System.Data.SqlClient;


namespace Baza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            var L = new Lekarz { Imie = "Jan", Nazwisko = "Kowalski", PESEL = 93040234527, Telefon = 675384920 };

            //using (var dbx = new Project_context())
            //{
            //    //taki sam sposob na dodawanie 
            //    dbx.Entry(L).State = EntityState.Added;
            //    dbx.SaveChanges();
            //}
            using (var db = new Project_context())
            {
                try
                {
                    db.Lekarze.Add(L); //w tym miejscu u mnie zatrzymuje sie :((
                    db.SaveChanges();

                var query = from b in db.Lekarze
                            orderby b.Nazwisko
                            select b.Nazwisko;
                }
               catch (Exception ex)
                {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }


                //foreach (var item in query)
                //{
                //    ComboBoxItem combo = new ComboBoxItem() { Content = (string)item };
                //    lekarzeComboBox.Items.Add(combo);
                //}

            }
            ComboBoxItem combo = new ComboBoxItem() { Content = "Kowalski"};
                    lekarzeComboBox.Items.Add(combo);

        }
        //ukrywa całą siatke tygodniową
        private void nowyPacjentClick(object sender, RoutedEventArgs e)
        {
            if (SiatkaTygodniowa.Visibility == Visibility.Visible)
            {
                SiatkaTygodniowa.Visibility = Visibility.Hidden;
                nowyPacjent.Content = "Cofnij";
            }
            else if (SiatkaTygodniowa.Visibility == Visibility.Hidden)
            {
                SiatkaTygodniowa.Visibility = Visibility.Visible;
                nowyPacjent.Content = "Dodaj nowego pacjenta";
            }
            //tu mam dopisac by pojawili sie textbox'y dla wpisania danych nowego pacjenta
        }
        private void logowanieButton(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            //teraz mozna cos z tym itemom zrobic
        }
    }
}
