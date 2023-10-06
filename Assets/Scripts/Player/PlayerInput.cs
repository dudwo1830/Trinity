using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string verticalAxisName = "Vertical";
    public string horizontalAxisName = "Horizontal";

    public float vertical { get; private set; }
    public float horizontal { get; private set; }

    private void Update()
    {
        vertical = Input.GetAxis(verticalAxisName);
        horizontal = Input.GetAxis(horizontalAxisName);
    }
}
