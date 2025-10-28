using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected abstract void Shoot();
}
