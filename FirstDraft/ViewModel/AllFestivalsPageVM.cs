using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
//using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using FirstDraft.Model;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FirstDraft.ViewModel;


public partial class AllFestivalsPageVM : ObservableObject, INotifyPropertyChanged
{
    Task _saveNewFestival = null;

    [ObservableProperty]
    ObservableCollection<Festival> _activeFestivals;

    [ObservableProperty]
    string _newFestivalName;
    
    [ObservableProperty]
    string _location;

    [ObservableProperty]
    DateOnly _newStartDate = DateOnly.FromDateTime(DateTime.Now);

    [ObservableProperty]
    DateOnly _newEndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));


    public AllFestivalsPageVM() 
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);
        context.Database.EnsureCreated();
        ActiveFestivals = new(context.Festivals
            .Include(f => f.FestivalsExtWorkersRelations)
            .ThenInclude(few => few.ExternalWorker)  
            .Include(f => f.Construction)
            .Include(f => f.Deconstruction)
            .AsNoTracking());
        context.Dispose();
    }

    [RelayCommand]
    static Task NavToFestivalSinglePage(Festival f)=>
        Shell.Current.GoToAsync(nameof(SingleFestivalPage), new Dictionary<string, object>
        {
            ["Festival"] = f
        });

    bool CanExecute()
    {
        
        if (_saveNewFestival == null || _saveNewFestival.Status != TaskStatus.Running)
            return true;
        return false;
    }

    [RelayCommand(CanExecute =nameof(CanExecute))]
    async Task AddNewFestival()
    {
        Festival f = new() { Name = NewFestivalName , StartDate = NewStartDate , EndDate = NewEndDate, Location = Location, FestivalsExtWorkersRelations = new() };

        MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        c.Festivals.Update(f);
        _saveNewFestival = c.SaveChangesAsync();
        await _saveNewFestival;
        //c.SaveChanges();

        _activeFestivals.Add(f);
        
    }      
    
}
