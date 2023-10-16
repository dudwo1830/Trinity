using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CardData
{
    public enum CardType
    {
        None, Attack, Skill, Heal
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Amount { get; set; }
    public CardType Type { get; set; }
    public int MaxLevel { get; set; }
    public float UpgradeAmount { get; set; }
    public int Coast { get; set; }
    public string Conditions { get; set; }
    public string ConditionDurations { get; set; }

    [BooleanTrueValues("Y")]
    [BooleanFalseValues("N")]
    public bool IsAllAble { get; set; } = false;

    public Dictionary<int, int> conditionInfo;

    public float defaultAmount;
    public int level;

    public CardData()
    {

    }

    public CardData(CardData data)
    {
        Id = data.Id;
        Name = data.Name;
        Description = data.Description;
        Amount = data.Amount;
        Type = data.Type;
        MaxLevel = data.MaxLevel;
        UpgradeAmount = data.UpgradeAmount;
        Coast = data.Coast;
        conditionInfo = data.conditionInfo;
        defaultAmount = data.defaultAmount;
        level = data.level;
    }

    public string GetDescription()
    {
        string newStr = Description;

        return newStr.Replace("{{Amount}}", Amount.ToString()).Replace("{{ConditionDuration}}", "¹Ì±¸Çö");
    }

    public void LevelUp()
    {
        if (level == MaxLevel)
        {
            return;
        }
        ++level;
        Name += "+";
        Amount += UpgradeAmount;
    }

    public void LevelDown()
    {
        --level;
        if (level < 1)
        {
            level = 0;
            Amount = defaultAmount;
            return;
        }
        Amount -= UpgradeAmount;
    }

    public override string ToString()
    {
        return $"Id: {Id}\nName: {Name}\nDesc: {GetDescription()}\nAmount: {Amount}\nType: {Type}\nLevel: {level}";
    }

    internal void ResetData()
    {
        level = 0;
        Amount = defaultAmount;
    }
}
