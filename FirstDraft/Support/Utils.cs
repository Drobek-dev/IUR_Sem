using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirstDraft.Support;
public enum TypeOfDatabase
{
    LocalSQLite,
    CloudPostgreSQL
}

public static partial class GlobalValues 
{
    public static readonly string warehouse = "warehouse";
    public static readonly string festival = "festival";
    public static readonly string transport = "transport";
    public static readonly string bin = "bin";

    [GeneratedRegex("^\\+?[1-9][0-9]{7,14}$", RegexOptions.Compiled)]
    public static partial Regex MyPhoneRegex();
    [GeneratedRegex(@"^\S+@\S+\.[a-z][a-z]+$", RegexOptions.Compiled)]
    public static partial Regex MyEmailRegex();

}



