
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
    string _newTransportName;

    [ObservableProperty]
    string _newTransportDriverFullName;

    [ObservableProperty]
    string _newTransportDriverPhone;

    [ObservableProperty]
    string _newTransportStartingPosition;

    [ObservableProperty]
    string _newTransportDestination;

    [ObservableProperty]
    DateTime _newTransportEstimatedArrivalTime = DateTime.MinValue;

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

        ActiveTransports = new(c.Transports.Include(t=> t.LocalEquipmentRelations).OrderByDescending(t=> t.ID));
        SearchResults = ActiveTransports;
    }

    protected override void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        base.Connectivity_ConnectivityChanged(sender, e);
        Init();
    }


    [RelayCommand]
    async Task NavToAddTransportPage()
    {
        await Shell.Current.GoToAsync(nameof(AddTransportPage));
    }

    [RelayCommand]
    async Task NavToSingleTransportPage(Transport t)
    {
        await Shell.Current.GoToAsync(nameof(SingleTransportPage), new Dictionary<string, object>
        {
            ["Transport"] = t
        });
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAdd))]
    async Task AddNew ()
        {
            using MyDBContext c = GetMyDBContextInstance();

            if (c is null)
                return;

            Transport t = new() { TransportName = NewTransportName, DriverFullName = NewTransportDriverFullName, StartingPosition = NewTransportStartingPosition
                                   , Destination = NewTransportDestination, DriverPhone = NewTransportDriverPhone, EstimatedArrivalTime =DateTime.SpecifyKind(NewTransportEstimatedArrivalTime, DateTimeKind.Utc)};

            c.Transports.Update(t);

            await PerformContextSave(c);

            if (_operationSucceeded)
            {
                _activeTransports.Add(t);
                await Shell.Current.GoToAsync("..");
            }

           
        }

        bool CanExecuteAdd()
        {

            if (_task is not null && _task.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        }

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResults = ActiveTransports;
        }
        else
        {

            SearchResults = new(ActiveTransports.Where(t => t.TransportName.Equals(query)).ToList());
        }
    });
}
