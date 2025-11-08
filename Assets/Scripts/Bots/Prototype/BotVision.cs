using UnityEngine;

public class BotVision : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] float visionAngle = 45f;  
    [SerializeField] int rayCount = 10;  
    [SerializeField] LayerMask visionLayerMask;
    [SerializeField] int enemiesInVicinity;
    [SerializeField] BotStateMachine botStateMachine;
    float visionDistance;

    private void Awake()
    {
        visionDistance = GetComponent<SphereCollider>().radius;
    }

    public Transform EnemyInSight()
    {
        if (enemiesInVicinity == 0)
        {
            return null;
        }
        for (int i = 0; i < rayCount; i++)
        {
            float angle = -visionAngle / 2f + (i * visionAngle / (rayCount - 1));  
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, visionDistance, visionLayerMask))
            {
                if (hit.transform.tag == "Player" || hit.transform.tag == "Bot")
                {
                    return hit.transform;
                }
                Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, direction * visionDistance, Color.red);
            }
        }
        return null;
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
