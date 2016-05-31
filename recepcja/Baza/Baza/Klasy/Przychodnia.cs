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
            this.Dyzury =  new HashSet<Dyzur>(); 
        }

        [Key]
        public int IDPrzychodnia { get; set; }
        public string Rodzaj { get; set; }
        public virtual ICollection<Dyzur> Dyzury { get; set; }
        
    }
}
