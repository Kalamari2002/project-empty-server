using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    Quaternion targetRotation;
    Vector3 offset;

    Rigidbody targetRB;
    const float TIME_TO_UPDATE_OFFSET = 0.3f;
    float offsetUpdateTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsetUpdateTimer = TIME_TO_UPDATE_OFFSET;
        offset = Vector3.zero;
        targetRB = target.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Add some sort of offset to targetRotation? We wanna make it so that Mr. Pink is susceptible to being
        //juxed. Holding one direction and then suddenly switching to the opposite direction should
        //make Mr. Pink whiff.

        //Likewise, holding one direction and never switching should make Mr. Pink catch up eventually
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        if((offsetUpdateTimer -= Time.deltaTime) <= 0)
        {
            UpdateOffset();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: if target gets too close, make offset less drastic
        Vector3 direction = (offset + target.position) - transform.position;
        targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void UpdateOffset()
    {
        print("Updated");
        offset = targetRB.linearVelocity / 3.0f;
        offsetUpdateTimer = TIME_TO_UPDATE_OFFSET;
    }
}
