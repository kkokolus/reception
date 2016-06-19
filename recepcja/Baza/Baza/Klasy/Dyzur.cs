using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Dyzur
    {
        public Dyzur()
        {
            this.Wizyty = new HashSet<Wizyta>();
        }

        [Key]
        public int IDDyzur { get; set; }
        public int IDPrzychodnia { get; set; }
        public int IDLekarz { get; set; }
        public int OdGodziny { get; set; }
        public int DoGodziny { get; set; }
        public string NrGabinetu { get; set; }
        public int DzienTygodnia { get; set; }

        public virtual Lekarz Lekarz { get; set; }
        public virtual Przychodnia Przychodnia { get; set; }
        public virtual ICollection<Wizyta> Wizyty { get; set; }
        
    }
}
