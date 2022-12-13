
using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstDraft.ViewModel;
[QueryProperty(nameof(IDFestival),nameof(IDFestival))]
public partial class EquipmentPageVM : ObservableObject
{
    [ObservableProperty]
    Guid _iDFestival;

    [ObservableProperty]
    ObservableCollection<EquipmentInFestival> festivalEquipment;
    /*async Task getEquipment(MyDBContext c)
    {
        var f =c.Festivals.Include(f => f.EquipmentInFestival).ThenInclude(eif => eif.Equipment).Where(f => f.ID.Equals(IDFestival)).First();

        FestivalEquipment = new(f.EquipmentInFestival);
    }*/

    public ICommand RefreshEquipment => new Command(
      execute:async () =>
      {
          MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

          var e = c.Equipment;
          var f = c.Festivals.ToList();
          var eif = c.EquipmentInFestivals.ToList();
          FestivalEquipment = new(c.EquipmentInFestivals.ToList());
      },
      canExecute: () => // in this case it is unnecessary as simultaneous adding of festivals does not produce any errors
      {

          return true;
      });

}
