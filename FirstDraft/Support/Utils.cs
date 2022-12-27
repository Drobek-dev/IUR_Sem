using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support;
public enum TypeOfDatabase
{
    LocalSQLite,
    CloudPostgreSQL
}

public static class LocationTypes 
{
    public static readonly string warehouse = "warehouse";
    public static readonly string festival = "festival";
    public static readonly string transport = "trnasport";
    public static readonly string bin = "bin";

}

