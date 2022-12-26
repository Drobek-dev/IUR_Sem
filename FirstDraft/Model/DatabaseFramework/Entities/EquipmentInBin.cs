using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Model.DatabaseFramework.Entities;

[PrimaryKey(nameof(IDEquipment))]
public class EquipmentInBin
{
    [ForeignKey(nameof(Equipment))]
    public Guid IDEquipment { get; init; }
    public Equipment Equipment { get; init; }
}
