using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class BuildingSiteDay
{
    public Guid ID { get; init; }
    public int AvailableWorkers { get;init; }
    public DateOnly Date {  get; init; }    
}
