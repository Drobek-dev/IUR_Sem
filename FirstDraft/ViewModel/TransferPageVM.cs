
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(OriginalLocation), nameof(OriginalLocation))]
[QueryProperty(nameof(OriginalLocationID), nameof(OriginalLocationID))]

[QueryProperty(nameof(NewLocation), nameof(NewLocation))]
[QueryProperty(nameof(Equipment), nameof(Equipment))]
public partial class TransferPageVM : ObservableObject
{
    [ObservableProperty]
    string _originalLocation;

    [ObservableProperty]
    Guid _originalLocationID;

    [ObservableProperty]
    string _newLocation;

    [ObservableProperty]
    ObservableCollection<Equipment> _equipment;
    
    [ObservableProperty]
    ObservableCollection<TargetType> _searchResults;

    [ObservableProperty]
    ObservableCollection<TargetType> _target;

    

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            
            SearchResults = Target;
        }
        else
        {
            SearchResults = new(Target.Where(t => t.Name.Equals(query)).ToList());
        }
    });

    [RelayCommand]
    async Task LoadTargetInfo()
    {
        using MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);
        Target = new();

        if (NewLocation.Equals(LocationTypes.festival))
        {
            var Festivals = await context.Festivals.OrderByDescending(f => f.ID).ToListAsync();
            foreach(var f in Festivals) 
            {
                Target.Add(new() { Name = f.Name, ID = f.ID });  
            }
        }
        else if (NewLocation.Equals(LocationTypes.warehouse))
        {
            var Warehouses = await context.Warehouses.OrderByDescending(w => w.ID).ToListAsync();
            foreach (var w in Warehouses)
            {
                Target.Add(new() { Name = w.Name, ID = w.ID });
            }
        }
        else if (NewLocation.Equals(LocationTypes.transport))
        {
            var Transports = await context.Transports.OrderByDescending(t => t.ID).ToListAsync();
            foreach (var t in Transports)
            {
                Target.Add(new() { Name = t.TransportName, ID = t.ID });
            }
        }
        else if (NewLocation.Equals(LocationTypes.bin))
        {
            Target.Add(new() { Name = "Bin" });
        }
        SearchResults = Target;
    }


    void AssignEquipmentToNewLocation(MyDBContext context, Guid IDTarget)
    {
        if (NewLocation.Equals(LocationTypes.festival))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInFestivals.Add(new() { IDEquipment = e.ID, IDFestival = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(LocationTypes.warehouse))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInWarehouses.Add(new() { IDEquipment = e.ID, IDWarehouse = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(LocationTypes.transport))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInTransports.Add(new() { IDEquipment = e.ID, IDTransport = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(LocationTypes.bin))
        {
            foreach (var e in Equipment)
            {
                context.Bin.Add(new() { IDEquipment = e.ID });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
    }

    async Task DeleteOldEquipmentLocation(MyDBContext context)
    {
        if (OriginalLocation.Equals(LocationTypes.festival))
        {
            foreach (var e in Equipment)
            {
                try
                {

                    context.EquipmentInFestivals.Remove(await context.EquipmentInFestivals.FindAsync(e.ID, OriginalLocationID));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }
        else if (OriginalLocation.Equals(LocationTypes.warehouse))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInWarehouses.Remove(await context.EquipmentInWarehouses.FindAsync(e.ID, OriginalLocationID));

            }
        }
        else if (OriginalLocation.Equals(LocationTypes.transport))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInTransports.Remove(await context.EquipmentInTransports.FindAsync(e.ID, OriginalLocationID));
            }
        }
        else if (OriginalLocation.Equals(LocationTypes.bin))
        {
            foreach (var e in Equipment)
            {
                context.Bin.Remove(await context.Bin.FindAsync(e.ID));
            }
        }
    }

    async Task RedirectToParentShellPage()
    {
        if (OriginalLocation.Equals(LocationTypes.festival))
        {
            await Shell.Current.GoToAsync(nameof(AllFestivalsPage));

        }
        else if (OriginalLocation.Equals(LocationTypes.warehouse))
        {
            await Shell.Current.GoToAsync(nameof(WarehousesPage));
        }
        else if (OriginalLocation.Equals(LocationTypes.transport))
        {
            await Shell.Current.GoToAsync(nameof(TransportsPage));
        }
        else if (OriginalLocation.Equals(LocationTypes.bin))
        {
            await Shell.Current.GoToAsync(nameof(EquipmentPage));
        }
    }
    [RelayCommand]
    async Task TransferEquipment(Guid IDTarget)
    {

        if (IDTarget.Equals(OriginalLocationID))
            return;

        using MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);

        AssignEquipmentToNewLocation(context, IDTarget);
        await DeleteOldEquipmentLocation(context);    


        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.ToString());   
        }
        

        await RedirectToParentShellPage();
    }


}
