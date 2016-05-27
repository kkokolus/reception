using Baza.Klasy;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Mapowanie
{
    public class Mapowanie_Lekarz : ClassMap<Lekarz>
    {
        public Mapowanie_Lekarz()
        {
            Id(x => x.IDLekarz);
            Map(x => x.PESEL).Not.Nullable();
            Map(x => x.Data_zatrudnienia);
            Map(x => x.Imie).Not.Nullable();
            Map(x => x.Nazwisko).Not.Nullable();
            Map(x => x.Ulica);
            Map(x => x.Nr_domu);
            Map(x => x.Miasto);
            Map(x => x.Telefon);

        }

    }
}
