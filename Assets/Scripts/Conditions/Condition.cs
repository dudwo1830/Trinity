using System.Collections;
using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Coeff { get; set; }

    public int duration = 0;
    public bool IsActive { get; set; }

    public abstract float FinalValue(float damage);

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
}
