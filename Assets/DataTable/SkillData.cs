using System;
using System.ComponentModel;
using UnityEngine;

public class SkillData
{
    public enum SkillAttribute
    {
        None, Rock, Scissors, Paper
    }
    public enum SkillType
    {
        None, Attack, Heal
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SkillAttribute Attribute { get; set; }
    public float Amount { get; set; }
    public SkillType Type { get; set; }
    public int Level { get; set; }
    public float UpgradeAmount { get; set; }

    public float defaultAmount;

    public string GetDescription()
    {
        string newStr = Description;
        newStr.Replace("{{Amount}}", Amount.ToString());
        return newStr;
    }

    public void LevelUp()
    {
        ++Level;
        Amount += UpgradeAmount;
    }

    public void LevelDown()
    {
        --Level;
        if (Level < 1)
        {
            Level = 0;
            Amount = defaultAmount;
            return;
        }
        Amount -= UpgradeAmount;
    }

    public override string ToString()
    {
        return $"Id: {Id}\nName: {Name}\nDesc: {GetDescription()}\nAttr: {Attribute}\nAmount: {Amount}\nType: {Type}\nLevel: {Level}";
    }

    internal void ResetData()
    {
        Level = 0;
        Amount = defaultAmount;
    }
}
