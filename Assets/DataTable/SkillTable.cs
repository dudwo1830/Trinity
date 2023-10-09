using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class SkillTable : DataTable
{
    private string path = @"Tables/SkillTable.csv";

    protected Dictionary<string, SkillData> dic = new Dictionary<string, SkillData>();

    public SkillTable()
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

            var records = csv.GetRecords<SkillData>();
            
            dic.Clear();
            foreach (var record in records)
            {
                record.defaultAmount = record.Amount;
                dic.Add(record.Name, record);
            }
        }

    }

    public SkillData GetSkill(string name)
    {
        if (!dic.ContainsKey(name))
        {
            return null;
        }
        return dic[name];
    }

    public SkillData GetSkill(int id)
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

    public SkillData GetRandomSkill()
    {
        return dic.ElementAt(UnityEngine.Random.Range(0, dic.Count)).Value;
    }
    public List<SkillData> ToList()
    {
        if (dic.Count <= 0)
        {
            return null;
        }

        var list = new List<SkillData>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }
        return list;
    }

    public void ResetAllSkill()
    {
        foreach (var skill in dic)
        {
            skill.Value.ResetData();
        }
    }
}
