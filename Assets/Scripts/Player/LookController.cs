using UnityEngine;

public class LookController : MonoBehaviour
{
    public float mouseSensitivity = 150.0f;
    public float clampAngle = 80.0f;

    private Rigidbody playerRigidbody;
    private Vector3 angle = Vector3.zero;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        angle.x = rot.y;
        angle.y = rot.x;
    }

    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        angle.x -= mouseY * mouseSensitivity * Time.deltaTime;
        angle.y += mouseX * mouseSensitivity * Time.deltaTime;

        //Left & Right is Player Rotation
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(angle));

        //Up & Down is Camera Rotation
    }
}
