using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
//using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using FirstDraft.Model;
using FirstDraft.Model.DatabaseFramework;
using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.Support;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FirstDraft.ViewModel;


public partial class AllFestivalsPageVM : ObservableObject
{
    static Task _navigationTask = null;

    [ObservableProperty]
    ObservableCollection<Festival> _activeFestivals;

    
    public AllFestivalsPageVM() 
    {
        MyDBContext context = new(TypeOfDatabase.CloudPostgreSQL);
        context.Database.EnsureCreated();
        ActiveFestivals = new(context.Festivals);
        context.Dispose();
    }
    static bool CanExecuteNavigate()
    {
        return _navigationTask is null;
    }
    [RelayCommand(CanExecute= nameof(CanExecuteNavigate))]
    static async void Navigate()
    {
        
        _navigationTask =  Shell.Current.GoToAsync("SingleFestivalPage");
        await _navigationTask;
        _navigationTask = null;
    }
}
