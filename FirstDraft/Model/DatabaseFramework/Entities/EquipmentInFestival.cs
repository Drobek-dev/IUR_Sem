using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

[PrimaryKey(nameof(IDFestival), nameof(IDEquipment))]
public class EquipmentInFestival
{
    //[ForeignKey(nameof(Equipment))]
    public Guid IDEquipment { get; set; }
    public Equipment Equipment { get; set; }    

    //[Column(nameof(Festival))]
    public Guid IDFestival { get; set; }
    public Festival Festival { get; set; }
}
