using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

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

    [Default(0)] 
    public int UpgradeConditionDuration { get; set; }

    [BooleanTrueValues("Y", "y")]
    [BooleanFalseValues("N", "n")]
    public bool IsAllAble { get; set; }

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
        UpgradeConditionDuration = data.UpgradeConditionDuration;
        defaultAmount = data.defaultAmount;
        level = data.level;
    }

    public string GetDescription()
    {
        var newStr = Description.Replace("{{Amount}}", Amount.ToString());
        //for (int i = 1; i <= conditionInfo.Count; i++)
        //{
        //    var dic = conditionInfo.ElementAt(i);
        //    newStr = newStr.Replace($"{{ConditionDuration{dic.Key}}}", dic.Value.ToString());
        //}
        if (conditionInfo != null)
        {
            foreach (var dic in conditionInfo)
            {
                newStr = newStr.Replace("{{ConditionDuration"+dic.Key+"}}", dic.Value.ToString());
            }
        }
        return newStr;
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
        foreach (var dic in conditionInfo)
        {
            conditionInfo[dic.Key] += UpgradeConditionDuration;
        }
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
