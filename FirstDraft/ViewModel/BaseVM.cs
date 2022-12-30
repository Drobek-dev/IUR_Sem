
using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Model.DatabaseFramework;

using System;
using System.Collections.Generic;
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

    protected MyDBContext GetMyDBContextInstance()
    {
        if (!_internetAvailable)
        {
            DisplayNotification("Can not connect to databse.");
            return null;
        }
        
        try
        {
            return new MyDBContext(Support.TypeOfDatabase.CloudPostgreSQL);
        }
        catch(Exception ex)
        {
            DisplayNotification(ex);
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


}
