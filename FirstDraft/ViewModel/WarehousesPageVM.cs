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

public partial class WarehousesPageVM : BaseVM
{
    Task _taskWarehouse = null;
    [ObservableProperty]
    string _newWarehouseName;

    [ObservableProperty]
    string _newWarehouseAddress;

    [ObservableProperty]
    ObservableCollection<Warehouse> _activeWarehouses;

    [ObservableProperty]
    ObservableCollection<Warehouse> _searchResults;

    [RelayCommand]
    async Task NavToAddWarehousePage()
    {
        await Shell.Current.GoToAsync(nameof(AddWarehousePage));
    }


    public WarehousesPageVM()
    {
        Init();
    }

    private void Init()
    {
        if (!InternetAvailable)
            return;
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;
        ActiveWarehouses = new(c.Warehouses.Include(w=>w.LocalEquipmentRelations));
        SearchResults = ActiveWarehouses;
    }

    protected override void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        base.Connectivity_ConnectivityChanged(sender, e);
        Init();
    }


    [RelayCommand]
    async Task NavToSingleWarehousePage(Warehouse w)
    {
        await Shell.Current.GoToAsync(nameof(SingleWarehousePage), new Dictionary<string, object>
        {
            ["Warehouse"] = w
        });
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    async Task AddNew ()
        {
            Warehouse w = new() { Name = NewWarehouseName, Address = NewWarehouseAddress};

            using MyDBContext c = GetMyDBContextInstance();

            if (c is null)
                return;

            c.Warehouses.Update(w);
            await PerformContextSave(c);

            if (_operationSucceeded)
            {
                _activeWarehouses.Add(w);
                await Shell.Current.GoToAsync("..");
            }
        }

    bool CanExecute()
        {

            if (_taskWarehouse is not null && _taskWarehouse.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        }

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
