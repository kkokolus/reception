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
         Project_context db = new Project_context();
         string defaultPassword = "adm";

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
            using (db = new Project_context())
            {
                try
                {
                    db.Lekarze.Add(L); //w tym miejscu u mnie zatrzymuje sie :(( error  26
                    db.SaveChanges();

                var query = from b in db.Lekarze
                            orderby b.Nazwisko
                            select b.Nazwisko;
                }
               catch (Exception ex)
                {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
 //wypelniamy na starcie danymi 2 combobox'a - specjalisci i lekarze - tylko trzeba dodac tabelę w stylu TypLekarza
               //var query = from b in db.Lekarze orderby b.Nazwisko select b.Nazwisko;                          
               // foreach (var item in query)
               // {
               //     ComboBoxItem Le = new ComboBoxItem() { Content = (string)item };
               //     lekarzeComboBox.Items.Add(Le);
               // }
                //query = from b in db.Specjalisci orderby b.Nazwa select b.Nazwa;
                //foreach (var item in query)
                //{
                //    ComboBoxItem Le = new ComboBoxItem() { Content = (string)item };
                //    specjalizacjaComboBox.Items.Add(Le);
                //}

            }
            ComboBoxItem combo = new ComboBoxItem() { Content = "Kowalski"};
                    lekarzeComboBox.Items.Add(combo);

        }
        //ukrywa całą siatke tygodniową, zamiast niej wyswietla textboxy dla nowego pacjenta
        private void nowyPacjentClick(object sender, RoutedEventArgs e)
        {
            if (SiatkaTygodniowa.Visibility == Visibility.Visible)
            {
                SiatkaTygodniowa.Visibility = Visibility.Hidden;
                dyzuryLabel.Visibility = Visibility.Hidden;
               nowyPacjent.Content = "Cofnij";
                personalButtons.Visibility = Visibility.Visible;
                gridPacjent.Visibility = Visibility.Visible;
                labelPacjent.Visibility = Visibility.Visible;
            }
            else if (SiatkaTygodniowa.Visibility == Visibility.Hidden)
            {
                SiatkaTygodniowa.Visibility = Visibility.Visible;
                dyzuryLabel.Visibility = Visibility.Visible;
                nowyPacjent.Content = "Dodaj nowego pacjenta";
                personalButtons.Visibility = Visibility.Hidden;
                gridPacjent.Visibility = Visibility.Hidden;
                labelPacjent.Visibility = Visibility.Hidden;
            }
        }
        private void logowanieButton(object sender, RoutedEventArgs e)
        {
            //tymczasowo nie ma hasła
            adminLogowanie.Visibility = Visibility.Hidden;
            adminPanel.Visibility = Visibility.Visible;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
           int selectedIndex = comboBox.SelectedIndex;
           string[] selectedValue = comboBox.SelectedValue.ToString().Split();
           string comboItemText = "";
           for (int i = 1; i < selectedValue.Length; i++)
                comboItemText += selectedValue[i] + " ";
            //teraz mozna cos z tym comboItemText zrobic: LINQ -> lista lekarzy o wybranej specjalizacji -> foreach add item
            if (comboBox.Name == "specjalizacjaComboBox")
            {
            var query = from b in db.Lekarze
                            //where b.IDLekarz == ( from i  in db.Srecjalizacja where i.Nazwa == comboItemText select i.ID <- podobne podzapytanie albo JOIN
                            orderby b.Nazwisko
                            select b.Nazwisko;

            lekarzeComboBox.Items.Clear();
            foreach (var item in query)
                {
                    ComboBoxItem combo = new ComboBoxItem() { Content = (string)item };
                    lekarzeComboBox.Items.Add(combo);
                }
            }
            if (comboBox.Name == "lekarzeComboBox")
            {
                tuBedzieNazwisko.Content = comboItemText;
            }
        }
    }
}
