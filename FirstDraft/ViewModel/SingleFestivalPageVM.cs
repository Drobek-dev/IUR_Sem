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

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(Festival), nameof(Festival))]
public partial class SingleFestivalPageVM : ObservableObject
{
    Task _saveChangesTask;

    [ObservableProperty]
    Festival _festival;
    

    [RelayCommand]
    async Task NavigateToExternalWorkers(ObservableCollection<FestivalsExtWorkersRelations> ews)
    {
        await Shell.Current.GoToAsync(nameof(ExternalWorkersPage), new Dictionary<string, object>
        {
            ["ExternalWorkers"] = ews
        });
    }

    bool CanExecute()
    {
        if (_saveChangesTask is not null && _saveChangesTask.Status == TaskStatus.Running)
            return false;
        return true;    
    }
    [RelayCommand]
    async Task SaveChanges()
    {
        MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        
        c.Festivals.Update(_festival);

        await c.SaveChangesAsync(); 
        c.Dispose();
        
    }

    [RelayCommand]
    async Task DeleteFestival()
    {
        MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);

        foreach(var ew in Festival.FestivalsExtWorkersRelations)
        {
            c.ExternalWorkers.Remove(ew.ExternalWorker);
        }
        c.Festivals.Remove(_festival);
        await c.SaveChangesAsync();
    }
    
}
