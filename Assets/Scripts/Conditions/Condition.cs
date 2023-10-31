using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Condition : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Coeff { get; set; }

    public int duration = 0;

    public abstract float ModifyAmount(float amount);
    public abstract void Apply(Character character);
    public abstract void Remove(Character character);
}
