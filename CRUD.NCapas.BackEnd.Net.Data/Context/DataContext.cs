using CRUD.NCapas.BackEnd.Net.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.NCapas.BackEnd.Net.Data.Context
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        #region DataSets

        public DbSet<Persona> Persona { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(k => k.IdPersona);
                entity.Property(x => x.NoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
