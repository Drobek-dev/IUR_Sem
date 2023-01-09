
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
public partial class TransferPageVM : BaseVM
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
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
            return;

        Target = new();

        if (NewLocation.Equals(GlobalValues.festival))
        {
            var Festivals = await c.Festivals.OrderByDescending(f => f.ID).ToListAsync();
            foreach(var f in Festivals) 
            {
                Target.Add(new() { Name = f.Name, ID = f.ID });  
            }
        }
        else if (NewLocation.Equals(GlobalValues.warehouse))
        {
            var Warehouses = await c.Warehouses.OrderByDescending(w => w.ID).ToListAsync();
            foreach (var w in Warehouses)
            {
                Target.Add(new() { Name = w.Name, ID = w.ID });
            }
        }
        else if (NewLocation.Equals(GlobalValues.transport))
        {
            var Transports = await c.Transports.OrderByDescending(t => t.ID).ToListAsync();
            foreach (var t in Transports)
            {
                Target.Add(new() { Name = t.TransportName, ID = t.ID });
            }
        }
        else if (NewLocation.Equals(GlobalValues.bin))
        {
            Target.Add(new() { Name = "Bin" });
        }
        SearchResults = Target;
    }

    void AssignEquipmentToNewLocation(MyDBContext context, Guid IDTarget)
    {
        if (NewLocation.Equals(GlobalValues.festival))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInFestivals.Add(new() { IDEquipment = e.ID, IDFestival = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(GlobalValues.warehouse))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInWarehouses.Add(new() { IDEquipment = e.ID, IDWarehouse = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(GlobalValues.transport))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInTransports.Add(new() { IDEquipment = e.ID, IDTransport = IDTarget });
                context.Equipment.Attach(e);
                e.Location = NewLocation;
            }
        }
        else if (NewLocation.Equals(GlobalValues.bin))
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
        if (OriginalLocation.Equals(GlobalValues.festival))
        {
            foreach (var e in Equipment)
           {
                context.EquipmentInFestivals.Remove(await context.EquipmentInFestivals.FindAsync(e.ID, OriginalLocationID));
   
            }
        }
        else if (OriginalLocation.Equals(GlobalValues.warehouse))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInWarehouses.Remove(await context.EquipmentInWarehouses.FindAsync(e.ID, OriginalLocationID));

            }
        }
        else if (OriginalLocation.Equals(GlobalValues.transport))
        {
            foreach (var e in Equipment)
            {
                context.EquipmentInTransports.Remove(await context.EquipmentInTransports.FindAsync(e.ID, OriginalLocationID));
            }
        }
        else if (OriginalLocation.Equals(GlobalValues.bin))
        {
            foreach (var e in Equipment)
            {
                context.Bin.Remove(await context.Bin.FindAsync(e.ID));
            }
        }
    }

   
    [RelayCommand(CanExecute =nameof(CanExecuteAction))]
    async Task TransferEquipment(Guid IDTarget)
    {
        IsPerformingAction = true;
        if (IDTarget.Equals(OriginalLocationID))
            return;

        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
            return;

        try
        {
            AssignEquipmentToNewLocation(c, IDTarget);

            await DeleteOldEquipmentLocation(c);

        }
        catch(Exception ex)
        {
            await DisplayNotification(ex);
            return;
        }

        await PerformContextSave(c);
        
        if (_operationSucceeded)
        {
            await Shell.Current.GoToAsync("..");
        }
        IsPerformingAction = false;
    }
}
