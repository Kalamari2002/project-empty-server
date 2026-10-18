using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask enemyRagdoll;
    [SerializeField] LayerMask enemyHurtBoxLayer;
    [SerializeField] LayerMask pickableLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    [SerializeField] float hitStopDuration = 0.05f;
    [SerializeField] float punchImpulse = 40;
    [SerializeField] float enemyRagdollLaunchForce = 200;
    [SerializeField] float weaponLaunchForce = 50;
    [SerializeField] float goombaStompRotationThreshold;
    [SerializeField] float goombaStompLaunchForce = 10;
    [SerializeField] float wallKickForce = 15;
    [SerializeField] Transform playerCamera;
    [SerializeField] EnemyStateMachine grabbedEnemy;
    [SerializeField] Transform weaponHolder;

    Coroutine camShakeRoutine;
    Transform orientation;
    Vector3 originalPlayerCameraPosition;
    Rigidbody rb;
    WeaponProp _currentWeapon;

    public EnemyStateMachine GrabbedEnemy { get { return grabbedEnemy; } }  
    public WeaponProp CurrentWeapon { get { return _currentWeapon; } }

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

    public IEnumerator CastHitRoutine(float punchRange, int damage, int damageAnimation, int hitActiveFrames)
    {
        for (int i = 0; i < hitActiveFrames; i++)
        {
            if (CastHit(punchRange, damage, damageAnimation))
            {
                break;
            }
            yield return null;
        }
    }

    bool CastHit(float punchRange, int damage, int enemyHitstunAnimation)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * punchRange, Color.blue);
        bool punchHit = false;
        if (Physics.Raycast(ray, out RaycastHit hit, punchRange, enemyLayer))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStateMachine enemyPrototype = hit.transform.GetComponent<EnemyStateMachine>();
                enemyPrototype.TakeDamage(damage);
                if (enemyHitstunAnimation < 3)
                {
                    enemyPrototype.EnterHitstun(enemyHitstunAnimation);
                }
                else
                {
                    enemyPrototype.EnterRagdoll(enemyRagdollLaunchForce, enemyRagdollLaunchForce, (cameraForward() + Vector3.up).normalized);
                }
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(enemyRagdollLaunchForce * (cameraForward() + Vector3.up).normalized, ForceMode.Impulse);
                hit.transform.root.GetComponent<EnemyRagdoll>().TakeHit(damage);
            }
            punchHit = true;
        }
        else if (Physics.Raycast(ray, out RaycastHit pickHit, punchRange, pickableLayer))
        {
            if (pickHit.transform.GetComponent<Rigidbody>())
            {
                WeaponProp weaponProp = pickHit.transform.GetComponent<WeaponProp>();
                if (weaponProp != null && !weaponProp.Grounded)
                {
                    pickHit.transform.forward = cameraForward();
                }
                pickHit.transform.GetComponent<Rigidbody>().AddForce(weaponLaunchForce * cameraForward(), ForceMode.Impulse);
            }
            punchHit = true;
        }
        else if (Physics.Raycast(ray, out RaycastHit interactableHit, punchRange, interactableLayer))
        {
            InteractableBase interactableScript = interactableHit.transform.GetComponent<InteractableBase>();
            if (interactableScript != null)
            {
                interactableScript.Action();
            }
            punchHit = true;
        }
        if (punchHit)
        {
            ShakeCamera(camShakeMagnitude * damage, camShakeDuration * damage);
            HitStop(hitStopDuration * damage / 10);
        }
        return punchHit;
    }

    public IEnumerator CastKickHitRoutine(float kickRange, float launchForceMultiplier, int damage, int hitActiveFrames)
    {
        for (int i = 0; i < hitActiveFrames; i++)
        {
            if (CastKickHit(kickRange, launchForceMultiplier, damage))
            {
                break;
            }
            yield return null;
        }
    }

    public IEnumerator CastWeaponHitRoutine()
    {
        for (int i = 0; i < CurrentWeapon.HitActiveFrames; i++)
        {
            if (CastWeaponHit())
            {
                break;
            }
            yield return null;
        }
    }

    bool CastWeaponHit()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        return CurrentWeapon.CastWeaponHit(ray, (cameraForward() * 3f + Vector3.up).normalized, cameraForward(), this);
    }

    bool CastKickHit(float kickRange, float launchForceMultiplier, int damage)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * kickRange, Color.blue);

        bool kickHit = false;

        if (Physics.Raycast(ray, out RaycastHit hit, kickRange, enemyLayer))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStateMachine enemyPrototype = hit.transform.GetComponent<EnemyStateMachine>();
                enemyPrototype.TakeDamage(damage);
                enemyPrototype.EnterRagdoll(enemyRagdollLaunchForce * launchForceMultiplier, 0, (cameraForward() * 2f + Vector3.up).normalized);
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                hit.transform.GetComponent<Rigidbody>()
                    .AddForce(enemyRagdollLaunchForce * launchForceMultiplier * (cameraForward() * 2f + Vector3.up).normalized, ForceMode.Impulse);
                hit.transform.root.GetComponent<EnemyRagdoll>().TakeHit(damage);
            }
            GoombaStomp();
            kickHit = true;
        }
        else if (Physics.Raycast(ray, out RaycastHit pickHit, kickRange, pickableLayer))
        {
            if(pickHit.transform.GetComponent<Rigidbody>())
            {
                WeaponProp weaponProp = pickHit.transform.GetComponent<WeaponProp>();
                if (weaponProp != null && !weaponProp.Grounded)
                {
                    pickHit.transform.forward = cameraForward();
                }
                pickHit.transform.GetComponent<Rigidbody>().AddForce(weaponLaunchForce * launchForceMultiplier * cameraForward(), ForceMode.Impulse);
            }
            GoombaStomp();
            kickHit = true;
        }
        else if (Physics.Raycast(ray, out RaycastHit interactableHit, kickRange, interactableLayer))
        {
            InteractableBase interactableScript = interactableHit.transform.GetComponent<InteractableBase>();
            if (interactableScript != null)
            {
                interactableScript.Action();
            }
            kickHit = true;
            GoombaStomp();
        }
        else if (Physics.Raycast(ray, out RaycastHit wallHit, kickRange, wallLayer))
        {
            GoombaStomp((wallHit.normal + Vector3.up * 1.5f).normalized, wallKickForce);
            kickHit = true;
        }
        if (kickHit)
        {
            float camShakeFactor = 20;
            ShakeCamera(camShakeMagnitude * camShakeFactor, camShakeDuration * camShakeFactor);
            HitStop(hitStopDuration * 2);
        }
        return kickHit;
    }


    public GrabActionsEnums CastGrabHit(float grabRange)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.blueViolet);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, enemyLayer))
        {
            Debug.Log("Enemy Layer Grabbed");
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStateMachine enemyStateMachine = hit.transform.GetComponent<EnemyStateMachine>();
                if (enemyStateMachine != null)
                {
                    if (enemyStateMachine.Attacking)
                    {
                        if (enemyStateMachine.VulnerableToCounter)
                        {
                            grabbedEnemy = enemyStateMachine;
                            grabbedEnemy.EnterGrabbed();
                            return GrabActionsEnums.COUNTER;
                        }
                        else
                        {
                            return GrabActionsEnums.NOTHING;
                        }
                    }
                    else
                    {
                        grabbedEnemy = enemyStateMachine;
                        grabbedEnemy.EnterGrabbed();
                        return GrabActionsEnums.GRAB;
                    }
                }
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                grabbedEnemy = hit.transform.root.GetComponent<EnemyRagdoll>().GetParentEnemy();
                if (grabbedEnemy != null)
                {
                    grabbedEnemy.EnterGrabbed();
                    return GrabActionsEnums.GRAB;
                }
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit pickHit, grabRange, pickableLayer))
        {
            Debug.Log("Pickable Layer Grabbed");
            _currentWeapon = pickHit.transform.GetComponent<WeaponProp>();
            if (_currentWeapon != null)
            {
                _currentWeapon.PickUp(weaponHolder);
                return GrabActionsEnums.PICKABLE;
            }
        }
        return GrabActionsEnums.NOTHING;
    }

    public void LaunchGrabbedEnemy()
    {
        LaunchGrabbedEnemy(enemyRagdollLaunchForce, enemyRagdollLaunchForce, (cameraForward() + Vector3.up).normalized);
    }
    public void LaunchGrabbedEnemy(float launchForce, float torque, Vector3 direction)
    {
        if (grabbedEnemy == null) return;
        grabbedEnemy.EnterRagdoll(launchForce, torque, direction, GrabPoint());
        grabbedEnemy = null;
    }

    public void KickLaunchGrabbedEnemy(float launchKickMultiplier)
    {
        LaunchGrabbedEnemy(enemyRagdollLaunchForce * launchKickMultiplier, 0, (cameraForward() * 2f + Vector3.up).normalized);
        GoombaStomp();
    }

    public void ReleaseGrabbedEnemy()
    {
        if (grabbedEnemy == null) return;
        grabbedEnemy.EnterRagdoll(10, 0, cameraForward(), GrabPoint()); 
        grabbedEnemy = null;
    }

    Vector3 GrabPoint()
    {
        return mainCamera.transform.position + cameraForward() * 1.2f - mainCamera.transform.up * 0.5f;
    }

    void GoombaStomp()
    {
        GoombaStomp(Vector3.up, goombaStompLaunchForce);
    }

    void GoombaStomp(Vector3 direction, float force)
    {
        if (Mathf.Rad2Deg * mainCamera.transform.localRotation.x < goombaStompRotationThreshold || Grounded()) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(direction * force, ForceMode.Impulse);
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

    public void DropCurrentWeapon()
    {
        _currentWeapon.Drop(mainCamera.transform.position + cameraForward() * 1.2f + mainCamera.transform.up * 0.25f, playerCamera.up, 
            cameraForward() * 1.5f * rb.linearVelocity.magnitude, Vector3.zero);
        _currentWeapon = null;
    }
}
