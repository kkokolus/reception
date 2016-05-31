using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Pacjent
    {
        public Pacjent()
        {
            this.Wizyty = new HashSet<Wizyta>();
        }

        [Key]
        public int IDPacjent { get; set; }
        public long PESEL { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Adres { get; set; }
        public long Telefon { get; set; }
        public virtual ICollection<Wizyta> Wizyty { get; set; }
    }
}
