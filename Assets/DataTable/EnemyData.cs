using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Linq;

public class EnemyData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int StartingHealth { get; set; }
    public string ActionListText { get; set; }

    public List<int> useCardIdList = new List<int>();

    public override string ToString()
    {
        return $"{Name} / {StartingHealth} / {ActionListText}";
    }
}