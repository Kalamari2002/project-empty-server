using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    [SerializeField] float hitStopDuration = 0.05f;
    [SerializeField] float punchImpulse = 40;
    [SerializeField] float enemyRagdollLaunchForce = 200;
    [SerializeField] Transform playerCamera;

    Coroutine camShakeRoutine;
    Transform orientation;
    Vector3 originalPlayerCameraPosition;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
        playerCamera = orientation.Find("Camera");
        originalPlayerCameraPosition = playerCamera.localPosition;
    }

    void Update()
    {

    }

    void ShakeCamera(float magnitude, float duration)
    {
        if (camShakeRoutine != null)
        {
            StopCoroutine(camShakeRoutine);
        }
        camShakeRoutine = StartCoroutine(CamShakeRoutine(magnitude, duration));
    }

    void HitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

    IEnumerator CamShakeRoutine(float magnitude, float duration)
    {
        playerCamera.localPosition = originalPlayerCameraPosition;
        float elapsed = 0;
        while (elapsed <= duration)
        {
            playerCamera.localPosition = originalPlayerCameraPosition + playerCamera.up * Random.Range(-magnitude, magnitude) + playerCamera.right * Random.Range(-magnitude, magnitude);
            elapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.localPosition = originalPlayerCameraPosition;
        camShakeRoutine = null;
    }

    IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    public Vector3 cameraForward()
    {
        return mainCamera.transform.forward;
    }

    public void CastHit(float punchRange, int damage, int damageAnimation)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * punchRange, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, punchRange, enemyLayer))
        {
            Debug.Log("Enemy Hit!");
            EnemyPrototype enemyPrototype = hit.transform.GetComponent<EnemyPrototype>();
            enemyPrototype.TakeDamage(damage, damageAnimation);
            if (damageAnimation == 3)
            {
                enemyPrototype.SpawnRagdoll(enemyRagdollLaunchForce, enemyRagdollLaunchForce);
            }
            ShakeCamera(camShakeMagnitude * damage, camShakeDuration * damage);
            HitStop(hitStopDuration * damage / 10);
        }
    }

    public void CastKickHit(float kickRange, int damage)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * kickRange, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, kickRange, enemyLayer))
        {
            Debug.Log("Enemy Hit!");
            EnemyPrototype enemyPrototype = hit.transform.GetComponent<EnemyPrototype>();
            enemyPrototype.TakeDamage(damage);
            float camShakeFactor = 20;
            ShakeCamera(camShakeMagnitude * camShakeFactor, camShakeDuration * camShakeFactor);
            HitStop(hitStopDuration * 2);
            enemyPrototype.SpawnRagdoll(enemyRagdollLaunchForce, 0);
        }
    }

    public void Impulse(float impulse)
    {
        rb.AddForce(orientation.forward * impulse, ForceMode.Impulse);
    }
}
