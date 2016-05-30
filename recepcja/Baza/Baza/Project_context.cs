using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Baza.Klasy;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Baza
{
    class Project_context : DbContext
    {
        public DbSet<Lekarz> Lekarze { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // base.OnModelCreating(modelBuilder);
        }
    }
}
