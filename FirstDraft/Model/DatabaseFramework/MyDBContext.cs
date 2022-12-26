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
    
    public DbSet<Warehouse> Warehouses { get; private set; }
    public DbSet<Transport> Transports { get; private set; }
    public DbSet<EquipmentInBin> Bin { get; private set; }

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
            optionsBuilder.LogTo((string m)=>System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Information);
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

        modelBuilder.Entity<EquipmentInFestival>().HasKey(eif => new { eif.IDEquipment, eif.IDFestival });
        modelBuilder.Entity<EquipmentInFestival>()
            .HasOne(eif => eif.Festival)
            .WithMany(f => f.LocalEquipmentRelation)
            .HasForeignKey(eif => eif.IDFestival);
        modelBuilder.Entity<EquipmentInFestival>()
            .HasOne(eif => eif.Equipment)
            .WithMany(e => e.EquipmentInFestival)
            .HasForeignKey(eif=> eif.IDEquipment);

        modelBuilder.Entity<EquipmentInWarehouse>().HasKey(eiw => new { eiw.IDEquipment, eiw.IDWarehouse });
        modelBuilder.Entity<EquipmentInWarehouse>()
            .HasOne(eiw => eiw.Warehouse)
            .WithMany(w => w.LocalEquipmentRelations)
            .HasForeignKey(eiw => eiw.IDWarehouse);
        modelBuilder.Entity<EquipmentInWarehouse>()
            .HasOne(eiw => eiw.Equipment)
            .WithMany(e => e.EquipmentInWarehouse)
            .HasForeignKey(eiw => eiw.IDEquipment);

        modelBuilder.Entity<EquipmentInTransport>().HasKey(eit => new { eit.IDEquipment, eit.IDTransport });
        modelBuilder.Entity<EquipmentInTransport>()
            .HasOne(eit=> eit.Transport)
            .WithMany(t => t.LocalEquipmentRelations)
            .HasForeignKey(eit => eit.IDTransport);

        modelBuilder.Entity<EquipmentInTransport>()
            .HasOne(eit => eit.Equipment)
            .WithMany(e => e.EquipmentInTransport)
            .HasForeignKey(eit => eit.IDEquipment);



    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
