﻿
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

public partial class TransportsPageVM : ObservableObject
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

    Task _saveNew = null;
    public TransportsPageVM()
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);

        ActiveTransports = new(context.Transports.Include(t=> t.LocalEquipmentRelations).OrderByDescending(t=> t.ID));
       
        SearchResults = ActiveTransports;
    }

    [RelayCommand]
    async Task NavToSingleTransportPage(Transport t)
    {
        await Shell.Current.GoToAsync(nameof(SingleTransportPage), new Dictionary<string, object>
        {
            ["Transport"] = t
        });
    }

    public ICommand AddNew => new Command(
        execute: async () =>
        {
            Transport t = new() { TransportName = NewTransportName, DriverFullName = NewTransportDriverFullName, StartingPosition = NewTransportStartingPosition
                                   , Destination = NewTransportDestination, DriverPhone = NewTransportDriverPhone, EstimatedArrivalTime =DateTime.SpecifyKind(NewTransportEstimatedArrivalTime, DateTimeKind.Utc)};

            MyDBContext c = new(TypeOfDatabase.CloudPostgreSQL);
            c.Transports.Update(t);
            _saveNew = c.SaveChangesAsync();
            await _saveNew;

            _activeTransports.Add(t);
        },
        canExecute: () => // in this case it is unnecessary as simultaneous adding of festivals does not produce any errors
        {

            if (_saveNew is not null && _saveNew.Status == TaskStatus.Running)
            {
                return false;
            }
            return true;
        });

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