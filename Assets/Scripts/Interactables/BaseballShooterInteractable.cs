using UnityEngine;

public class BaseballShooterInteractable : InteractableBase
{
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileLaunchForce;
    [SerializeField] Transform spawnPoint;
    public override void Action()
    {
        GameObject projectileInstance = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody>().AddForce(projectileLaunchForce * spawnPoint.up, ForceMode.Impulse);
    }
}
