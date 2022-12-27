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

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Transport), nameof(Transport))]
public partial class SingleTransportPageVM : BaseVM
{
    [ObservableProperty]
    Transport _transport;


    [RelayCommand]
    async Task SaveChanges()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        
        c.Transports.Update(_transport);

        await PerformContextSave(c);


    }

    [RelayCommand]
    async Task Delete()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

        foreach (var ler in Transport.LocalEquipmentRelations)
        {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }
        c.Transports.Remove(Transport);
        await PerformContextSave(c);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task NavToEquipment()
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Transport.ID,
                ["Location"] = LocationTypes.transport
            });
    }
}
