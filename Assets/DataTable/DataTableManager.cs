using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static Dictionary<Type, DataTable> tables = new Dictionary<Type, DataTable>();

    static DataTableManager()
    {
        tables.Clear();
        var stringTable = new StringTable();
        tables.Add(typeof(StringTable), stringTable);
        var skillTable = new SkillTable();
        tables.Add(typeof(SkillTable), skillTable);
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
        {
            return null;
        }
        return tables[id] as T;
    }

    public static void LoadAll()
    {
        //tables.Add(, new MyDataTable());
        Debug.Log(tables);
        foreach (var item in tables)
        {
            item.Value.Load();
        }
    }
}
