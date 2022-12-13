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
    [Column("IDEquipment")]
    public Guid IDEquipment { get; set; }
    [Column("IDFestival")]
    public Guid IDFestival { get; set; }
  

}
