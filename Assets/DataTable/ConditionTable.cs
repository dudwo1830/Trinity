using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class ConditionTable : DataTable
{
    private string path = @"Tables/ConditionTable.csv";

    protected Dictionary<string, ConditionData> dic = new Dictionary<string, ConditionData>();

    public ConditionTable()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, path);
        Load();
    }

    public override void Load()
    {
        string fileText = string.Empty;
        try
        {
            fileText = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error Loading file:{e.Message}");
        }
        var csvStr = new TextAsset(fileText);

        using (TextReader reader = new StringReader(csvStr.text))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.HasHeaderRecord = true;
            var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<ConditionData>();

            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.Name, record);
            }
        }

    }

    public ConditionData GetDataByName(string name)
    {
        if (!dic.ContainsKey(name))
        {
            return null;
        }
        return dic[name];
    }

    public ConditionData GetDataById(int id)
    {
        foreach (var item in dic.Values)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public ConditionData GetRandomData()
    {
        return dic.ElementAt(UnityEngine.Random.Range(0, dic.Count)).Value;
    }

    public List<ConditionData> ToList()
    {
        if (dic.Count <= 0)
        {
            return null;
        }

        var list = new List<ConditionData>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }
        return list;
    }
}
