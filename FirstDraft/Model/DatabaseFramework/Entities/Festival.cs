using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Festival : INotifyPropertyChanged
{


    public Guid ID { get; private init; }

    private string _name;
    public required string Name { get => _name;
        init 
        {
            _name = value;

            OnPropertyChanged(nameof(Name));
        }
    }

    public required DateOnly StartDate { get; init; }
    
   
    public required DateOnly EndDate { get; init; }
    public required string Location {  get; init; }

    public ObservableCollection<FestivalsExtWorkersRelations> FestivalsExtWorkersRelations { get; init; }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

