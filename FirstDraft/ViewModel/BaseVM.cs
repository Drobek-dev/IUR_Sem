
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
            await DisplayNotification(
                $"Thrown Exception: {ex.Message} {Environment.NewLine}{Environment.NewLine}" +
                $"Task Exception: {_task.Exception?.Message}");
        }
    }

    protected async Task<bool> YesNoAlert(string question)
    {
        return await Shell.Current.DisplayAlert("Question?", question, "Yes", "No");
    }

    async public Task DisplayNotification(string message)
    {
        await Shell.Current.DisplayAlert("Alert", message, "Ok");
    }

}
