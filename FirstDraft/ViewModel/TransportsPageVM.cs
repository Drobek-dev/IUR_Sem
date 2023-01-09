
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using FirstDraft.View;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace FirstDraft.ViewModel;

public partial class TransportsPageVM : BaseVM
{
    [ObservableProperty]
    string _newTransportName="";

    [ObservableProperty]
    string _newTransportDriverFullName="";

    [ObservableProperty]
    string _newTransportDriverPhone = "";

    [ObservableProperty]
    string _newTransportStartingPosition = "";

    [ObservableProperty]
    string _newTransportDestination = "";    

    [ObservableProperty]
    DateTime _newTransportEstimatedArrivalDate = DateTime.Now;


    [ObservableProperty]
    ObservableCollection<Transport> _activeTransports;

    [ObservableProperty]
    ObservableCollection<Transport> _searchResults;

    public TransportsPageVM()
    {
        Init();
    
    }

    private void Init()
    {
        if (!InternetAvailable)
            return;
        using MyDBContext c = GetMyDBContextInstance();

        if (c is null)
            return;

        ActiveTransports = new(c.Transports.Include(t=> t.LocalEquipmentRelationss).OrderByDescending(t=> t.ID));
        SearchResults = ActiveTransports;
    }

    protected override void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        base.Connectivity_ConnectivityChanged(sender, e);
        Init();
    }


    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToAddTransportPage()
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync(nameof(AddTransportPage))
            );
    }

    [RelayCommand(CanExecute = nameof(IsNavigating))]
    async Task NavToSingleTransportPage(Transport t)
    {
        await 
            NavigateTo(
                Shell.Current.GoToAsync(nameof(SingleTransportPage), new Dictionary<string, object>
                    {
                        ["Transport"] = t
                    })
            );
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    async Task AddNew ()
    {
        if (! await AreInputsValid(
            standartEntries: new string[] { NewTransportName, NewTransportDriverFullName, NewTransportDestination, NewTransportStartingPosition },
            phoneNumbers: new string[] { NewTransportDriverPhone })
            )
            return;

        DateTime d = DateTime.SpecifyKind(NewTransportEstimatedArrivalDate, DateTimeKind.Utc);
        Transport t = new() { TransportName = NewTransportName, DriverFullName = NewTransportDriverFullName, StartingPosition = NewTransportStartingPosition
                                , Destination = NewTransportDestination, DriverPhone = NewTransportDriverPhone, EstimatedArrivalTime = d};

        await AddNewEntity(t);
    }

    [RelayCommand]
     void PerformSearch(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveTransports;
        }
        else
        {

            SearchResults = new(ActiveTransports.Where(t => t.TransportName.Equals(query)).ToList());
        }
    }
}
