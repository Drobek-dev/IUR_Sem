using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;
[PrimaryKey(nameof(IDFestival), nameof(IDExternalWorker))]
public class FestivalsExtWorkersRelations
{

    public Guid IDFestival { get; set; }
    public Festival Festival { get; set; }

    public Guid IDExternalWorker { get; set; }
    public ExternalWorker ExternalWorker { get; set; }
}

