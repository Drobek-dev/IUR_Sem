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

    AllFestivalsPage _viewPage;
    public AllFestivalsPageVM(AllFestivalsPage viewPage)
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);

        ActiveFestivals = new(context.Festivals.OrderByDescending(f=>f.ID)
            .Include(f => f.FestivalsExtWorkersRelations)
            .ThenInclude(few => few.ExternalWorker)
            .Include(f => f.Construction)
            .Include(f => f.Deconstruction)
            .AsNoTracking());

        SearchResults = ActiveFestivals;
        _viewPage = viewPage;
      
    }

    [RelayCommand]
    async Task NavToFestivalSinglePage(Festival f)
    {
        bool isPageNavConfirmed = await _viewPage.YesNoAlert($"Move to {f.Name}?");

        if (isPageNavConfirmed)
        {
            await Shell.Current.GoToAsync(nameof(SingleFestivalPage), new Dictionary<string, object>
            {
                ["Festival"] = f
            });
        }

    }


    public ICommand AddNewFestival => new Command(
        execute: async () =>
    {
        Festival f = new() { Name = NewFestivalName, StartDate = NewStartDate, EndDate = NewEndDate, Location = Location, FestivalsExtWorkersRelations = new() };

        using MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
        c.Festivals.Update(f);
        _saveNewFestival = c.SaveChangesAsync();

        try
        {
            await _saveNewFestival;
            _activeFestivals.Add(f);

        }
        catch (Exception ex) 
        {
            await _viewPage.DisplayNotification(
                $"Thrown Exception: {ex.Message} {Environment.NewLine}{Environment.NewLine}" +
                $"Task Exception: {_saveNewFestival.Exception?.Message}");
        }

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
