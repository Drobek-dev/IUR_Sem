
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
public partial class EquipmentPageVM : ObservableObject, INotifyPropertyChanged
{
    [ObservableProperty]
    ObservableCollection<Equipment> _equipmentToTransfer = new();

    [ObservableProperty]
    Equipment _draggedEquipment;

    [ObservableProperty]
    Guid _iDLocation;

    [ObservableProperty]
    string _location;

    [ObservableProperty]
    ObservableCollection<Equipment> _localEquipment;

    
    string _selection;
    public string Selection
    {
        get
        {
            return _selection;
        }
        set
        {
            _selection = value;
            OnPropertyChanged(nameof(Selection));
        }
    }


    public ICommand RefreshEquipment => new Command(
      execute: () =>
      {
          LocalEquipment = new();
          using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

          if(string.IsNullOrWhiteSpace(Location))
          {
              var BinEquipment = c.Bin.Include(b => b.Equipment).ToList();
              foreach (var localRelation in BinEquipment)
              {
                  LocalEquipment.Add(localRelation.Equipment);
              }
          }

          else if (Location.Equals(LocationTypes.festival))
          {
            var Festival = c.Festivals.Include(f => f.LocalEquipmentRelation).ThenInclude(eif => eif.Equipment).Where(f=> f.ID.Equals(IDLocation)).First();
              
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
          

      });

    public ICommand DragEquipment => new Command<Equipment>((Equipment e) => 
    {
        _draggedEquipment = e;
    });

    public ICommand DropEquipment => new Command(() => 
    {
        if (!EquipmentToTransfer.Contains(DraggedEquipment))
        {
        EquipmentToTransfer.Add(DraggedEquipment);
        _draggedEquipment = null;

        }
    });

    [RelayCommand]
    async Task NavToTransferPage()
    {
        string originalLocation = string.IsNullOrWhiteSpace(Location) ? LocationTypes.bin : Location;
        await Shell.Current.GoToAsync(nameof(TransferPage),new Dictionary<string, object>
        {
            ["OriginalLocation"] = originalLocation,
            ["OriginalLocationID"] = IDLocation,
            ["NewLocation"] = Selection,
            ["Equipment"] = EquipmentToTransfer
        });
    }

}
