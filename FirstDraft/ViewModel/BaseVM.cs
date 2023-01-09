

using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.ViewModel;

public partial class BaseVM : ObservableObject
{
    protected Task _task;
    protected bool _operationSucceeded = true;

    [ObservableProperty]
    bool _internetAvailable;
    [ObservableProperty]
    public bool _isPerformingAction;  


    public BaseVM()
    {
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType == NetworkAccess.Internet)
        {
            InternetAvailable = true;
        }
        else
        {
            InternetAvailable = false;
        }

    }

    ~BaseVM() =>
        Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

    
    protected virtual void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.Internet)
        {
            InternetAvailable = true;
        }
        else
        {
            InternetAvailable = false;
        }

    }

    protected async Task PerformContextSave(MyDBContext c)
    {

        try
        {
            _operationSucceeded = true;
            _task = c.SaveChangesAsync();
            await _task;
        }
        catch (Exception ex)
        {
            _operationSucceeded = false;
            Exception e = _task.Exception;
            string innerExceptions="";
            while(e is not null)
            {
                innerExceptions += $"{e.Message} {Environment.NewLine} {Environment.NewLine}";
                e = e.InnerException;
            }
            await BaseVM.DisplayNotification(
                $"Thrown Exception: {ex.Message} {Environment.NewLine}{Environment.NewLine}" +
                $"Task Exceptions: {innerExceptions}");
        }
    }

    protected async Task NavigateTo(Task navTask)
    {
        IsPerformingAction = true;
        await navTask;
        IsPerformingAction = false;
    }

    protected async Task AddNewEntity(object entity)
    {
        IsPerformingAction = true;
        using MyDBContext c = await GetMyDBContextInstance();

        if (c is null)
        {
            IsPerformingAction = false;
            return;
        }

        if (entity is Festival festival)
            c.Festivals.Add(festival);
        else if (entity is Warehouse warehouse)
            c.Warehouses.Add(warehouse);
        else if(entity is Transport transport)
            c.Transports.Add(transport);

        await PerformContextSave(c);

        if (_operationSucceeded)
        {

            await Shell.Current.GoToAsync("..");
        }
        IsPerformingAction = false;
    }
    protected bool IsNavigating()
    {
        return !IsPerformingAction;
    }
    protected bool CanExecuteAction()
    {

        if (_task is not null && _task.Status == TaskStatus.Running
            || IsPerformingAction || IsPerformingAction)
        {
            return false;
        }
        return true;
    }

    async protected Task<MyDBContext> GetMyDBContextInstance()
    {
        if (!_internetAvailable)
        {
            await DisplayNotification("Can not connect to databse.");
            return null;
        }
        
        try
        {
            return new MyDBContext(Support.TypeOfDatabase.CloudPostgreSQL);
        }
        catch(Exception ex)
        {
            await DisplayNotification(ex);
            return null;
        }
    }
    protected static async Task<bool> YesNoAlert(string question)
    {
        return await Shell.Current.DisplayAlert("Question?", question, "Yes", "No");
    }

    public static async Task DisplayNotification(string message)
    {
        await Shell.Current.DisplayAlert("Alert", message, "Ok");
    }

    public static async Task DisplayNotification(Exception e)
    {
        string message = "";
        while(e is not null)
        {
            message += $" Exception: {e.Message} {Environment.NewLine}";
            e = e.InnerException;
        }
        await Shell.Current.DisplayAlert("Alert", message, "Ok");
    }

    // Input Check Begin --------------------------------------------------

    async protected static Task<bool> AreInputsValid(string[] standartEntries = null, string[] emails = null, 
        string[] phoneNumbers = null, string[] dateOnlyStrings = null,
        string[] dateTimeStrings = null, string[] integerStrings = null)
    {
        bool isAllInputValid = true;
        if(standartEntries is not null && !IsOfTypeStandartEntry(standartEntries))
        {
            isAllInputValid = false;
        }
        if(emails is not null && !IsOfTypeEmail(emails))
        {
            isAllInputValid = false;
        }
        if (phoneNumbers is not null && !IsOfTypePhoneString(phoneNumbers))
        {
            isAllInputValid = false;
        }
        if(dateOnlyStrings is not null && !IsOfTypeDateOnly(dateOnlyStrings))
        {
            isAllInputValid = false;
        }
        if(dateTimeStrings is not null && !IsOfTypeDateTime(dateTimeStrings))
        {
            isAllInputValid = false;
        }
        if (integerStrings is not null && !IsOfTypeInteger(integerStrings))
        {
            isAllInputValid = false;
        }

        if (isAllInputValid)
            return true;
        else
        {
            await DisplayNotification($"Všechna políčka pro vyplnění musí svítit zeleně.{Environment.NewLine}" +
                $"Každé vstupní políčko obsahuje nápovědu. {Environment.NewLine}");
            return false;
        }

    }

    protected static bool IsOfTypeInteger(string[] strings)
    {
        foreach (string s in strings)
        {
            if(!int.TryParse(s, out int _))
            {
                return false;
            }
        }
        return true;
    }
    protected static bool IsOfTypeStandartEntry(string[] strings)
    {
        foreach (string s in strings)
        {
            if (s.Length < 2 || string.IsNullOrWhiteSpace(s))
            {
                return false;
            }
        }
        return true;
    }
    protected static bool IsOfTypeEmail(string[] strings)
    {
        foreach (string s in strings)
        {
            if (!GlobalValues.MyEmailRegex().IsMatch(s))
            {
                return false;
            }
        }
        return true;
    }

    protected static bool IsOfTypePhoneString(string[] strings)
    {
        foreach (string s in strings)
        {
            if (!GlobalValues.MyPhoneRegex().IsMatch(s))
            {
                return false;
            }
        }
        return true;
    }

    protected static bool IsOfTypeDateOnly(string[] strings)
    {
        foreach (string s in strings)
        {
            if (!DateOnly.TryParse(s,out DateOnly _))
            {
                return false;
            }
        }
        return true;
    }


    protected static bool IsOfTypeDateTime(string[] strings)
    {
        foreach (string s in strings)
        {
            if (!DateTime.TryParse(s, out DateTime _))
            {
                return false;
            }
        }
        return true;
    }

    // Input Check End --------------------------------------------------

}
