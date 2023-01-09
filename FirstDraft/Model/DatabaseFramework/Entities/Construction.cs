using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Construction : INotifyPropertyChanged
{

    public Guid ID { get; init; }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }

    }

    private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }

    public ObservableCollection<ConstructionDaysRelations> ConstructionDaysRelations { get; init; }

 


}
