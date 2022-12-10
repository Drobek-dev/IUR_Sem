using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

[Index(nameof(FirstName))]
[Index(nameof(LastName))]

public sealed class ExternalWorker
{
    public Guid ID { get; private init; }
    public required string Function { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }

    internal ObservableCollection<FestivalsExtWorkersRelations> FestivalsExtWorkersRelations { get; } = new();

}
