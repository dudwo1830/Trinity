
public class Vulnerable : Condition
{
    private float amount = 1.5f;

    public float ApplyValue(float damage)
    {
        return damage * amount;
    }
}