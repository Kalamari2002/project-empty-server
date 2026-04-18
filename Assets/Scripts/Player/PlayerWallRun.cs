using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallRun : MonoBehaviour
{
    const float UP_FORCE = 5f;
    const float WALL_JUMP_FORCE = 15f;
    

    bool canWallRun = true;
    bool isWallRunning = false;

    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayers;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool TouchingWall()
    {
        return Physics.CheckBox(
            wallCheck.position, 
            wallCheck.GetComponent<BoxCollider>().size/2, 
            wallCheck.rotation, 
            wallLayers
        );
    }

    public void WallRun()
    {
        if(!canWallRun) return;
        rb.AddForce(Vector3.up * UP_FORCE);
    }

    public void WallJump(Vector3 direction)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction * WALL_JUMP_FORCE, ForceMode.Impulse);
    }

    public void SetCanWallRun(bool canWallRun)
    {
        this.canWallRun = canWallRun;
    }

    public bool GetCanWallRun()
    {
        return canWallRun;
    }

    public void SetIsWallRunning(bool isWallRunning)
    {
        this.isWallRunning = isWallRunning;
    }
    public bool GetIsWallRunning()
    {
        return isWallRunning;
    }
}
