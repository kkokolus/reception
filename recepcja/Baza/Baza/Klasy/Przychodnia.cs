using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Klasy
{
    public class Przychodnia
    {
        public Przychodnia()
        {
            this.Lekarze = new HashSet<Lekarz>();
        }

        [Key]
        public int IDPrzychodnia { get; set; }
        public string Rodzaj { get; set; }
        public virtual ICollection<Lekarz> Lekarze { get; set; }

    }
}
