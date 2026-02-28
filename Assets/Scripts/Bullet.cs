using UnityEngine;

public class Bullet : MonoBehaviour
{
    float distanceToDestroy = 200;
    float speed = 1000;
    Vector3 origin;
    Vector3 destination;
    Rigidbody rb;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, origin) >= distanceToDestroy
            || Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 origin, Vector3 destination, Vector3 direction)
    {
        this.origin = origin;
        this.destination = destination;
        transform.forward = direction;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}
