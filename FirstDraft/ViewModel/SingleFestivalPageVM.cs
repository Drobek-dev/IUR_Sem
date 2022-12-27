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
public partial class SingleFestivalPageVM : BaseVM
{

    [ObservableProperty]
    Festival _festival;

    SingleFestivalPage _viewPage;


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

        await PerformContextSave(c);
        
    }

    [RelayCommand]
    async Task DeleteFestival()
    {
        if (await YesNoAlert($"Proceed to delete {Festival.Name} festival?"))
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

            await PerformContextSave(c);
            await Shell.Current.GoToAsync("..");
        }
    }

    public ICommand NavToEquipment => new Command(async() =>
    {
        await Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
            new Dictionary<string, object>
            {
                ["IDLocation"] = Festival.ID,
                ["Location"] = LocationTypes.festival
            });
    });



}
