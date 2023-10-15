using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Coeff {  get; set; }

    public int duration = 0;

}
