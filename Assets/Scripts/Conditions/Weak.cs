using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Weak : Condition
{
    private float amount = 0.25f;

    public float ApplyValue(float damage)
    {
        return damage - (damage * amount);
    }
}
