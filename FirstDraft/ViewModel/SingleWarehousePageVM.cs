using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Warehouse), nameof(Warehouse))]
public partial class SingleWarehousePageVM : BaseVM
{
    [ObservableProperty]
    Warehouse _warehouse;

    [RelayCommand]
    async Task SaveChanges()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        c.Warehouses.Update(_warehouse);
        await PerformContextSave(c);

    }

    [RelayCommand]
    async Task Delete()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

        foreach (var ler in Warehouse.LocalEquipmentRelations)
        {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }
        c.Warehouses.Remove(_warehouse);
        await PerformContextSave(c);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task NavToEquipment()
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Warehouse.ID,
                ["Location"] = LocationTypes.warehouse
            });
    }

}
