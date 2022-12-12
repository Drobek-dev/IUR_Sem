
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Festival), nameof(Festival))]
[QueryProperty(nameof(ExternalWorkers), nameof(ExternalWorkers))]

public partial class ExternalWorkersVM : ObservableObject
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
        MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);

        c.ExternalWorkers.Add(ew);
        try
        {
        await c.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        FestivalsExtWorkersRelations few = new() { IDFestival = Festival.ID, IDExternalWorker = ew.ID};
        c.FestivalsExtWorkersRelations.Add(few);
        await c.SaveChangesAsync();

        ExternalWorkers.Add(few);
    }

    [RelayCommand]
    static async Task UpdateWorker(ExternalWorker ew)
    {
        MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        c.ExternalWorkers.Update(ew);
        await c.SaveChangesAsync();

    }
    [RelayCommand]
    async Task DeleteWorker(ExternalWorker ew)
    {
        MyDBContext c = new(Support.TypeOfDatabase.CloudPostgreSQL);
        c.ExternalWorkers.Remove(ew);
        await c.SaveChangesAsync();
        ExternalWorkers.Remove(ExternalWorkers.Where(few=> few.IDExternalWorker == ew.ID).First()); // Shady, but dunno how to
       
    }
}
