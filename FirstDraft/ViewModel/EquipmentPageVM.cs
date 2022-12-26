
using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FirstDraft.ViewModel;
[QueryProperty(nameof(IDLocation),nameof(IDLocation))]
[QueryProperty(nameof(Location),nameof(Location))]
public partial class EquipmentPageVM : ObservableObject
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

          else if (Location.Equals(nameof(Festival)))
          {
            var Festival = c.Festivals.Include(f => f.LocalEquipmentRelation).ThenInclude(eif => eif.Equipment).Where(f=> f.ID.Equals(IDLocation)).First();
              
              foreach (var localRelation in Festival.LocalEquipmentRelation)
              {
                  LocalEquipment.Add(localRelation.Equipment);
              }

          }
          else if (Location.Equals(nameof(Warehouse)))
          {
              var Warehouse = c.Warehouses.Include(w => w.LocalEquipmentRelations).ThenInclude(ler => ler.Equipment).Where(w => w.ID.Equals(IDLocation)).First();

              foreach (var localRelation in Warehouse.LocalEquipmentRelations)
              {
                  LocalEquipment.Add(localRelation.Equipment);
              }
          }
          else if (Location.Equals(nameof(Transport)))
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
        EquipmentToTransfer.Add(DraggedEquipment);
        //_draggedEquipment = null;
    });

}
