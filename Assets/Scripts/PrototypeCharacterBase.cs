using UnityEngine;

public class PrototypeCharacterBase : CharacterBase
{
    [SerializeField] Transform aim;
    [SerializeField] float shootSpread;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField] int damage = 5;
    GameManager gameManager;
    GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bullet = Resources.Load("VFX/Bullet") as GameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Shoot()
    {
        Vector3 shootDirection = aim.right * Random.Range(-shootSpread, shootSpread) + aim.up * Random.Range(-shootSpread, shootSpread) + aim.forward;
        Bullet bulletScript = Instantiate(bullet, aim.position, Quaternion.identity).GetComponent<Bullet>();
        if (Physics.Raycast(aim.position, shootDirection.normalized , out RaycastHit hit, range, layerMask))
        {
            PrototypeCharacterBase enemyCharacterBase = hit.collider.GetComponent<PrototypeCharacterBase>();
            if(enemyCharacterBase != null && enemyCharacterBase != this)
            {
                enemyCharacterBase.TakeDamage(damage);
            }
            Debug.Log(aim.position);
            Debug.Log(hit.point);
            Debug.Log(shootDirection);
            bulletScript.Initialize(aim.position, hit.point, shootDirection);
        }
        else
        {
            bulletScript.Initialize(aim.position, aim.position + shootDirection * range, shootDirection);
        }
    }

    

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            Respawn();
        }
    }

    protected virtual void Respawn()
    {
        health = maxHealth;
        transform.position = gameManager.GetRandomSpawnPoint().position;
    }
}
