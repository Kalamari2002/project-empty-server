using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallRun : MonoBehaviour
{
    const float WALL_CHECK_DIST = 0.8f;
    const float AWAy_FORCE = 5f;
    const float UP_FORCE = 4f;
    const float FORWARD_FORCE = 3f;

    bool canWallRun = true;
    bool isWallRunning = false;

    [SerializeField] Transform wallCheckOrigin;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] Transform orientation;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Debug.DrawRay(wallCheckOrigin.position, orientation.right * WALL_CHECK_DIST, Color.red);
        Debug.DrawRay(wallCheckOrigin.position, -orientation.right * WALL_CHECK_DIST, Color.red);
    }

    public int IsTouchingWall()
    {
        if(WallRayCast(-1).collider) return -1;
        if(WallRayCast(1).collider) return 1;
        return 0;
    }
    
    RaycastHit WallRayCast(int dir)
    {
        Physics.Raycast(
            wallCheckOrigin.position,
            dir * orientation.right,
            out RaycastHit hit,
            WALL_CHECK_DIST,
            wallLayers
        );    
        
        Debug.Log(hit.normal);

        return hit;
    }

    public void WallRun()
    {
        if(!canWallRun) return;
        rb.AddForce(Vector3.up * UP_FORCE);
    }

    public void WallJump(Vector3 direction)
    {
        
        RaycastHit leftHit = WallRayCast(-1);
        RaycastHit rightHit = WallRayCast(1);
        
        Vector3 wallNormal = leftHit.collider ? leftHit.normal : rightHit.normal;
        
        rb.AddForce(wallNormal * AWAy_FORCE, ForceMode.Impulse);
        rb.AddForce(Vector3.up * UP_FORCE, ForceMode.Impulse);
        rb.AddForce(direction * FORWARD_FORCE, ForceMode.Impulse);
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
