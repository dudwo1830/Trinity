using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class CardTable : DataTable
{
    private string path = @"Tables/CardTable";

    protected Dictionary<string, CardData> dic = new Dictionary<string, CardData>();

    public CardTable()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, path);
        Load();
    }

    public override void Load()
    {
        string fileText = string.Empty;
        try
        {
            //fileText = File.ReadAllText(filePath);
            var textAsset = Resources.Load<TextAsset>(path);
            fileText = textAsset.text;
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

            var records = csv.GetRecords<CardData>();
            
            dic.Clear();
            foreach (var record in records)
            {
                record.defaultAmount = record.Amount;
                record.level = 0;

                if (record.Conditions != string.Empty)
                {
                    var conditions = record.Conditions.Split("|");
                    var durations = record.ConditionDurations.Split("|");

                    record.conditionInfo = new Dictionary<int, int>();
                    for (int i = 0; i < conditions.Length; i++)
                    {
                        int id = int.TryParse(conditions[i], out id) ? id : 0;
                        int duration = int.TryParse(durations[i], out duration) ? duration : 0;

                        record.conditionInfo.Add(id, duration);
                    }
                }

                dic.Add(record.Name, record);
            }
        }

    }

    public CardData GetDataByName(string name)
    {
        if (!dic.ContainsKey(name))
        {
            return null;
        }
        return dic[name];
    }

    public CardData GetDataById(int id)
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

    public CardData GetRandomData()
    {
        return dic.ElementAt(UnityEngine.Random.Range(0, dic.Count)).Value;
    }

    public List<CardData> ToList()
    {
        if (dic.Count <= 0)
        {
            return null;
        }

        var list = new List<CardData>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }
        return list;
    }

    public void ResetAllCardData()
    {
        foreach (var card in dic)
        {
            card.Value.ResetData();
        }
    }
}