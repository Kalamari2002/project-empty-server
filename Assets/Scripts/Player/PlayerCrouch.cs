using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    bool isSliding = false;
    const float CROUCH_OFFSET = 0.6f;
    [SerializeField] Transform cameraTransform;
    Vector3 initCameraPos;
    void Start()
    {
        initCameraPos = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Crouch()
    {
        cameraTransform.localPosition = new Vector3(
            initCameraPos.x, 
            initCameraPos.y - CROUCH_OFFSET, 
            initCameraPos.z
        );
    }

    public void StopCrouch()
    {
        cameraTransform.localPosition = initCameraPos;
    }

    void Slide()
    {
        
    }
}
