using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using Microsoft.EntityFrameworkCore;
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

    [ObservableProperty]
    string _title;

    public async Task Refresh()
    {
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        Warehouse = await c.Warehouses
            .Include(w=> w.LocalEquipmentRelations)
            .Where(W=> W.ID.Equals(Warehouse.ID))
            .FirstOrDefaultAsync();

        Title = Warehouse is not null ? $"Sklad {Warehouse.Name}" : Title;
    }

    [RelayCommand]
    async Task SaveChanges()
    {
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        c.Warehouses.Update(_warehouse);
        await PerformContextSave(c);

        if (_operationSucceeded)
            await Refresh();

    }

    [RelayCommand]
    async Task Delete()
    {

        if (!await YesNoAlert($"Proceed to delete {Warehouse.Name} festival?"))
            return;

        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        foreach (var ler in Warehouse.LocalEquipmentRelations)
        {
            Equipment e = await c.Equipment.FindAsync(ler.IDEquipment);
            c.Equipment.Remove(e);
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
                ["Location"] = LocationTypes.warehouse,
                ["LocationName"] = Warehouse.Name
            });
    }

}
