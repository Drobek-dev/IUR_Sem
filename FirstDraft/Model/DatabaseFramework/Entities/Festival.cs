using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Festival
{


    public Guid ID { get; private init; }
    
    public required string Name { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }

    internal ObservableCollection<FestivalsExtWorkersRelations> FestivalsExtWorkersRelations { get; } = new();


}

