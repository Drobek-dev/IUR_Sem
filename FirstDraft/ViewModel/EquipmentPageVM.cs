
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
    string _locationName = "";

    [ObservableProperty]
    Guid _iDLocation;

    [ObservableProperty]
    string _location = "";

    [ObservableProperty]
    ObservableCollection<Equipment> _localEquipment;

    [ObservableProperty]
    string _selection;
 
    async public void RefreshEquipmentMethod()
    {
        LocalEquipment = new();
        EquipmentToTransfer = new();
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
            return;

        if (Location.Equals(GlobalValues.bin))
        {
            var BinEquipment = c.Bin.Include(b => b.Equipment).ToList();
            foreach (var localRelation in BinEquipment)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }
        }

        else if (Location.Equals(GlobalValues.festival))
        {
            var Festival = c.Festivals.Include(f => f.LocalEquipmentRelations).ThenInclude(eif => eif.Equipment).Where(f => f.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Festival.LocalEquipmentRelations)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }

        }
        else if (Location.Equals(GlobalValues.warehouse))
        {
            var Warehouse = c.Warehouses.Include(w => w.LocalEquipmentRelationss).ThenInclude(ler => ler.Equipment).Where(w => w.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Warehouse.LocalEquipmentRelationss)
            {
                LocalEquipment.Add(localRelation.Equipment);
            }
        }
        else if (Location.Equals(GlobalValues.transport))
        {
            var Transport = c.Transports.Include(t => t.LocalEquipmentRelationss).ThenInclude(ler => ler.Equipment).Where(t => t.ID.Equals(IDLocation)).First();

            foreach (var localRelation in Transport.LocalEquipmentRelationss)
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

    // CollectionView Multiple selection End ----------------------------------------------

    [RelayCommand]
    void DropEquipment()
    {
        if (_draggedEquipment is null)
            return;
        LocalEquipment.Remove(_draggedEquipment);
        EquipmentToTransfer.Add(_draggedEquipment);
     
    }
    //Drag and Drop End ----------------------------------------------------------------

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToTransferPage()
    {
        if(EquipmentToTransfer?.Count==0 || Selection is null)
        {
            await DisplayNotification($"Destination must be selected.{Environment.NewLine}" +
                $"Number of selected items must be greater than Zero.");
            return;
        }
        string originalLocation = string.IsNullOrWhiteSpace(Location) ? GlobalValues.bin : Location;
        await NavigateTo( Shell.Current.GoToAsync(nameof(TransferPage),new Dictionary<string, object>
        {
            ["OriginalLocation"] = originalLocation,
            ["OriginalLocationID"] = IDLocation,
            ["NewLocation"] = Selection,
            ["Equipment"] = EquipmentToTransfer,
            ["OriginalLocationName"] =LocationName
        }));
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToAddEquipmentPage()
    {
        string equipmentLocation = string.IsNullOrWhiteSpace(Location) ? GlobalValues.bin : Location;
        await NavigateTo( Shell.Current.GoToAsync(nameof(AddEquipmentPage), new Dictionary<string, object>
        {
            ["Location"] = equipmentLocation,
            ["LocationID"] = IDLocation,
            ["LocationName"] = LocationName
        }));
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task DeleteSelectedEquipment()
    {
        if (await BaseVM.YesNoAlert($"Opravdu chcete vysypat z koše vybrané vybavení? {Environment.NewLine}" +
            $"{EquipmentToTransfer.Count} položek vybavení bude nenávratně ztraceno."))
        {
            IsPerformingAction = true;
            using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);

            if(c is null)
            {
                IsPerformingAction= false;
                return;
            }
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
            IsPerformingAction = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task DeleteAllEquipment()
    {
        if (await BaseVM.YesNoAlert($"Opravdu chcete vysypat koš? {Environment.NewLine}" +
            $"{LocalEquipment.Count} položek vybavení bude nenávratně ztraceno."))
        {
            IsPerformingAction = true;
            using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
            if (c is null)
            {
                IsPerformingAction = false;
                return;
            }
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
            IsPerformingAction = false;
        }
    }
}
