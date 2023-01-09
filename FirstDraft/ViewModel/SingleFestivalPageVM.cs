using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Model.DatabaseFramework;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Support;
using FirstDraft.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Festival), nameof(Festival))]
public partial class SingleFestivalPageVM : BaseVM
{

    [ObservableProperty]
    Festival _festival;

    [ObservableProperty]
    string _title;

    public async Task Refresh()
    {
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
            return;

        Festival = await c.Festivals
            .Include(f=> f.LocalEquipmentRelations)
            .Include(f=> f.FestivalsExtWorkersRelations)
            .ThenInclude(few=> few.ExternalWorker)
            .Include(f=> f.Construction)
            .Include(f=> f.Deconstruction)
            .Where(W => W.ID.Equals(Festival.ID))
            .FirstOrDefaultAsync();

        Title = Festival is not null ? $"Festival {Festival.Name}" : Title;
    }

    [RelayCommand(CanExecute =nameof(IsNavigating))]
    async Task NavigateToExternalWorkers()
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync(nameof(ExternalWorkersPage), new Dictionary<string, object>
                {
                    ["Festival"] = _festival,
                    ["ExternalWorkers"] = _festival.FestivalsExtWorkersRelations
                })
            );
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToEquipment ()
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Festival.ID,
                ["Location"] = GlobalValues.festival,
                ["LocationName"] = Festival.Name
            });
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task SaveChanges()
    {
        if (! await AreInputsValid(
            standartEntries: new string[] { Festival.Name, Festival.Location })
            )
            return;

        IsPerformingAction = true;  
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        c.Festivals.Update(_festival);

        await PerformContextSave(c);

        if (_operationSucceeded)
        {
            await Refresh();
            await DisplayNotification("Změny uloženy.");

        }
        IsPerformingAction = false;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task DeleteFestival()
    {
        if (!await YesNoAlert($"Smazat festival {Festival.Name}?{Environment.NewLine}" +
            $"{Festival.FestivalsExtWorkersRelations?.Count ?? 0} externich pracovníků bude odstraněno.{Environment.NewLine}" +
            $"{Festival.LocalEquipmentRelations?.Count ?? 0} kusů vybavení bude odstraněno."))
            return;
        IsPerformingAction = true;
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        foreach (var ew in Festival.FestivalsExtWorkersRelations)
        {
            c.ExternalWorkers.Remove(ew.ExternalWorker);
        }
        foreach(var ler in Festival.LocalEquipmentRelations)
            {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }

        try
        {
            var q=await c.Constructions.ToListAsync();    
            Construction con = await c.Constructions.FindAsync(Festival.IDConstruction);
            c.Constructions.Remove(con);
            Deconstruction de = await c.Deconstructions.FindAsync(Festival.IDDeconstruction);
            c.Deconstructions.Remove(de);
            c.Festivals.Remove(await c.Festivals.FindAsync(Festival.ID));

            await PerformContextSave(c);

            if(_operationSucceeded)
                await Shell.Current.GoToAsync("..");


        }
        catch(Exception ex)
        {
            await DisplayNotification(ex);
        }

        IsPerformingAction = false;
    }



}
