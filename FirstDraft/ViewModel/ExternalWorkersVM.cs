
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Model.DatabaseFramework.Entities;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace FirstDraft.ViewModel;

[QueryProperty(nameof(ExternalWorkers), nameof(ExternalWorkers))]
public partial class ExternalWorkersVM : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<FestivalsExtWorkersRelations> externalWorkers;
}
