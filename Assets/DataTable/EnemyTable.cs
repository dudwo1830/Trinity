using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyTable : DataTable
{
    private string path = @"Tables/EnemyTable.csv";

    protected Dictionary<string, EnemyData> dic = new Dictionary<string, EnemyData>();

    public EnemyTable()
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

            var records = csv.GetRecords<EnemyData>();

            dic.Clear();
            foreach (var record in records)
            {
                var arr = record.ActionListText.Split("|");
                foreach (var item in arr)
                {
                    record.useCardIdList.Add(int.Parse(item));
                }
                Debug.Log(record.ToString());
                dic.Add(record.Name, record);
            }
        }

    }

    public EnemyData GetDataByName(string name)
    {
        if (!dic.ContainsKey(name))
        {
            return null;
        }
        return dic[name];
    }

    public EnemyData GetDataById(int id)
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

    public EnemyData GetRandomData()
    {
        return dic.ElementAt(UnityEngine.Random.Range(0, dic.Count)).Value;
    }

    public List<EnemyData> ToList()
    {
        if (dic.Count <= 0)
        {
            return null;
        }

        var list = new List<EnemyData>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }
        return list;
    }
}
