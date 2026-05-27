using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask enemyRagdoll;
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    [SerializeField] float hitStopDuration = 0.05f;
    [SerializeField] float punchImpulse = 40;
    [SerializeField] float enemyRagdollLaunchForce = 200;
    [SerializeField] float goombaStompRotationThreshold;
    [SerializeField] float goombaStompLaunchForce = 10;
    [SerializeField] Transform playerCamera;
    [SerializeField] EnemyPrototype grabbedEnemy;

    Coroutine camShakeRoutine;
    Transform orientation;
    Vector3 originalPlayerCameraPosition;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
        playerCamera = orientation.Find("CameraHolder").Find("Camera");
        originalPlayerCameraPosition = playerCamera.localPosition;
    }

    void Update()
    {

    }

    public void ShakeCamera(float magnitude, float duration)
    {
        if (camShakeRoutine != null)
        {
            StopCoroutine(camShakeRoutine);
        }
        camShakeRoutine = StartCoroutine(CamShakeRoutine(magnitude, duration));
    }

    public void HitStop(float duration)
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
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy Hit!"); EnemyPrototype enemyPrototype = hit.transform.GetComponent<EnemyPrototype>();
                enemyPrototype.TakeDamage(damage, damageAnimation);
                if (damageAnimation == 3)
                {
                    enemyPrototype.SpawnRagdoll(enemyRagdollLaunchForce, enemyRagdollLaunchForce, (cameraForward() + Vector3.up).normalized);
                }
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                Debug.Log("Enemy Ragdoll Hit!");
                hit.transform.GetComponent<Rigidbody>().AddForce(enemyRagdollLaunchForce * (cameraForward() + Vector3.up).normalized, ForceMode.Impulse);
                hit.transform.root.GetComponent<EnemyRagdoll>().TakeHit();
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
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy Kicked!");
                EnemyPrototype enemyPrototype = hit.transform.GetComponent<EnemyPrototype>();
                enemyPrototype.TakeDamage(damage);
                enemyPrototype.SpawnRagdoll(enemyRagdollLaunchForce * 1.5f, 0, (cameraForward() * 2f + Vector3.up).normalized);
                GoombaStomp();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                Debug.Log("Enemy Ragdoll Kicked!");
                hit.transform.GetComponent<Rigidbody>().AddForce(enemyRagdollLaunchForce * 1.5f * (cameraForward() * 2f + Vector3.up).normalized, ForceMode.Impulse);
                hit.transform.root.GetComponent<EnemyRagdoll>().TakeHit();
            }
            float camShakeFactor = 20;
            ShakeCamera(camShakeMagnitude * camShakeFactor, camShakeDuration * camShakeFactor);
            HitStop(hitStopDuration * 2);
        }
    }

    public bool CastGrabHit(float grabRange)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.blueViolet);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, enemyLayer))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy Grabbed!");
                grabbedEnemy = hit.transform.GetComponent<EnemyPrototype>();
                grabbedEnemy.DisableRendering();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                Debug.Log("Enemy Ragdoll Grabbed!");
                grabbedEnemy = hit.transform.root.GetComponent<EnemyRagdoll>().GetParentEnemy();
                grabbedEnemy.DestroyActiveRagdoll();
            }
            return true;
        }
        return false;
    }

    public void LaunchGrabbedEnemy()
    {
        LaunchGrabbedEnemy(enemyRagdollLaunchForce, enemyRagdollLaunchForce, (cameraForward() + Vector3.up).normalized);
    }

    public void KickLaunchGrabbedEnemy()
    {
        LaunchGrabbedEnemy(enemyRagdollLaunchForce * 1.5f, 0, (cameraForward() * 2f + Vector3.up).normalized);
        GoombaStomp();
    }

    public void LaunchGrabbedEnemy(float launchForce, float torque, Vector3 direction)
    {
        if (grabbedEnemy == null) return;
        grabbedEnemy.SpawnRagdoll(launchForce, torque, direction, GrabPoint());
        grabbedEnemy = null;
    }

    public void ReleaseGrabbedEnemy()
    {
        if (grabbedEnemy == null) return;
        grabbedEnemy.SpawnRagdoll(10, 0, cameraForward(), GrabPoint()); 
        grabbedEnemy = null;
    }

    Vector3 GrabPoint()
    {
        return mainCamera.transform.position + cameraForward() * 1.2f - mainCamera.transform.up * 0.5f;
    }

    void GoombaStomp()
    {
        if (Mathf.Rad2Deg * mainCamera.transform.localRotation.x < goombaStompRotationThreshold || Grounded()) return;
        rb.AddForce(Vector3.up * goombaStompLaunchForce, ForceMode.Impulse);
    }

    public void Impulse(float impulse)
    {
        if (rb.linearVelocity.magnitude >= 0.01f) 
        {
            return;
        }
        rb.AddForce(orientation.forward * impulse, ForceMode.Impulse);
    }

    bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, groundLayer);
    }
}
