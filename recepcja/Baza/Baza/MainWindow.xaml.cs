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
        IQueryable qry;
        IQueryable q3;




        /*
        // Wątek który ma sprawdzać która specjalizacja jest wybrana i wyświetlać na liście lekarzy lekarzy którzy mają dyżur w tej przychodni
        void doSomething()
        {
            if(przychodnieComboBox.SelectedItem != null)
            foreach (var i in q3)
            {
                ComboBoxItem Le = new ComboBoxItem() { Content = (string)i };
                lekarzeComboBox.Items.Add(Le);
            }
        }*/

        public MainWindow()
        {
            InitializeComponent();


            /* Thread thr = new Thread(doSomething);
           thr.Start();*/

      // Stworzenie przykładowych lekarzy, przychodni itp. na potrzeby stworzenia bazy w SQL Server i testów
            var przykladowy_lekarz = new Lekarz { Imie = "Jan", Nazwisko = "Kowalski", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz2 = new Lekarz { Imie = "Adam", Nazwisko = "Nowak", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowy_lekarz3 = new Lekarz { Imie = "Anna", Nazwisko = "Nowacka", PESEL = 93040234527, Telefon = 675384920 };
            var przykladowa_przychodnia = new Przychodnia { Rodzaj = "Okulistyczna" };
            var przykladowa_przychodnia2 = new Przychodnia { Rodzaj = "Rehabilitacyjna" };

            var przykladowy_dyzur = new Dyzur { IDPrzychodnia = 1, IDLekarz = 2 };
            var przykladowy_dyzur2 = new Dyzur { IDPrzychodnia = 1, IDLekarz = 1, DzienTygodnia = "Poniedziałek" };
            var przykladowy_dyzur3 = new Dyzur { IDPrzychodnia = 2, IDLekarz = 3, DzienTygodnia = "Poniedziałek" };

            // Inny sposób na dodawanie

            //using (var dbx = new Project_context())
            //{
            //    dbx.Entry(przykladowy_lekarz).State = EntityState.Added;
            //    dbx.SaveChanges();
            //}

            using (db)
            {
                try
                {
                    db.Lekarze.Add(przykladowy_lekarz); //w tym miejscu u mnie zatrzymuje sie :(( error  26
                    db.Lekarze.Add(przykladowy_lekarz2);
                    db.Lekarze.Add(przykladowy_lekarz3);
                    db.Przychodnie.Add(przykladowa_przychodnia);
                    db.Przychodnie.Add(przykladowa_przychodnia2);
                    db.SaveChanges();
                    db.Dyzury.Add(przykladowy_dyzur);
                    db.Dyzury.Add(przykladowy_dyzur2);
                    db.Dyzury.Add(przykladowy_dyzur3);
                    db.SaveChanges();

                    qry = from b in db.Lekarze
                          orderby b.Nazwisko
                          select b.Nazwisko;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

        // Dodawanie przychodni na listę przychodni
                var q2 = from rodzaje in db.Przychodnie orderby rodzaje.Rodzaj select rodzaje.Rodzaj;

                foreach (var item in q2)
                {
                    ComboBoxItem R = new ComboBoxItem() { Content = (string)item };
                    przychodnieComboBox.Items.Add(R);

                }

                // Query które sprawdza co jest wybrane na liście rodzajów przychodni i zwraca nazwiska lekarzy którzy mają tam dyżur
                //  q3 = from z in db.Dyzury join przych in db.Przychodnie on z.IDPrzychodnia equals przych.IDPrzychodnia join lek in db.Lekarze on z.IDLekarz equals lek.IDLekarz orderby z.IDLekarz where (przych.Rodzaj).ToString() == przychodnieComboBox.SelectedItem.ToString() select lek.Nazwisko;

                //  lekarzeComboBox.Items.Clear();

                /*   foreach (var i in qry)
                   {
                       ComboBoxItem Le = new ComboBoxItem() { Content = (string)i };
                       lekarzeComboBox.Items.Add(Le);
                   }*/


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
            /*    ComboBoxItem combo = new ComboBoxItem() { Content = "Kowalski"};
                        lekarzeComboBox.Items.Add(combo);*/

        }


        // var query = from b in db.Lekarze orderby b.Nazwisko where b.IDLekarz == 1 select b.Nazwisko;     

        //query = from b in db.Specjalisci orderby b.Nazwa select b.Nazwa;
        //foreach (var item in query)
        //{
        //    ComboBoxItem Le = new ComboBoxItem() { Content = (string)item };
        //    specjalizacjaComboBox.Items.Add(Le);
        //}





        //  ComboBoxItem combo = new ComboBoxItem() { Content = "Kowalski"};


        // Dodawanie listy lekarzy do Comboboxa 
        // lekarzeComboBox.Items.Add(combo);



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
                //from d in db.Dyzury select d.DzienTygodnia;

                /*  from z in db.Dyzury.Local
                  join przych in db.Przychodnie on z.IDPrzychodnia equals przych.IDPrzychodnia 
                  join lek in db.Lekarze on z.IDLekarz equals lek.IDLekarz
                  where przych.Rodzaj.ToString() == g
                  //where z.IDPrzychodnia == przych.IDPrzychodnia && z.IDLekarz == lek.IDLekarz && przych.Rodzaj.ToString() == g
                  //select przych.IDPrzychodnia;
                  select lek.Nazwisko;*/

                from l in db.Lekarze
                join dyz in db.Dyzury on l.IDLekarz equals dyz.IDLekarz
                join przych in db.Przychodnie on dyz.IDPrzychodnia equals przych.IDPrzychodnia
                where przych.Rodzaj == nazwa_przychodni
                select l.Nazwisko;



                //join l in db.Lekarze on d.IDLekarz equals l.IDLekarz where d.IDPrzychodnia == przychodnieComboBox.SelectedIndex select l.Nazwisko;

                //from b in db.Lekarze
                //where b.IDLekarz == ( from i  in db.Srecjalizacja where i.Nazwa == comboItemText select i.ID <- podobne podzapytanie albo JOIN
                //orderby b.Nazwisko
                //select b.Nazwisko;


                /*from z in db.Dyzury.Local
                join przych in db.Przychodnie on z.IDPrzychodnia equals przych.IDPrzychodnia
                join lek in db.Lekarze on z.IDLekarz equals lek.IDLekarz

                where przych.Rodzaj.ToString() == przychodnieComboBox.SelectedValue.ToString()
                select lek.Nazwisko;*/

        // Czyszczenie za każdym razem listy lekarzy i dodawanie na nowo tylko tych, którzy spełniają warunek podany w zapytaniu query_lista_lekarzy
                lekarzeComboBox.Items.Clear();

                var t = string.Join(" ", query_lista_lekarzy);
                var f = t.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in f)
                {
                    ComboBoxItem combo = new ComboBoxItem() { Content = item };
                    lekarzeComboBox.Items.Add(combo);
                }
 


            }

            /*if (comboBox.Name == "lekarzeComboBox")
            {
                tuBedzieNazwisko.Content = comboItemText;*/
            //   }


        }


    }
}


/*
 *  ComboBox comboBox = (ComboBox)sender;
            int selectedIndex = comboBox.SelectedIndex;
            string[] selectedValue = comboBox.SelectedValue.ToString().Split();
            string comboItemText = "";
            for (int i = 1; i < selectedValue.Length; i++)
                comboItemText += selectedValue[i] + " ";
            //teraz mozna cos z tym comboItemText zrobic: LINQ -> lista lekarzy o wybranej specjalizacji -> foreach add item



            var query = //from b in db.Lekarze
                        //where b.IDLekarz == ( from i  in db.Srecjalizacja where i.Nazwa == comboItemText select i.ID <- podobne podzapytanie albo JOIN
                        //orderby b.Nazwisko
                        //select b.Nazwisko;

             from z in db.Dyzury.Local join przych in db.Przychodnie on z.IDPrzychodnia equals przych.IDPrzychodnia join lek in db.Lekarze on z.IDLekarz equals lek.IDLekarz

             where przych.Rodzaj.ToString() == przychodnieComboBox.SelectedItem.ToString()
                 select lek.Nazwisko;

                                            // where (przych.Rodzaj) == comboItemText

            //select z.Nazwisko;

            //  orderby b.Nazwisko
            // select b.Nazwisko;

        
*/