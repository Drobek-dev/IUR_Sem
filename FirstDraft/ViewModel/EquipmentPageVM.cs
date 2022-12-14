
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

    void FillLocalEquipment(object storage)
    {
        LocalEquipment = new();
        if (Location.Equals(nameof(Festival)))
            {
                Festival f = (Festival)storage;
                foreach (var localRelation in f.LocalEquipmentRelation)
                {
                    LocalEquipment.Add(localRelation.Equipment);
                }
            }

    }
    public ICommand RefreshEquipment => new Command(
      execute: () =>
      {
          using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
          LocalEquipment = new();

          if (Location.Equals(nameof(Festival)))
          {
            var Festival = c.Festivals.Include(f => f.LocalEquipmentRelation).ThenInclude(eif => eif.Equipment).Where(f=> f.ID.Equals(IDLocation)).First();
              
              foreach (var localRelation in Festival.LocalEquipmentRelation)
              {
                  LocalEquipment.Add(localRelation.Equipment);
              }

          }

      },
      canExecute: () => // in this case it is unnecessary as simultaneous adding of festivals does not produce any errors
      {

          return true;
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
