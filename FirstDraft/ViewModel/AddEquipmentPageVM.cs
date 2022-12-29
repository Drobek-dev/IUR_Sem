using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FirstDraft.ViewModel;


[QueryProperty(nameof(Location), nameof(Location))]
[QueryProperty(nameof(LocationID), nameof(LocationID))]
[QueryProperty(nameof(LocationName), nameof(LocationName))]
public partial class AddEquipmentPageVM : BaseVM
{
    [ObservableProperty]
    string _location;

    [ObservableProperty]
    string _locationName;

    [ObservableProperty]
    Guid _locationID;

    [ObservableProperty]
    string _newEquipmentName;

    [ObservableProperty]
    int _newEquipmentQuantity;

    [ObservableProperty]
    DateOnly _newDateOfPurchase = DateOnly.FromDateTime(DateTime.Now);

    [RelayCommand]
    async Task AddNewEquipment()
    {
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        Equipment e = new() { Name = _newEquipmentName, Location = Location, 
            Quantity = NewEquipmentQuantity, DayOfPurchase = NewDateOfPurchase };
        
        c.Equipment.Add(e);

        await PerformContextSave(c);

        if (_operationSucceeded)
        {
            if (Location.Equals(LocationTypes.festival))
            {
                EquipmentInFestival eif = new() { IDEquipment = e.ID, IDFestival=LocationID };
                c.EquipmentInFestivals.Add(eif);    

            }
            else if (Location.Equals(LocationTypes.warehouse))
            {
                EquipmentInWarehouse eiw = new() { IDEquipment = e.ID, IDWarehouse = LocationID };
                c.EquipmentInWarehouses.Add(eiw);
            }
            else if (Location.Equals(LocationTypes.transport))
            {
                EquipmentInTransport eit = new() { IDEquipment = e.ID, IDTransport = LocationID };
                c.EquipmentInTransports.Add(eit);
            }
            else if (Location.Equals(LocationTypes.bin))
            {
                EquipmentInBin eib = new() { IDEquipment = e.ID};
                c.Bin.Add(eib);
            }

            await PerformContextSave(c);

            await Shell.Current.GoToAsync("..");
        }

    }


}
