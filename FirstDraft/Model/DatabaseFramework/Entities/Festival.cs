using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Festival:INotifyPropertyChanged  
{

    
    public Guid ID { get; private init; }


    private string _name;

    public required string Name { get => _name;
        init 
        {
            _name = value;
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private DateOnly _startDate;
    public required DateOnly StartDate 
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));   
        }
    
    }

    private DateOnly _endDate;
    public required DateOnly EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }
    public required string Location {  get; init; }

    [ForeignKey(nameof(Construction))]
    public Guid IDConstruction { get; init; }

    public Construction Construction { get; init; }

    [ForeignKey(nameof(Deconstruction))]
    public Guid IDDeconstruction { get; init; }
    public Deconstruction Deconstruction { get; init; }


    public ObservableCollection<FestivalsExtWorkersRelations> FestivalsExtWorkersRelations { get; init; }
    public ObservableCollection<EquipmentInFestival> LocalEquipmentRelations { get; init; }


   
    
}

