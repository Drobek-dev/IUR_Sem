using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

    [ObservableProperty]
    ObservableCollection<Festival> _searchResults;

    [ObservableProperty]
    string dummyString;


    public AllFestivalsPageVM()
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);

        ActiveFestivals = new(context.Festivals
            .Include(f => f.FestivalsExtWorkersRelations)
            .ThenInclude(few => few.ExternalWorker)
            
            .Include(f => f.Construction)
            .Include(f => f.Deconstruction)
            .AsNoTracking());
        context.Dispose();

        SearchResults = ActiveFestivals;
    }


    public ICommand NavToFestivalSinglePage => new Command<Festival>(async (Festival f)=>
    {
        await Shell.Current.GoToAsync(nameof(SingleFestivalPage), new Dictionary<string, object>
        {
            ["Festival"] = f
        });

    });


    public ICommand AddNewFestival => new Command(
        execute: async () =>
    {
        Festival f = new() { Name = NewFestivalName, StartDate = NewStartDate, EndDate = NewEndDate, Location = Location, FestivalsExtWorkersRelations = new() };

        MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        c.Festivals.Update(f);
        _saveNewFestival = c.SaveChangesAsync();
        await _saveNewFestival;

        _activeFestivals.Add(f);
    },
        canExecute: () => // in this case it is unnecessary as simultaneous adding of festivals does not produce any errors
        {
            
            if(_saveNewFestival is not null && _saveNewFestival.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        });
        

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveFestivals;
        }
        else
        {

        SearchResults = new(ActiveFestivals.Where(f => f.Name.Equals(query)).ToList());
        }
    });


}
