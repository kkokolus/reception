using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Lekarz
    {
        public virtual int IDLekarz { get; set; }
        public virtual int PESEL { get; set; }
        public virtual string Data_zatrudnienia { get; set; }
        public virtual string Imie { get; set; }
        public virtual string Nazwisko { get; set; }
        public virtual string Ulica { get; set; }
        public virtual string Nr_domu { get; set; }
        public virtual string Miasto { get; set; }
        public virtual int Telefon { get; set; }
    }
}
