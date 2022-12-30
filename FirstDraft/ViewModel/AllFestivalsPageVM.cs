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


public partial class AllFestivalsPageVM : BaseVM, INotifyPropertyChanged
{

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


    public AllFestivalsPageVM()
    {
        Init();
    }

    protected override void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        base.Connectivity_ConnectivityChanged(sender, e);    
        Init();
    }

    private void Init()
    {
        if (!InternetAvailable)
            return;

        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        ActiveFestivals = new(c.Festivals.OrderByDescending(f => f.ID)
            .Include(f => f.FestivalsExtWorkersRelations)
            .ThenInclude(few => few.ExternalWorker)
            .Include(f => f.Construction)
            .Include(f => f.Deconstruction)
            .AsNoTracking());

        SearchResults = ActiveFestivals;
    }

    [RelayCommand]
    async Task NavToAddFestivalPage()
    {
        await Shell.Current.GoToAsync(nameof(AddFestivalPage));
    }

    [RelayCommand]
    async Task NavToFestivalSinglePage(Festival f)
    {
       
            await Shell.Current.GoToAsync(nameof(SingleFestivalPage), new Dictionary<string, object>
            {
                ["Festival"] = f
            });
        

    }

    [RelayCommand(CanExecute =nameof(CanExecute))]
    async Task AddNew()
    {
        Festival f = new() { Name = NewFestivalName, StartDate = NewStartDate, EndDate = NewEndDate, Location = Location, FestivalsExtWorkersRelations = new() };

        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        c.Festivals.Update(f);

        await PerformContextSave(c);
        if (_operationSucceeded)
        {
            ActiveFestivals.Add(f);
            await Shell.Current.GoToAsync("..");
        }

    }
        bool CanExecute ()
        {
  
            if(_task is not null && _task.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        }
        

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
