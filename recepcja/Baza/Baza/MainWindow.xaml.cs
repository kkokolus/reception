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
                var q3 = from p in db.Pacjenci orderby p.Nazwisko select p.Nazwisko;
                    foreach (var item in q3)
                    {
                        ComboBoxItem P = new ComboBoxItem() { Content = item };
                        pacjentNameComboBox.Items.Add(P);
                    }
                    textBoxPacjentID.Text = "test";
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

        private void Click_uzytkownikEdit(object sender, RoutedEventArgs e)
        {
            // uzytkownicyData.ItemsSource = Uzytkownik.GetUzytkownik();
            uzytkownicyData.Visibility = Visibility.Visible;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
        }
        private void Click_lekarzEdit(object sender, RoutedEventArgs e)
        {
            // lekarzeData.ItemsSource = 
            uzytkownicyData.Visibility = Visibility.Hidden;
            gridLekarz.Visibility = Visibility.Hidden;
            gridUzytkownik.Visibility = Visibility.Hidden;
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
            gridUzytkownik.Visibility = Visibility.Hidden;
        }

        private void nowyUzytkownik(object sender, RoutedEventArgs e)
        {
            gridUzytkownik.Visibility = Visibility.Visible;
            gridLekarz.Visibility = Visibility.Hidden;
            uzytkownicyData.Visibility = Visibility.Hidden;
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

        // Czyszczenie za każdym razem listy lekarzy i dodawanie na nowo tylko tych, którzy spełniają warunek podany w zapytaniu query_lista_lekarzy
        // Co sie dzieje? Jak sie wybierze lekarza, a potem wybierzesz Przychodnie, to wysypuje sie
                  //  if (lekarzeComboBox is ComboBox)
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
                var tmp = pacjentNameComboBox.SelectedItem.ToString();
                string[] words = tmp.Split(' ');
                string pacjentWybrany = "";
                for (int i = 1; i < words.Length; i++)
                    pacjentWybrany += words[i] + " ";
                var query = from p in db.Pacjenci where p.Nazwisko == pacjentWybrany select p;
                textBoxPacjentID.Text = query.ToString();
                 //nic nie otrzymuje na textBoxe, czemu?
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
            string selectedDate = calendar.SelectedDate.ToString();
            //data jest z formacie DATETIME, teraz trzeba z tym trafic do SQL - DATEPART
            //nic nie otrzymuje na textBoxe, czemu?
            textBoxPacjentID.Text = selectedDate;
        }

        private void pacjentIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

        }
    }
}

