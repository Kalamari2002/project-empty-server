using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallRun : MonoBehaviour
{
    const float UP_FORCE = 5f;

    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayers;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(TouchingWall()) print("WALL");
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
        rb.AddForce(Vector3.up * UP_FORCE);
    }

    public void StopWallRun()
    {
        rb.useGravity = true;
    }
}
