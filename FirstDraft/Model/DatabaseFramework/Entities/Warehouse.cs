using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Warehouse : INotifyPropertyChanged

{
    public Guid ID { get; private init; }

    private string _name;
    public required string Name
    {
        get => _name;
        init
        {
            _name = value;

            OnPropertyChanged(nameof(Name));
        }
    }
    public required string Address {  get; set; }

    public ObservableCollection<EquipmentInWarehouse> LocalEquipmentRelations { get; init; } = new();

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
