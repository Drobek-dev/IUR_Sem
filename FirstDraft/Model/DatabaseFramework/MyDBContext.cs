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
    public DbSet<EquipmentInFestival> EquipmentInFestivals { get; private set; }
    public DbSet<ExternalWorker> ExternalWorkers { get; private set; }
    public DbSet<Construction> Constructions { get; private set; }
    public DbSet<Deconstruction> Deconstructions { get; private set; }
    public DbSet<Equipment> Equipment { get; private set; }

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

        modelBuilder.Entity<ConstructionDaysRelations>().HasKey(cdr => new { cdr.IDConstruction, cdr.IDDay });

        modelBuilder.Entity<ConstructionDaysRelations>()
            .HasOne(cdr => cdr.Construction)
            .WithMany(c => c.ConstructionDaysRelations)
            .HasForeignKey(cdr => cdr.IDConstruction);

        modelBuilder.Entity<DeconstructionDaysRelations>().HasKey(cdr => new { cdr.IDDeconstruction, cdr.IDDay });
        modelBuilder.Entity<DeconstructionDaysRelations>()
            .HasOne(cdr => cdr.Deconstruction)
            .WithMany(c => c.DeconstructionDaysRelations)
            .HasForeignKey(cdr => cdr.IDDeconstruction);

        /*modelBuilder.Entity<EquipmentInFestival>().HasKey(eif => new { eif.IDFestival,eif.IDEquipment });
        modelBuilder.Entity<EquipmentInFestival>()
            .HasOne(eir => eir.Festival)
            .WithMany(f => f.EquipmentInFestival)
            .HasForeignKey(eir => eir.IDEquipment);*/

    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
