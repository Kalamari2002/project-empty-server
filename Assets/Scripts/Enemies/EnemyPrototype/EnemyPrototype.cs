using UnityEngine;

public class EnemyPrototype : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float backOffDistance;
    [SerializeField] float stopDistance;
    Transform Player;
    CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Player == null) { return; }

        transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));
        if (Vector3.Distance(transform.position, Player.transform.position) >= stopDistance)
        {
            controller.SimpleMove(transform.forward * speed);
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) <= backOffDistance)
        {
            controller.SimpleMove(-transform.forward * speed/2);
        }
    }
}
