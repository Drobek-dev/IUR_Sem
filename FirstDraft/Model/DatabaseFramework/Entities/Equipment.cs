using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Equipment
{
    public Guid ID { get; init; }
    public required string Name { get; init; }  
    public DateOnly DayOfPurchase { get; init; }
    public int Quantity {  get; init; }
    private string _location;
    public required string Location
    {
        get => _location;
        set
        {
            if (value.Equals("festival") ||
                value.Equals("transport") ||
                value.Equals("bin") ||
                value.Equals("warehouse"))
            {
                _location = value;
            }
            else
                throw new ArgumentException();
        }
    }
    public ObservableCollection<EquipmentInFestival> EquipmentInFestival { get; init; }
    public ObservableCollection<EquipmentInWarehouse> EquipmentInWarehouse { get; init; }

    //public EquipmentInTransport EquipmentInTransport { get; init; }
    public ObservableCollection<EquipmentInTransport> EquipmentInTransport { get;init; }
}
