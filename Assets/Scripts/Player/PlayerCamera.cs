using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    [Header("Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float verticalClamp = 90f;
    float maxHorizontalClamp = 0f;
    float minHorizontalClamp = 0f;
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
        yRotation = (maxHorizontalClamp == minHorizontalClamp) 
            ? yRotation % 360f 
            : Mathf.Clamp(yRotation, minHorizontalClamp, maxHorizontalClamp) % 360f;
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Debug.Log("Rotation Y: " + yRotation);
    }

    public void SetHorizontalClamp(float min, float max)
    {
        minHorizontalClamp = min % 360f;
        maxHorizontalClamp = max % 360f;
    }
    public void RemoveHorizontalClamp()
    {
        minHorizontalClamp = 0;
        maxHorizontalClamp = 0;
    }

    public float GetYRotation()
    {
        return yRotation;
    }

    public void SetYRotation(float yRotation)
    {
        this.yRotation = yRotation;
    }
}
