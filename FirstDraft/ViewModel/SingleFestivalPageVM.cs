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

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Festival), nameof(Festival))]
public partial class SingleFestivalPageVM : ObservableObject
{

    [ObservableProperty]
    Festival _festival;

    [RelayCommand]
    async Task NavigateToExternalWorkers()
    {
        await Shell.Current.GoToAsync(nameof(ExternalWorkersPage), new Dictionary<string, object>
        {
            ["Festival"] = _festival,
            ["ExternalWorkers"] = _festival.FestivalsExtWorkersRelations
        });
    }

    [RelayCommand]
    async Task SaveChanges()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        
        c.Festivals.Update(_festival);

        await c.SaveChangesAsync(); 
        c.Dispose();
        
    }

    [RelayCommand]
    async Task DeleteFestival()
    {
        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

        foreach(var ew in Festival.FestivalsExtWorkersRelations)
        {
            c.ExternalWorkers.Remove(ew.ExternalWorker);
        }
        foreach(var ler in Festival.LocalEquipmentRelation)
        {
            c.Equipment.Remove(await c.Equipment.FindAsync(ler.IDEquipment));
        }

        c.Constructions.Remove(Festival.Construction);
        c.Deconstructions.Remove(Festival.Deconstruction);
        c.Festivals.Remove(Festival);
        await c.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    public ICommand NavToEquipment => new Command(async() =>
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Festival.ID,
                ["Location"] = $"{nameof(Model.DatabaseFramework.Entities.Festival)}"
            });
    });



}
