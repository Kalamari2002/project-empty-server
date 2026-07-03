using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    [Header("Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float verticalClamp = 90f;
    [SerializeField] float horizontalClamp = 180f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        if (orientation == null) orientation = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        yRotation += mouseX;
        float positiveAngle = yRotation <= -90f ? yRotation + 360f : yRotation % 360f;
        yRotation = horizontalClamp == 0 ? positiveAngle : Mathf.Clamp(positiveAngle, -horizontalClamp, horizontalClamp);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        Debug.Log("Rotation Y: " + yRotation);
    }

    public void HorizontalClamp(float clamp)
    {
        horizontalClamp = clamp % 360f;
    }

    public void RemoveHorizontalClamp()
    {
        horizontalClamp = 0;
    }

    public void SetYRotation(float yRotation)
    {
        this.yRotation = yRotation;
    }
}
