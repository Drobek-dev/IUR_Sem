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
    
    [ObservableProperty]
    string _newWarehouseName = "";

    [ObservableProperty]
    string _newWarehouseAddress = "";

    [ObservableProperty]
    ObservableCollection<Warehouse> _activeWarehouses;

    [ObservableProperty]
    ObservableCollection<Warehouse> _searchResults;

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
        ActiveWarehouses = new(c.Warehouses.Include(w=>w.LocalEquipmentRelationss));
        SearchResults = ActiveWarehouses;
    }

    protected override void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        base.Connectivity_ConnectivityChanged(sender, e);
        Init();
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToAddWarehousePage()
    {
        await
            NavigateTo(
                Shell.Current.GoToAsync(nameof(AddWarehousePage))
            );
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToSingleWarehousePage(Warehouse w)
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync(nameof(SingleWarehousePage), new Dictionary<string, object>
                {
                    ["Warehouse"] = w
                })
            );
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task AddNew ()
        {

        if (! await AreInputsValid(
            standartEntries: new string[] { NewWarehouseAddress, NewWarehouseName })
            )
            return;

        Warehouse w = new() { Name = NewWarehouseName, Address = NewWarehouseAddress };
        await AddNewEntity(w);
        
    }

    [RelayCommand]
    void PerformSearch(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveWarehouses;
        }
        else
        {

            SearchResults = new(ActiveWarehouses.Where(w => w.Name.Contains(query)).ToList());
        }
    }
}
