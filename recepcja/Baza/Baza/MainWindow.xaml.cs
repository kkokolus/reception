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
        string pacjentWizytaUstalona = "";

        string defaultPassword = "adm";
        IQueryable qry;
        IQueryable q3;
        
        public MainWindow()
        {
            InitializeComponent();

      // Stworzenie przykładowych lekarzy, przychodni itp. na potrzeby stworzenia bazy w SQL Server i testów
            var przykladowy_lekarz = new Lekarz { Imie = "Jan", Nazwisko = "Kowalski", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz2 = new Lekarz { Imie = "Adam", Nazwisko = "Nowak", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz3 = new Lekarz { Imie = "Anna", Nazwisko = "Nowacka", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowa_przychodnia = new Przychodnia { Rodzaj = "Okulistyczna" };
            var przykladowa_przychodnia2 = new Przychodnia { Rodzaj = "Rehabilitacyjna" };
            var pacjent = new Pacjent { Imie = "Tomasz", Nazwisko = "Szklarski", PESEL = 12345678901234 };
            var pacjent2 = new Pacjent { Imie = "Antoni", Nazwisko = "Janowski", PESEL = 12345678901234 };
            var przykladowy_dyzur = new Dyzur { IDPrzychodnia = 1, IDLekarz = 2 };
            var przykladowy_dyzur2 = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = "Poniedziałek" };
            var przykladowy_dyzur3 = new Dyzur { IDPrzychodnia = 2, IDLekarz = 3, DzienTygodnia = "Poniedziałek" };

            using (db)
            {
                try
                {
                    db.Lekarze.Add(przykladowy_lekarz); 
                    db.Lekarze.Add(przykladowy_lekarz2);
                    db.Lekarze.Add(przykladowy_lekarz3);
                    db.Przychodnie.Add(przykladowa_przychodnia);
                    db.Przychodnie.Add(przykladowa_przychodnia2);
                    db.Pacjenci.Add(pacjent);
                    db.Pacjenci.Add(pacjent2);
                    db.SaveChanges();
                    db.Dyzury.Add(przykladowy_dyzur);
                    db.Dyzury.Add(przykladowy_dyzur2);
                    db.Dyzury.Add(przykladowy_dyzur3);
                    db.SaveChanges();
        // Dodawanie lekarzy na listę lekarzy
                qry = from b in db.Lekarze orderby b.Nazwisko select b.Nazwisko;
      
                    foreach (var item in qry)
                    {
                        ComboBoxItem R = new ComboBoxItem() { Content = item };
                        lekarzeComboBox.Items.Add(R);
                    }   
         // Dodawanie przychodni na listę przychodni
                var q2 = from rodzaje in db.Przychodnie orderby rodzaje.Rodzaj select rodzaje.Rodzaj;

                    foreach (var item in q2)
                    {
                        ComboBoxItem R = new ComboBoxItem() { Content = item };
                        przychodnieComboBox.Items.Add(R);
                    }
                    // Dodawanie pacjentow na listę pacjentow
                    var q3 = db.Pacjenci.Select(x => x.Nazwisko + " " + x.Imie).ToList();

                    var y = string.Join(" ", q3);
                    var f = y.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in f)

                    { 
                        ComboBoxItem P = new ComboBoxItem() { Content = item };
                        pacjentNameComboBox.Items.Add(P);
                    }
                    //textBoxPacjentID.Text = "test";
                    int sg = 5;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }
//--------------------------------------------------------------------------------------------------------------
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

        private void Click_pacjentUsun(object sender, RoutedEventArgs e)
        {
            // uzytkownicyData.ItemsSource = Uzytkownik.GetUzytkownik();
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;
        }
        private void Click_lekarzUsun(object sender, RoutedEventArgs e)
        {
            // lekarzeData.ItemsSource = 
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;

        }

        private void Click_dyzurUsun(object sender, RoutedEventArgs e)
        {

            uzytkownicyData.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;

        }

        private void Click_przychodniaUsun(object sender, RoutedEventArgs e)
        {
             
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;

        }
        private void Click_adminLogout(object sender, RoutedEventArgs e)
        {
            adminLogowanie.Visibility = Visibility.Visible;
            adminPanel.Visibility = Visibility.Hidden;
            passwordTip.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
        }

        private void Click_nowyLekarz(object sender, RoutedEventArgs e)
        {
            gridLekarz.Visibility = Visibility.Visible;
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;

        }

        private void Click_nowyPacjent(object sender, RoutedEventArgs e)
        {
            gridUzytkownik.Visibility = Visibility.Visible;
            gridLekarz.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;
        }

        private void Click_nowyDyzur(object sender, RoutedEventArgs e)
        {
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Visible;
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Hidden;
        }

        private void Click_nowaPrzychodnia(object sender, RoutedEventArgs e)
        {
            gridUzytkownik.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridDyzur.Visibility = Visibility.Hidden;
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridPrzychodnia.Visibility = Visibility.Visible;
        }


        private void przychodnieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project_context db = new Project_context();

            using (db)
            {
        // W zmiennej nazwa_przychodni znajduje się nazwa przychodni wybranej w comboboxie przychodni
                var tmp = przychodnieComboBox.SelectedItem.ToString();
                string[] words = tmp.Split(' ');
                var nazwa_przychodni = words[1];

        // Zapytanie zwraca nazwiska lekarzy którzy kiedykolwiek pełnią dyżur w wybranej przychodni
                var query_lista_lekarzy =
                from l in db.Lekarze
                join dyz in db.Dyzury on l.IDLekarz equals dyz.IDLekarz
                join przych in db.Przychodnie on dyz.IDPrzychodnia equals przych.IDPrzychodnia
                where przych.Rodzaj == nazwa_przychodni
                select l.Nazwisko;

       
                    lekarzeComboBox.Items.Clear();

                var t = string.Join(" ", query_lista_lekarzy);
                var f = t.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in f)
                {
                    ComboBoxItem combo = new ComboBoxItem() { Content = item };
                    lekarzeComboBox.Items.Add(combo);
                }
            }
        }

        private void pacjentNameCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project_context db = new Project_context();

            using (db)
            {
                // U mnie to nadal nie działa, a jeśli w imieniu dajesz mu words[2] wyrzuca przekroczenie zakresu tablicy
                var tmp = pacjentNameComboBox.SelectedItem.ToString();
                string[] words = tmp.Split(' ');
                string pacjentWybranyNazwisko = words[1];
                string pacjentWybranyImie = words[0];
                var query = from p in db.Pacjenci where p.Nazwisko == pacjentWybranyNazwisko select p.IDPacjent;

                if (query != null)
                    {
                     foreach (var q in query)
                     textBoxPacjentID.Text = ((int)q).ToString();
                    }
                else
                textBoxPacjentID.Text = "";

            }
        }

       private void lekarzeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Błąd polegał na nakazaniu mu pracy na nullu, teraz instrukcja wykonuje się dopiero po sprawdzeniu że na comboboxie wybraliśmy jakiegoś lekarza
            if (lekarzeComboBox.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                string tmp = comboBox.SelectedValue.ToString();
                string[] words = tmp.Split(' ');
                string nazwisko_lekarza = words[1];
                tuBedzieNazwisko.Content = nazwisko_lekarza;
            }
        }

        private void calendarSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Project_context db = new Project_context();
            string selectedDate = calendar.SelectedDate.ToString();     
                  
            string[] data = selectedDate.Split(' ');
            data[0].Replace('/', '-');
                  //data jest teraz w formacie dd-mm-yyyy jak w SQL
            using (db)
            {
                  // trzeba pobrac z bazy dane dot. dyzurow
            }
            textBoxPacjentID.Text = "";

        }

        private void pacjentIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

        }

        //inny kolor dla panelu admina
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           if(adminTabHeader.IsSelected)
           {
                SolidColorBrush brush = new SolidColorBrush(Colors.DarkOliveGreen);
               tabControl.Background = brush;
           }
           if (userTabHeader.IsSelected)
           {
              SolidColorBrush brush = new SolidColorBrush(Colors.BurlyWood);
              tabControl.Background = brush;
           }   
                
        }
        private void zapiszPacjenta_Click(object sender, RoutedEventArgs e)
        {
             Project_context db = new Project_context();
             using (db)
             {
                 var przykladowy_pacjent7 = new Pacjent { Imie = nowyPacjentImie.Text, Nazwisko = nowyPacjentNazwisko.Text, Adres = nowyPacjentAdres.Text, PESEL = Int64.Parse(nowyPacjentPESEL.Text), Telefon = Int64.Parse(nowyPacjentTelefon.Text) };
                 db.Pacjenci.Add(przykladowy_pacjent7);
                 db.SaveChanges();
             }
        }
    }
}

