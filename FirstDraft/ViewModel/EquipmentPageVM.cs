
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FirstDraft.ViewModel;
[QueryProperty(nameof(IDLocation),nameof(IDLocation))]
[QueryProperty(nameof(Location),nameof(Location))]
[QueryProperty(nameof(LocationName),nameof(LocationName))]
public partial class EquipmentPageVM : BaseVM, INotifyPropertyChanged
{
    [ObservableProperty]
    ObservableCollection<Equipment> _equipmentToTransfer = new();

    [ObservableProperty]
    ObservableCollection<Equipment> _selectedItems;

    [ObservableProperty]
    Equipment _draggedEquipment;

    [ObservableProperty]
    string _locationName;

    [ObservableProperty]
    Guid _iDLocation;

    [ObservableProperty]
    string _location;

    [ObservableProperty]
    ObservableCollection<Equipment> _localEquipment;

    [ObservableProperty]
    string _selection;
 
    public void RefreshEquipmentMethod()
    {
        LocalEquipment = new();
        EquipmentToTransfer = new();
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        if (Location.Equals(LocationTypes.bin))
        {
            var BinEquipment = c.Bin.Include(b => b.Equipment).ToList();
            foreach (var localRelation in BinEquipment)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }
        }

        else if (Location.Equals(LocationTypes.festival))
        {
            var Festival = c.Festivals.Include(f => f.LocalEquipmentRelation).ThenInclude(eif => eif.Equipment).Where(f => f.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Festival.LocalEquipmentRelation)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }

        }
        else if (Location.Equals(LocationTypes.warehouse))
        {
            var Warehouse = c.Warehouses.Include(w => w.LocalEquipmentRelations).ThenInclude(ler => ler.Equipment).Where(w => w.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Warehouse.LocalEquipmentRelations)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }
        }
        else if (Location.Equals(LocationTypes.transport))
        {
            var Transport = c.Transports.Include(t => t.LocalEquipmentRelations).ThenInclude(ler => ler.Equipment).Where(t => t.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Transport.LocalEquipmentRelations)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }
        }
    }

    [RelayCommand]
    void RefreshEquipment()
      {
         RefreshEquipmentMethod();  
      }

    // CollectionView Multiple selection Begin ----------------------------------------------
    [RelayCommand]
    void TransferToPickedEqp()
    {
        if (SelectedItems is null)
            return;
        foreach(var e in SelectedItems)
        {
            LocalEquipment.Remove(e);
            EquipmentToTransfer.Add(e);
        }
    }
    // CollectionView Multiple selection End ----------------------------------------------

    // Drag and Drop Begin -------------------------------------------------------------
    [RelayCommand]
    void DraqEquipment(Equipment e)
    {
        _draggedEquipment = e; // Unknown Error occurs here! Concurrency issue?

    }

    [RelayCommand]
    void ClearEquipmentToTransfer()
    {
        EquipmentToTransfer = new();
        RefreshEquipmentMethod();
    }

    [RelayCommand]
    void DropEquipment()
    {
        if (_draggedEquipment is null)
            return;
        LocalEquipment.Remove(_draggedEquipment);
        EquipmentToTransfer.Add(_draggedEquipment);
     
    }
    //Drag and Drop End ----------------------------------------------------------------

    [RelayCommand]
    async Task NavToTransferPage()
    {
        if(EquipmentToTransfer?.Count==0 || Selection is null)
        {
            await DisplayNotification($"Destination must be selected.{Environment.NewLine}" +
                $"Number of selected items must be greater than Zero.");
            return;
        }
        string originalLocation = string.IsNullOrWhiteSpace(Location) ? LocationTypes.bin : Location;
        await Shell.Current.GoToAsync(nameof(TransferPage),new Dictionary<string, object>
        {
            ["OriginalLocation"] = originalLocation,
            ["OriginalLocationID"] = IDLocation,
            ["NewLocation"] = Selection,
            ["Equipment"] = EquipmentToTransfer,
            ["OriginalLocationName"] =LocationName
        });
    }

    [RelayCommand]
    async Task NavToAddEquipmentPage()
    {
        string equipmentLocation = string.IsNullOrWhiteSpace(Location) ? LocationTypes.bin : Location;
        await Shell.Current.GoToAsync(nameof(AddEquipmentPage), new Dictionary<string, object>
        {
            ["Location"] = equipmentLocation,
            ["LocationID"] = IDLocation,
            ["LocationName"] = LocationName
        });
    }

    [RelayCommand]
    async Task DeleteSelectedEquipment()
    {
        if (await BaseVM.YesNoAlert($"Opravdu chcete vysypat z koše vybrané vybavení? {Environment.NewLine}" +
            $"{EquipmentToTransfer.Count} položek vybavení bude nenávratně ztraceno."))
        {
            using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
            foreach(var e in EquipmentToTransfer)
            {
                c.Bin.Remove(await c.Bin.FindAsync(e.ID));
                c.Equipment.Remove(await c.Equipment.FindAsync(e.ID));

            }

            await PerformContextSave(c);

            if (_operationSucceeded)
            {
                EquipmentToTransfer = new();
                RefreshEquipmentMethod();
            }
        }
    }

    [RelayCommand]
    async Task DeleteAllEquipment()
    {
        if (await BaseVM.YesNoAlert($"Opravdu chcete vysypat koš? {Environment.NewLine}" +
            $"{LocalEquipment.Count} položek vybavení bude nenávratně ztraceno."))
            {
                using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
                foreach (var e in LocalEquipment)
                {
                    c.Bin.Remove(await c.Bin.FindAsync(e.ID));
                    c.Equipment.Remove(await c.Equipment.FindAsync(e.ID));

                }

                await PerformContextSave(c);

                if (_operationSucceeded)
                {
                    EquipmentToTransfer = new();
                    RefreshEquipmentMethod();
                }
            }
    }
}
