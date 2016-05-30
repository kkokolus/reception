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

            using (var db = new Project_context())
            {

                var L = new Lekarz { Imie = "Jan", Nazwisko = "Kowalski", PESEL = 93040234527 };
                db.Lekarze.Add(L);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.Lekarze
                            orderby b.Imie
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Imie);
                }

                
            }

        }
    }
}
