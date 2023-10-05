using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillTable : DataTable
{
    private string path = "Tables/SkillTable.csv";

    protected Dictionary<string, string> dic = new Dictionary<string, string>();

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
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<SkillData>();

            dic.Clear();
            foreach (var record in records)
            {
                //dic.Add(record.ID, record.STRING);
            }
        }

    }

    public string GetString(string id)
    {
        if (!dic.ContainsKey(id))
        {
            return string.Empty;
        }
        return dic[id];
    }
}
