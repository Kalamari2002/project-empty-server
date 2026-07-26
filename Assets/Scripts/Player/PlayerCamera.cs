using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    [Header("Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float verticalClamp = 90f;
    bool isHorizontallyBounded = false;
    float clockWiseBound = 0f;
    float counterClockWiseBound = 0f;
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
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        if(isHorizontallyBounded)
        {
            if(
                (mouseX > 0f && yRotation + mouseX >= clockWiseBound) 
                || (mouseX < 0f && yRotation + mouseX <= counterClockWiseBound)
            ) return;
        }

        yRotation += mouseX; // This thing can grow indefinitely. It'll become a god.
        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void SetHorizontalBounds(float counterClockWise, float clockWise)
    {
        counterClockWiseBound = counterClockWise;
        clockWiseBound = clockWise;
        isHorizontallyBounded = true;
    }
    public void RemoveHorizontalBounds()
    {
        counterClockWiseBound = 0;
        clockWiseBound = 0;
        isHorizontallyBounded = false;
    }

    public float GetYRotation()
    {
        return yRotation;
    }

    public void SetYRotation(float yRotation)
    {
        this.yRotation = yRotation;
    }

    public void MakeRotationPositive()
    {
        if (yRotation >= 0f) return;
        yRotation = (yRotation + 360f) % 360f;
    }
}
