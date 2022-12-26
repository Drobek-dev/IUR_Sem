using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

[PrimaryKey(nameof(IDWarehouse), nameof(IDEquipment))]
public class EquipmentInWarehouse
{
    public Guid IDWarehouse { get; set; }
    public Warehouse Warehouse { get; set; }

    public Guid IDEquipment { get; set; }
    public Equipment Equipment { get; set; }
}
