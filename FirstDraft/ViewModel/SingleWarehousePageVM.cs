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
            .Include(w=> w.LocalEquipmentRelationss)
            .Where(W=> W.ID.Equals(Warehouse.ID))
            .FirstAsync();

        Title = Warehouse is not null ? $"Sklad {Warehouse.Name}" : Title;
    }

    [RelayCommand(CanExecute =nameof(CanExecuteAction))]
    async Task SaveChanges()
    {
        if (! await AreInputsValid(
            standartEntries: new string[] { Warehouse.Name, Warehouse.Address})
            )
            return;

        IsPerformingAction = true;  
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        c.Warehouses.Update(_warehouse);
        await PerformContextSave(c);

        if (_operationSucceeded)
        {
            await Refresh();
            await DisplayNotification("Změny uloženy.");
        }
        IsPerformingAction = false;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task Delete()
    {
        IsPerformingAction = true;
        if (!await YesNoAlert($"Smazat sklad {Warehouse.Name}?{Environment.NewLine}" +
            $"{Warehouse.LocalEquipmentRelationss?.Count ?? 0} vybavení bode odstraněno."))
        {
            IsPerformingAction = false;
            return;
        }

        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        foreach (var ler in Warehouse.LocalEquipmentRelationss)
        {
            Equipment e = await c.Equipment.FindAsync(ler.IDEquipment);
            c.Equipment.Remove(e);
        }
        c.Warehouses.Remove(_warehouse);

        await PerformContextSave(c);

        if(_operationSucceeded)
            await Shell.Current.GoToAsync("..");
        IsPerformingAction = false;
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToEquipment()
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
                    new Dictionary<string, object>
                    {
                        ["IDLocation"] = Warehouse.ID,
                        ["Location"] = GlobalValues.warehouse,
                        ["LocationName"] = Warehouse.Name
                    })
                );
    }

}
