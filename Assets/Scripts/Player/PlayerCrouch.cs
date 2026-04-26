using UnityEngine;
using UnityEngine.Animations.Rigging;


/**
* TODO: punching people while crouched makes the camera snap back
* to its initial local position.
*/

[RequireComponent(typeof(Rigidbody))]
public class PlayerCrouch : MonoBehaviour
{
    const float CROUCH_OFFSET = 0.6f;
    const float SLIDE_FRICTION = 3f;
    const float CROUCH_COLLISION_HEIGHT = 1.36367f;
    const float CROUCH_COLLISION_Y = 0.3181652f;
    const float AIR_CROUCH_GROUNDCHECK_Y = -0.248f;

    float INIT_COLLISION_HEIGHT, INIT_COLLISION_Y, INIT_GROUNDCHECK_Y;
    bool isCrouching = false;

    [SerializeField] Transform cameraTransform;
    [SerializeField] CapsuleCollider collision;
    [SerializeField] Transform groundCheck;

    Vector3 initCameraPos;
    
    Rigidbody rb;

    void Start()
    {
        INIT_COLLISION_HEIGHT = collision.height;
        INIT_COLLISION_Y = collision.center.y;
        INIT_GROUNDCHECK_Y = groundCheck.localPosition.y;

        rb = GetComponent<Rigidbody>();
        initCameraPos = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Crouch(bool isGrounded)
    {
        if(isCrouching)
            return;
        
        isCrouching = true;
        
        float yOffset = isGrounded ? -CROUCH_COLLISION_Y : CROUCH_COLLISION_Y;
        float yCameraOffset = isGrounded ? CROUCH_OFFSET : 0;
        float yGroundCheckOffset = isGrounded ? INIT_GROUNDCHECK_Y : AIR_CROUCH_GROUNDCHECK_Y;
        
        collision.height = CROUCH_COLLISION_HEIGHT;
        collision.center = new Vector3(collision.center.x, yOffset, collision.center.z);
        groundCheck.localPosition = new Vector3(groundCheck.localPosition.x, yGroundCheckOffset, groundCheck.localPosition.z);

        cameraTransform.localPosition = new Vector3(
            initCameraPos.x, 
            initCameraPos.y - yCameraOffset, 
            initCameraPos.z
        );
    }

    public void StopCrouch()
    {
        isCrouching = false;
        collision.center = new Vector3(collision.center.x, INIT_COLLISION_Y, collision.center.z);
        collision.height = INIT_COLLISION_HEIGHT;
        cameraTransform.localPosition = initCameraPos;
        groundCheck.localPosition = new Vector3(groundCheck.localPosition.x, INIT_GROUNDCHECK_Y, groundCheck.localPosition.z);
    }

    void Slide()
    {

    }
}
