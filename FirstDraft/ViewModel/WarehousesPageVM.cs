using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstDraft.ViewModel;

public partial class WarehousesPageVM : ObservableObject
{
    Task _saveNewWarehouse = null;
    [ObservableProperty]
    string _newWarehouseName;

    [ObservableProperty]
    string _newWarehouseAddress;

    [ObservableProperty]
    ObservableCollection<Warehouse> _activeWarehouses;

    [ObservableProperty]
    ObservableCollection<Warehouse> _searchResults;

    public WarehousesPageVM()
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);

        ActiveWarehouses = new(context.Warehouses.Include(w=>w.LocalEquipmentRelations));
        context.Dispose();

        SearchResults = ActiveWarehouses;
    }

    [RelayCommand]
    async Task NavToSingleWarehousePage(Warehouse w)
    {
        await Shell.Current.GoToAsync(nameof(SingleWarehousePage), new Dictionary<string, object>
        {
            ["Warehouse"] = w
        });
    }

    public ICommand AddNewWarehouse => new Command(
        execute: async () =>
        {
            Warehouse w = new() { Name = NewWarehouseName, Address = NewWarehouseAddress};

            MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
            c.Warehouses.Update(w);
            _saveNewWarehouse = c.SaveChangesAsync();
            await _saveNewWarehouse;

            _activeWarehouses.Add(w);
        },
        canExecute: () => // in this case it is unnecessary as simultaneous adding of festivals does not produce any errors
        {

            if (_saveNewWarehouse is not null && _saveNewWarehouse.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        });

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveWarehouses;
        }
        else
        {

            SearchResults = new(ActiveWarehouses.Where(w => w.Name.Equals(query)).ToList());
        }
    });
}
