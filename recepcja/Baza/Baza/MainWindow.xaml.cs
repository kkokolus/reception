using System.Data.Entity;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Collections;
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
        IQueryable qry;
        IQueryable q3;
        string godzinaWizyty = "";
        string selectedDate = "";
        int numerDniaTygodnia = 0;
        string nazwisko_lekarza = "";
        int idWybranegoLekarza = 0;
        int iddyzur;
        public MainWindow()
        {
            InitializeComponent();

      // Stworzenie przykładowych lekarzy, przychodni itp. na potrzeby stworzenia bazy w SQL Server i testów
            var przykladowa_przychodnia = new Przychodnia { Rodzaj = "Okulistyczna" };
            var przykladowa_przychodnia2 = new Przychodnia { Rodzaj = "Rehabilitacyjna" };
            var przykladowy_lekarz = new Lekarz { Imie = "Jan", Nazwisko = "Kowalski", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz2 = new Lekarz { Imie = "Adam", Nazwisko = "Nowak", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz3 = new Lekarz { Imie = "Anna", Nazwisko = "Nowacka", PESEL = 93040234527, Telefon = 675384920 };
            
            var pacjent = new Pacjent { Imie = "Tomasz", Nazwisko = "Szklarski", PESEL = 12345678901234 };
            var pacjent2 = new Pacjent { Imie = "Antoni", Nazwisko = "Janowski", PESEL = 12345678901234 };
            var przykladowy_dyzur = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = 1, OdGodziny = 9, DoGodziny = 13 };
            var przykladowy_dyzur2 = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = 5, OdGodziny = 8, DoGodziny = 12 };
            var przykladowy_dyzur3 = new Dyzur { IDPrzychodnia = 2, IDLekarz = 3, DzienTygodnia = 2, OdGodziny = 10, DoGodziny = 16 };
            var przykladowy_dyzur4 = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = 3, OdGodziny = 0, DoGodziny = 0 };
            var przykladowy_dyzur5 = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = 4, OdGodziny = 10, DoGodziny = 16 };

            using (db)
            {
                try
                {
                    if(db.Lekarze.SingleOrDefault(s => s.Nazwisko == przykladowy_lekarz.Nazwisko)==null)
                    db.Lekarze.Add(przykladowy_lekarz);
                    if (db.Lekarze.SingleOrDefault(s => s.Nazwisko == przykladowy_lekarz2.Nazwisko) == null)
                    db.Lekarze.Add(przykladowy_lekarz2);
                    if (db.Lekarze.SingleOrDefault(s => s.Nazwisko == przykladowy_lekarz3.Nazwisko) == null)
                    db.Lekarze.Add(przykladowy_lekarz3);
                    db.Przychodnie.Add(przykladowa_przychodnia);
                    db.Przychodnie.Add(przykladowa_przychodnia2);
                    db.Pacjenci.Add(pacjent);
                    db.Pacjenci.Add(pacjent2);
                    db.SaveChanges();
                 if( db.Dyzury.SingleOrDefault(b => b.DzienTygodnia == przykladowy_dyzur.DzienTygodnia) == null)
                    db.Dyzury.Add(przykladowy_dyzur);
                 if (db.Dyzury.SingleOrDefault(b => b.DzienTygodnia == przykladowy_dyzur2.DzienTygodnia) == null)
                    db.Dyzury.Add(przykladowy_dyzur2);
                 if (db.Dyzury.SingleOrDefault(b => b.DzienTygodnia == przykladowy_dyzur3.DzienTygodnia) == null)
                    db.Dyzury.Add(przykladowy_dyzur3);
                 if (db.Dyzury.SingleOrDefault(b => b.DzienTygodnia == przykladowy_dyzur4.DzienTygodnia) == null)
                    db.Dyzury.Add(przykladowy_dyzur4);
                 if (db.Dyzury.SingleOrDefault(b => b.DzienTygodnia == przykladowy_dyzur5.DzienTygodnia) == null)
                    db.Dyzury.Add(przykladowy_dyzur5);
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
                    var q3 = db.Pacjenci.Select(x => x.Nazwisko + " " + x.Imie).OrderBy(s => s).ToList();
                    foreach (var item in q3)
                    {
                        ComboBoxItem P = new ComboBoxItem() { Content = item };
                        pacjentNameComboBox.Items.Add(P);
                    }
                    //siatka z dyżurami zablokowana az wybierze sie lekarza
                    WyczyscSiatketygodniowa();
                    PrzypiszEvent();
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
            WyczyscSiatketygodniowa();
            tuBedzieNazwisko.Content = "";
            Project_context db = new Project_context();
            if (lekarzeComboBox.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                string tmp = comboBox.SelectedValue.ToString();
                string[] words = tmp.Split(' ');
                nazwisko_lekarza = words[1];
                var tmpID = from l in db.Lekarze where l.Nazwisko == nazwisko_lekarza select l.IDLekarz;
                foreach (var i in tmpID){ idWybranegoLekarza = i;}
                tuBedzieNazwisko.Content = nazwisko_lekarza; 
                //siatka tygodniowa ma wypełnic się dyżurami wybranego lekarza w biezącym tygodniu
                
 //zapisuje dane o godzinach dyzurow lekarza do 2 list
                var dniDyzurow = from d in db.Dyzury where d.IDLekarz == idWybranegoLekarza orderby d.DzienTygodnia select d.DzienTygodnia;
                var odgodziny = from d in db.Dyzury where d.IDLekarz == idWybranegoLekarza orderby d.DzienTygodnia select d.OdGodziny;
                var dogodziny = from d in db.Dyzury where d.IDLekarz == idWybranegoLekarza orderby d.DzienTygodnia select d.DoGodziny;
                List<int> dniPracy = new List<int>();
                foreach(var item in dniDyzurow)
                {
                   dniPracy.Add(item);
                }
                List<int> start = new List<int>();
                foreach (var item in odgodziny)
                {
                    start.Add(item);
                }
                List<int> finish = new List<int>();
                foreach (var item in dogodziny)
                {
                   finish.Add(item);
                }
                int idx = 0;
                foreach(int s  in dniPracy )
                {
                    int ilegodzin = finish[idx] - s;                  
 
                //odblokowuje tyle przycisków ile godzin przyjmuje lekarz 
                    if (s == 1)
                    {
                        int licznik = 0;
                        foreach (UIElement b in _PN_.Children)
                        {
                            if (licznik < ilegodzin*2)
                            {
                                if (b is Button)
                                {
                                    string[] dyzurGodzina = ((Button)b).Content.ToString().Split(':');
                                    int g = Convert.ToInt32(dyzurGodzina[0]);
                                    if (g >= start[idx] && g<= finish[idx])
                                    {
                                   ((Button)b).IsEnabled = true;
                                    ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                                    licznik++;
                                    }                                  
                                }
                            }
                        }
                        idx++;
                        continue;
                    }
                 else   if (s == 2)
                    {
                        int licznik = 0;
                        foreach (UIElement b in _WT_.Children)
                        {
                            if (licznik < ilegodzin * 2)
                            {
                                if (b is Button)
                                {
                                    string[] dyzurGodzina = ((Button)b).Content.ToString().Split(':');
                                    int g = Convert.ToInt32(dyzurGodzina[0]);
                                    if (g >= start[idx] && g <= finish[idx])
                                    {
                                        ((Button)b).IsEnabled = true;
                                        ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                                        licznik++;
                                    }
                                }
                            }
                        }
                        idx++;
                        continue;
                    }
                  else  if (s == 3)
                    {
                        int licznik = 0;
                        foreach (UIElement b in _SR_.Children)
                        {
                            if (licznik < ilegodzin * 2)
                            {
                                if (b is Button)
                                {
                                    string[] dyzurGodzina = ((Button)b).Content.ToString().Split(':');
                                    int g = Convert.ToInt32(dyzurGodzina[0]);
                                    if (g >= start[idx] && g <= finish[idx])
                                    {
                                        ((Button)b).IsEnabled = true;
                                        ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                                        licznik++;
                                    }
                                }
                            }
                        }
                        idx++;
                        continue;
                    }
                  else  if (s == 4)
                    {
                        int licznik = 0;
                        foreach (UIElement b in _CZ_.Children)
                        {
                            if (licznik < ilegodzin * 2)
                            {
                                if (b is Button)
                                {
                                    string[] dyzurGodzina = ((Button)b).Content.ToString().Split(':');
                                    int g = Convert.ToInt32(dyzurGodzina[0]);
                                    if (g >= start[idx] && g <= finish[idx])
                                    {
                                        ((Button)b).IsEnabled = true;
                                        ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                                        licznik++;
                                    }
                                }
                            }
                        }
                        idx++;
                        continue;
                    }
                 else   if (s == 5)
                    {
                        int licznik = 0;
                        foreach (UIElement b in _PT_.Children)
                        {
                            if (licznik < ilegodzin * 2)
                            {
                                if (b is Button)
                                {
                                    string[] dyzurGodzina = ((Button)b).Content.ToString().Split(':');
                                    int g = Convert.ToInt32(dyzurGodzina[0]);
                                    if (g >= start[idx] && g <= finish[idx])
                                    {
                                        ((Button)b).IsEnabled = true;
                                        ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                                        licznik++;
                                    }
                                }
                            }
                        }
                        idx++;
                        continue;
                    }
                }
            }          
        }

        private void calendarSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Project_context db = new Project_context();
            selectedDate = calendar.SelectedDate.ToString();                     
            string[] dataTmp = selectedDate.Split(' ');
            string[] data = dataTmp[0].Split('.');         
            //string[] tydzien = { "poniedziałek", "wtorek", "sroda", "czwartek", "piatek", "sobota", "niedziela" };
            int dzienTygodnia = DzienTygodnia(data);
            // textBoxPacjentID.Text = tydzien[dzienTygodnia];           
            //chcę dodać wyswietlanie daty na siatce tygodniowej
      
            UIElement[] dniSiatkitygodniowej = { stackPN, stackWT, stackSR, stackCZ, stackPT };
            foreach (StackPanel day in dniSiatkitygodniowej)
            {
                ///if (day.Children is Label)
                 ///   (day.Children)..Content = "";
            }
            if (dzienTygodnia == 0)
            {
                label_pndData.Content = data[0] + '/' + data[1];
                stackPN.Background = new SolidColorBrush(Colors.OliveDrab);
            }               
            if (dzienTygodnia == 1)
            {
                label_wtrData.Content = data[0] + '/' + data[1];
                stackPN.Background = new SolidColorBrush(Colors.OliveDrab);
            }                
            if (dzienTygodnia == 2)
            {
                stackPN.Background = new SolidColorBrush(Colors.OliveDrab);
                label_srdData.Content = data[0] + '/' + data[1];
            }               
            if (dzienTygodnia == 3)
            {
                stackPN.Background = new SolidColorBrush(Colors.OliveDrab);
                label_cztData.Content = data[0] + '/' + data[1];
            }               
            if (dzienTygodnia == 4)
            {
                stackPN.Background = new SolidColorBrush(Colors.OliveDrab);
                label_ptnData.Content = data[0] + '/' + data[1];
            }
                
           
            using (db)
            {
                  // trzeba pobrac z bazy dane dot. dyzurow
            }

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
       
        public void WyczyscSiatketygodniowa()
        {
            UIElementCollection[] dniSiatkitygodniowej = { _PN_.Children, _WT_.Children, _SR_.Children, _CZ_.Children, _PT_.Children };
            foreach (var day in dniSiatkitygodniowej)
            {
            foreach (UIElement b in day)
                {
                if (b is Button)
                    ((Button)b).IsEnabled = false;
                }
            } 
        }
        public void PrzypiszEvent()
        {
            UIElementCollection[] dniSiatkitygodniowej = { _PN_.Children, _WT_.Children, _SR_.Children, _CZ_.Children, _PT_.Children };
            foreach (var day in dniSiatkitygodniowej)
            {
                foreach (UIElement b in day)
                {
                    if (b is Button)
                        ((Button)b).Click += Click_dyzurButton;
                }                    
            }
        }
  
        private void Click_dyzurButton(object sender, RoutedEventArgs e)
        {
            //czyszczenie wczesniej zaznaczonych na pomaranczowo terminow
            UIElementCollection[] dniSiatkitygodniowej = { _PN_.Children, _WT_.Children, _SR_.Children, _CZ_.Children, _PT_.Children };
            foreach (var day in dniSiatkitygodniowej)
            {
                foreach (UIElement btn in day)
                {
                    if (btn is Button)
                    {
                        if (((Button)btn).Background.ToString() == Colors.Orange.ToString())
                            ((Button)btn).Background = new SolidColorBrush(Colors.LimeGreen);
                    }
                }
            }
            //zamiana koloru terminu + godzina wizyty zapisywana do zmiennej
            UIElement b = sender as UIElement;
            if (b is Button)
            {
                if (((Button)b).Background.ToString() == Colors.LimeGreen.ToString())
                {
                    ((Button)b).Background = new SolidColorBrush(Colors.Orange);
                    godzinaWizyty = ((Button)b).Content.ToString();

                    //by znalezc ID dyzuru - musze znac wybrany dzien tygodnia                   
                    foreach (var day in dniSiatkitygodniowej)
                    {
                        foreach (UIElement bt in day)
                        {
                            if (bt is Button)
                            {
                                if (((Button)bt).Background.ToString() == Colors.Orange.ToString())
                                {
                                    if (day == _PN_.Children)
                                        numerDniaTygodnia = 1;
                                    if (day == _WT_.Children)
                                        numerDniaTygodnia = 2;
                                    if (day == _SR_.Children)
                                        numerDniaTygodnia = 3;
                                    if (day == _CZ_.Children)
                                        numerDniaTygodnia = 4;
                                    if (day == _PT_.Children)
                                        numerDniaTygodnia = 5;
                                }
                            }
                        }
                    }
                }
                else if (((Button)b).Background.ToString() == Colors.Orange.ToString())
                {
                    ((Button)b).Background = new SolidColorBrush(Colors.LimeGreen);
                    godzinaWizyty = "";
                    numerDniaTygodnia = 0;
                }
            }
        }

        private void textBox_tylkoLiczby(object sender, TextCompositionEventArgs e)//tylko liczby
       {
           TextBox t = (TextBox)sender;
            if (t.Name.Contains("Telefon"))
           {
               if (!Char.IsDigit(e.Text,0)&&!e.Text.Contains('-'))
               e.Handled = true;            }            
            else
            {
               if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
            }           
        }
        private void textBox_tylkoLitery(object sender, TextCompositionEventArgs e)//tylko litery
        { 
               if (!Char.IsLetter(e.Text, 0) && !e.Text.Contains('-'))
                    e.Handled = true;
        }
        //funkcja zwraca dzien tygodnia dla wybranej daty
        public static int DzienTygodnia(string[] data)
        {
            int[] liczbaDni = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};
            int dzien = Convert.ToInt32(data[0]);
            int miesiac = Convert.ToInt32(data[1]);
            int rok = Convert.ToInt32(data[2]);
            if ((rok % 4 == 0 && rok % 100 != 0) || rok % 400 == 0)
            {
                for (int i = 2; i < liczbaDni.Length; i++)
                    liczbaDni[i] += 1;
            }
            int dzienRoku =0;
            int yy, c, g;
            int wynik=0;
            dzienRoku = dzien + liczbaDni[miesiac - 1];
            yy = (rok - 1) % 100;
            c = (rok - 1) - yy;
            g = yy + (yy / 4);
            wynik = (((((c / 100) % 4) * 5) + g) % 7);
            wynik += dzienRoku - 1;
            wynik %= 7;
            return wynik;
        }

        private void zapiszPacjenta_Click(object sender, RoutedEventArgs e)
        {
            Project_context db = new Project_context();
            using (db)
            {
                var nowy_pacjent = new Pacjent { Imie = nowyPacjentImie.Text, Nazwisko = nowyPacjentNazwisko.Text, Adres = nowyPacjentAdres.Text, PESEL = Int64.Parse(nowyPacjentPESEL.Text), Telefon = Int64.Parse(nowyPacjentTelefon.Text) };
                db.Pacjenci.Add(nowy_pacjent);
                db.SaveChanges();
            }
        }
        private void zapiszLekarza_Click(object sender, RoutedEventArgs e)
        {
            Project_context db = new Project_context();
            using (db)
            {
                var nowy_lekarz = new Lekarz { Imie = textLekarzImie.Text, Nazwisko = textLekarzNazwisko.Text, Adres = textAdresLekarz.Text, PESEL = Int64.Parse(textLekarzPESEL.Text), Telefon = Int64.Parse(textTelefonLekarz.Text) };
                db.Lekarze.Add(nowy_lekarz);
                db.SaveChanges();
            }
        }

        private void zapiszPrzychodnie_Click(object sender, RoutedEventArgs e)
        {
            Project_context db = new Project_context();
            using (db)
            {
                var nowa_przychodnia = new Przychodnia { Rodzaj = nowaPrzychodniaRodzaj.Text };
                db.Przychodnie.Add(nowa_przychodnia);
                db.SaveChanges();
            }
        }

        private void zapiszDyzur_Click(object sender, RoutedEventArgs e)
        { 
       // To jest koniecznie do dokończenia!

            /*var tmp = nowyDyzurNazwaPrzychodni.Text;
           // string[] words = tmp.Split(' ');
           // var nazwa_przychodni = words[1];

            var q2 = from rodzaje in db.Przychodnie orderby rodzaje.Rodzaj select rodzaje.Rodzaj;
            */

        }

        private void Click_dodajWizyte(object sender, RoutedEventArgs e)
        {
            Project_context db = new Project_context();
            using (db)
            {
                var _iddyzur = from d in db.Dyzury where d.DzienTygodnia == numerDniaTygodnia && d.IDLekarz == idWybranegoLekarza select d.IDDyzur;
                foreach (var v in _iddyzur)
                    iddyzur = v;
                //podsumujmy: taka data bardziej poprawna by byla - 2016-06-23 23:40:51.840 ale mamy tam stringa, wiec to nieistotne
                var wizyta = new Wizyta
                {
                    IDPacjent = Int32.Parse(textBoxPacjentID.Text),
                    IDDyzur = iddyzur,
                    GodzinaWizyty = godzinaWizyty,
                    DataWizyty = selectedDate
                };
                db.Wizyty.Add(wizyta);
                db.SaveChanges();
            }
        }
    }
}

