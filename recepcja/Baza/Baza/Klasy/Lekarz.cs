using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Lekarz
    {
        [Key]
        public int IDLekarz { get; set; }
        public long PESEL { get; set; }
        public string Data_zatrudnienia { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Ulica { get; set; }
        public string Nr_domu { get; set; }
        public string Miasto { get; set; }
        public int Telefon { get; set; }
    }
}
