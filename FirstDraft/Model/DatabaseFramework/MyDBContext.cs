using FirstDraft.Model.DatabaseFramework.Entities;
using Microsoft.EntityFrameworkCore;
using FirstDraft.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework;


public class MyDBContext : DbContext
{

    public TypeOfDatabase TypeOfDatabase { get; private init; }

    public DbSet<Festival> Festivals { get; private set; }
    public DbSet<FestivalsExtWorkersRelations> FestivalsExtWorkersRelations { get; private set; }
    public DbSet<ExternalWorker> ExternalWorkers { get; private set; }

    public MyDBContext(TypeOfDatabase t)
    {
        TypeOfDatabase = t;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (TypeOfDatabase == TypeOfDatabase.LocalSQLite)
        {
            var path = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(path, "chillDB.db");
            optionsBuilder
                .UseSqlite($"filename={databasePath}");
        }
        else if (TypeOfDatabase == TypeOfDatabase.CloudPostgreSQL)
        {
            optionsBuilder.UseNpgsql("Host=surus.db.elephantsql.com;" +
            "Database=vhbvixhu;" +
            "Username=vhbvixhu;" +
            "Password=y1DwR8W4lNuGyWIAwMl2NfhFbygEIU_a");
        }
        else
            throw new ArgumentException($"This app does not recognize database type {TypeOfDatabase}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // FestivalExtWorker relation
        modelBuilder.Entity<FestivalsExtWorkersRelations>().HasKey(few => new { few.IDFestival, few.IDExternalWorker });

        modelBuilder.Entity<FestivalsExtWorkersRelations>()
            .HasOne(few => few.Festival)
            .WithMany(f => f.FestivalsExtWorkersRelations)
            .HasForeignKey(few => few.IDFestival);

        modelBuilder.Entity<FestivalsExtWorkersRelations>()
            .HasOne(few => few.ExternalWorker)
            .WithMany(w => w.FestivalsExtWorkersRelations)
            .HasForeignKey(few => few.IDExternalWorker);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
