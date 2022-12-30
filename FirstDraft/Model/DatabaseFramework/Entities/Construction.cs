using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

public class Construction
{

    public Guid ID { get; init; }

    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init;}

    public ObservableCollection<ConstructionDaysRelations> ConstructionDaysRelations { get; init; }

 


}
