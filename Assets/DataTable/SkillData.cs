using System;

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
    public SkillAttribute Attribute { get; set;}
    public float Amount { get; set; }
    public SkillType Type { get; set; }
    public int Level { get; set; }

    public string GetDescription()
    {
        string newStr = Description;
        newStr.Replace("{{Amount}}", Amount.ToString());
        return newStr;
    }

    public override string ToString()
    {
        return $"Id: {Id}\nName: {Name}\nDesc: {GetDescription()}\nAttr: {Attribute}\nAmount: {Amount}\nType: {Type}\nLevel: {Level}";
    }
}
