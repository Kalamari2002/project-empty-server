using UnityEngine;

public class BotVision : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] float visionAngle = 45f;  
    [SerializeField] float visionDistance = 10f;  
    [SerializeField] int rayCount = 10;  
    [SerializeField] LayerMask visionLayerMask;
    [SerializeField] int enemiesInVicinity;
    [SerializeField] BotStateMachine botStateMachine;
    SphereCollider sphereCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInVicinity > 0)
        {
            DrawVision();
        }
    }

    void DrawVision()
    {
        bool enemyInSight = false;
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the angle for the ray. Spread rays over the cone's angle
            float angle = -visionAngle / 2f + (i * visionAngle / (rayCount - 1));  // Angle spread evenly across the cone

            // Rotate the forward direction by the calculated angle
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            // Cast the ray from the enemy's position in the direction calculated
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, visionDistance, visionLayerMask))
            {
                botStateMachine.SetPriorityEnemy(hit.collider.transform);
                enemyInSight = true;
                break;
            }
            Debug.DrawRay(transform.position, direction * visionDistance, Color.red);
        }
        if (!enemyInSight)
        {
            botStateMachine.SetPriorityEnemy(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemiesInVicinity++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemiesInVicinity--;
        }
    }

}
