
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

    [ObservableProperty] string _function = "";
    [ObservableProperty] string _firstName = "";
    [ObservableProperty] string _lastName = "";
    [ObservableProperty] string _PhoneNumber = "";
    [ObservableProperty] string _Email = "";

    [ObservableProperty]
    Festival _festival;

    [ObservableProperty]
    ObservableCollection<FestivalsExtWorkersRelations> _externalWorkers;

    public string AffiliatedFestivalID { get; set; }

    
    [RelayCommand(CanExecute =nameof(CanExecuteAction))]
    async Task AddWorker()
    {
        IsPerformingAction = true;
        ExternalWorker ew = new() { FirstName = FirstName, LastName = LastName, Function = Function, PhoneNumber = PhoneNumber, Email = Email };
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);

        if(c is null)
        {
            IsPerformingAction = false;
            return;
        }

        c.ExternalWorkers.Add(ew);
        await PerformContextSave(c);

        FestivalsExtWorkersRelations few = new() { IDFestival = Festival.ID, IDExternalWorker = ew.ID};
        c.FestivalsExtWorkersRelations.Add(few);
        await PerformContextSave(c);

        if(_operationSucceeded)
            ExternalWorkers.Add(few);
        IsPerformingAction = false;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task UpdateWorker(ExternalWorker ew)
    {
        IsPerformingAction = true;
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }
        c.ExternalWorkers.Update(ew);
        await PerformContextSave(c);
        IsPerformingAction = false;

    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task DeleteWorker(ExternalWorker ew)
    {
        IsPerformingAction = true;
        ExternalWorkers.Remove(ExternalWorkers.Where(few => few.IDExternalWorker == ew.ID).First());
        using MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        c.ExternalWorkers.Remove(ew);
        if(c is null)
        {
            IsPerformingAction = false;
            return;
        }

        await PerformContextSave(c);

        if (!_operationSucceeded)
            await DisplayNotification("Smazání položky selhalo.");
        IsPerformingAction = false;
    }
}
