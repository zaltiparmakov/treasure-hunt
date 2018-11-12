using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class LovNaZakladDbContext : DbContext
    {
        public LovNaZakladDbContext() : base()
        {
            // disable proxy creation when querying
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Treasure> Treasures { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LovNaZakladDbContext>(null);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>().HasRequired(m => m.Location)
                            .WithMany(m => m.LocationQuestions).HasForeignKey(m => m.LocationID)
                            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>().HasRequired(m => m.NextLocation)
                            .WithMany(m => m.NextLocationQuestions).HasForeignKey(m => m.NextLocationID)
                            .WillCascadeOnDelete(false);

        }

    }
}