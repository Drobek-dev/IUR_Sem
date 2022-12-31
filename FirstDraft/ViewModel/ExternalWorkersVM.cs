
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.View;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Festival), nameof(Festival))]
[QueryProperty(nameof(ExternalWorkers), nameof(ExternalWorkers))]

public partial class ExternalWorkersVM : BaseVM
{

    [ObservableProperty] string _function;
    [ObservableProperty] string _firstName;
    [ObservableProperty] string _lastName;
    [ObservableProperty] string _PhoneNumber;
    [ObservableProperty] string _Email;

    [ObservableProperty]
    Festival _festival;

    [ObservableProperty]
    ObservableCollection<FestivalsExtWorkersRelations> _externalWorkers;

    public string AffiliatedFestivalID { get; set; }

    
    [RelayCommand]
    async Task AddWorker()
    {
        ExternalWorker ew = new() { FirstName = FirstName, LastName = LastName, Function = Function, PhoneNumber = PhoneNumber, Email = Email };
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);

        c.ExternalWorkers.Add(ew);
        await PerformContextSave(c);

        FestivalsExtWorkersRelations few = new() { IDFestival = Festival.ID, IDExternalWorker = ew.ID};
        c.FestivalsExtWorkersRelations.Add(few);
        await PerformContextSave(c);

        if(_operationSucceeded)
            ExternalWorkers.Add(few);
    }

    [RelayCommand]
    async Task UpdateWorker(ExternalWorker ew)
    {
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        c.ExternalWorkers.Update(ew);
        await PerformContextSave(c);

    }
    [RelayCommand]
    async Task DeleteWorker(ExternalWorker ew)
    {
        ExternalWorkers.Remove(ExternalWorkers.Where(few => few.IDExternalWorker == ew.ID).First());
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        c.ExternalWorkers.Remove(ew);
       
        await PerformContextSave(c);

        if (!_operationSucceeded)
            await DisplayNotification("Smazání položky selhalo.");

    }
}
