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
        public Lekarz()
        {
            this.Dyzury = new HashSet<Dyzur>();       
        }

        [Key]
        public int IDLekarz { get; set; }
        public int IDPrzychodnia { get; set; }
        public long PESEL { get; set; }
        public string Data_zatrudnienia { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Adres { get; set; }
        public long Telefon { get; set; }

        public virtual ICollection<Dyzur> Dyzury { get; set; }
        public virtual Przychodnia Przychodnia { get; set; }
    }
}
