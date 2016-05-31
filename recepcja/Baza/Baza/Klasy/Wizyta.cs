using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Wizyta
    {
        [Key]
        public int IDWizyta { get; set; }
        public int IDPacjent { get; set; }
        public int IDDyzur { get; set; }
        public string Opis { get; set; }
        public string DataWizyty { get; set; }
        public string GodzinaWizyty { get; set; }
        public string DataRejestracji { get; set; }
        public string GodzinaRejestracji { get; set; }

        public virtual Dyzur Dyzur { get; set; }
        public virtual Pacjent Pacjent { get; set; }
    }
}
