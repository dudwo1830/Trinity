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
    public string Description { get; set; }
    public int StartingHealth { get; set; }
    public string CardListPlainText { get; set; }
    public List<CardData> usingCardList = new List<CardData>();
}