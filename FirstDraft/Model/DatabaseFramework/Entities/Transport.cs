using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Transport : INotifyPropertyChanged
{
    public Guid ID { get; private init; }
    private string _transportName;
    public required string TransportName
    {
        get => _transportName;
        init
        {
            _transportName = value;

            OnPropertyChanged(nameof(TransportName));
        }
    }
    public string DriverFullName { get; set; }
    public required string DriverPhone {  get; set; }
    public required string StartingPosition {  get; set; }
    public required string Destination {  get; set; }

    private DateTime _estimatedArrivalTime;
    public required DateTime EstimatedArrivalTime
    {
        get => _estimatedArrivalTime;
        set
        {
            _estimatedArrivalTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            OnPropertyChanged(nameof(EstimatedArrivalTime));
        }
    }

    public ObservableCollection<EquipmentInTransport> LocalEquipmentRelationss { get; private init; } = new();

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
