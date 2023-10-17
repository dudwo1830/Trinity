public class ConditionData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Coeff { get; set; }

    public int duration = 0;
    public bool IsActive = false;

    public ConditionData()
    {

    }

    public ConditionData(ConditionData data)
    {
        Id = data.Id;
        Name = data.Name;
        Description = data.Description;
        Coeff = data.Coeff;
    }

    public float ApplyValue(float amount)
    {
        switch (Id)
        {
            case 1:
                return amount + (amount * Coeff);
            case 2:
                return amount - (amount * Coeff);
        }
        return amount;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Duration: {duration}";
    }
}
