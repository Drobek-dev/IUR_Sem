using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

[PrimaryKey(nameof(IDTransport), nameof(IDEquipment))]
public class EquipmentInTransport
{
    public Guid IDEquipment { get; set; }
    public Equipment Equipment { get; set; }

    //[Column(nameof(Festival))]
    public Guid IDTransport { get; set; }
    public Transport Transport { get; set; }
}
