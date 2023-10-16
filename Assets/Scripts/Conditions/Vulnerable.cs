
public class Vulnerable : Condition
{
    public override float FinalValue(float damage)
    {
        return damage * Coeff;
    }
}