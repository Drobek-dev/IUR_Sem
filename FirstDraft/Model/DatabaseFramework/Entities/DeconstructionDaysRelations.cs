using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class DeconstructionDaysRelations
{
    public Guid IDDay { get; init; }
    public BuildingSiteDay BuildingSiteDay { get; init; }
    public Guid IDDeconstruction { get; init; }
    public Deconstruction Deconstruction { get; init; }
}
