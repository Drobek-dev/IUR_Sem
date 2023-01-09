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
    string _newFestivalName="";

    [ObservableProperty]
    string _location="";

    [ObservableProperty]
    DateTime _newStartDate = DateTime.Now;

    [ObservableProperty]
    DateTime _newEndDate = DateTime.Now.AddDays(1);

    [ObservableProperty]
    ObservableCollection<Festival> _searchResults;


    AddFestivalPage _addView;

    public AllFestivalsPageVM(AddFestivalPage addView)
    {
        Init();
        _addView = addView;
    }
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
            .Include(f => f.Construction)         
            .Include(f => f.Deconstruction)           
            .Include(f => f.FestivalsExtWorkersRelations)
            .ThenInclude(few => few.ExternalWorker)
            .AsNoTracking());

        SearchResults = ActiveFestivals;
    }

    [RelayCommand(CanExecute =nameof(IsNavigating))]
    async Task NavToAddFestivalPage()
    {
        await NavigateTo(Shell.Current.GoToAsync(nameof(AddFestivalPage)));
        
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToFestivalSinglePage(Festival f)
    {

        await
            NavigateTo(
                Shell.Current.GoToAsync(nameof(SingleFestivalPage), new Dictionary<string, object>
                {
                    ["Festival"] = f
                })
            );
    }

    [RelayCommand(CanExecute =nameof(CanExecuteAction))]
    async Task AddNew()
    {
        if (! await AreInputsValid(standartEntries:new string[] { NewFestivalName, Location}))
            return;

        Festival f = new() { Name = NewFestivalName, 
            StartDate = DateOnly.FromDateTime(NewStartDate), 
            EndDate = DateOnly.FromDateTime(NewEndDate), 
            Location = Location, 
            FestivalsExtWorkersRelations = new(),
            LocalEquipmentRelations = new(),
            Construction = new(),
            Deconstruction = new()};
        await AddNewEntity(f);
    }


    [RelayCommand]
    void PerformSearch(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveFestivals;
        }
        else
        {
            SearchResults = new(ActiveFestivals.Where(f => f.Name.Contains(query)).ToList());
        }
    }

    



}
