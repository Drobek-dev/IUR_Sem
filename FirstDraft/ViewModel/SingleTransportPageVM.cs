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
            .Include(w => w.LocalEquipmentRelationss)
            .Where(W => W.ID.Equals(Transport.ID)).FirstAsync();

        Title = Transport is not null ? $"Transport {Transport.TransportName}" : Title;

    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task SaveChanges()
    {
        IsPerformingAction = true;
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        c.Transports.Update(_transport);

        await PerformContextSave(c);

        if (_operationSucceeded)
        {
            await Refresh();
            await DisplayNotification("Změny uloženy.");
        }
        IsPerformingAction=false;
    }

    [RelayCommand(CanExecute =nameof(CanExecuteAction))]
    async Task Delete()
    {
        if (!await YesNoAlert($"Smazat transport {Transport.TransportName}?{Environment.NewLine}" +
            $"{Transport.LocalEquipmentRelationss?.Count ?? 0} vybavení bude odstraněno."))
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

        foreach (var ler in Transport.LocalEquipmentRelationss)
        {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }
        c.Transports.Remove(Transport);

        await PerformContextSave(c);

        if(_operationSucceeded)
            await Shell.Current.GoToAsync("..");
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToEquipment()
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
                new Dictionary<string, object>
                {
                    ["IDLocation"] = Transport.ID,
                    ["Location"] = GlobalValues.transport,
                    ["LocationName"] = Transport.TransportName
                })
            );
    }
}
