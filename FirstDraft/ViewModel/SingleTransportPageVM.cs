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


namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Transport), nameof(Transport))]
public partial class SingleTransportPageVM : BaseVM
{
    [ObservableProperty]
    Transport _transport;

    [ObservableProperty]
    string _title;

    public async Task Refresh()
    {
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        Transport = await c.Transports
            .Include(w => w.LocalEquipmentRelations)
            .Where(W => W.ID.Equals(Transport.ID)).FirstOrDefaultAsync();
        Title = Transport is not null ? $"Transport {Transport.TransportName}" : Title;

    }

    [RelayCommand]
    async Task SaveChanges()
    {
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        c.Transports.Update(_transport);

        await PerformContextSave(c);

        if (_operationSucceeded)
        {
            await Refresh();
            await DisplayNotification("Změny uloženy.");
        }
    }

    [RelayCommand]
    async Task Delete()
    {
        if (!await YesNoAlert($"Smazat transport {Transport.TransportName}?{Environment.NewLine}" +
            $"{Transport.LocalEquipmentRelations?.Count ?? 0} vybavení bude odstraněno."))
            return;
       
            using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        foreach (var ler in Transport.LocalEquipmentRelations)
        {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }
        c.Transports.Remove(Transport);

        await PerformContextSave(c);

        if(_operationSucceeded)
            await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task NavToEquipment()
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Transport.ID,
                ["Location"] = LocationTypes.transport,
                ["LocationName"] = Transport.TransportName
            });
    }
}
