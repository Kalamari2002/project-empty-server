using UnityEngine;
using UnityEngine.Animations.Rigging;


/**
* TODO: punching people while crouched makes the camera snap back
* to its initial local position.
*/

[RequireComponent(typeof(Rigidbody))]
public class PlayerCrouch : MonoBehaviour
{
    bool isCrouching = false;
    const float CROUCH_OFFSET = 0.6f;
    const float SLIDE_FRICTION = 3f;

    [SerializeField] Transform cameraTransform;
    Vector3 initCameraPos;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initCameraPos = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Crouch()
    {
        isCrouching = true;
        cameraTransform.localPosition = new Vector3(
            initCameraPos.x, 
            initCameraPos.y - CROUCH_OFFSET, 
            initCameraPos.z
        );
    }

    public void StopCrouch()
    {
        isCrouching = false;
        cameraTransform.localPosition = initCameraPos;
    }

    void Slide()
    {

    }
}
