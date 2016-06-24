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
        public Project_context() : base("Recepcja")
              { }
        public DbSet<Lekarz> Lekarze { get; set; }
        public DbSet<Dyzur> Dyzury { get; set; }
        public DbSet<Przychodnia> Przychodnie { get; set; }
        public DbSet<Wizyta> Wizyty { get; set; }
        public DbSet<Pacjent> Pacjenci { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // base.OnModelCreating(modelBuilder);
        }
    }
}
