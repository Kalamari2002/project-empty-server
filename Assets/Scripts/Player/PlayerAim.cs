using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    Coroutine camShakeRoutine;
    Vector3 originalPlayerCameraPosition;
    [SerializeField] Transform playerCamera;

    private void Start()
    {
        playerCamera = transform.Find("Orientation").Find("Camera");
        originalPlayerCameraPosition = playerCamera.localPosition;
    }

    void Update()
    {

    }

    public void CastHit(float range, int damage)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.blue);
        
        if (Physics.Raycast(ray, out RaycastHit hit, range, enemyLayer))
        {
            Debug.Log("Enemy Hit!");
            hit.transform.GetComponent<EnemyPrototype>().TakeDamage(damage);
            ShakeCamera(camShakeMagnitude * damage, camShakeDuration * damage);
        }
    }

    void ShakeCamera(float magnitude, float duration)
    {
        if (camShakeRoutine != null)
        {
            StopCoroutine(camShakeRoutine);
        }
        camShakeRoutine = StartCoroutine(CamShakeRoutine(magnitude, duration));
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
    public Vector3 cameraForward()
    {
        return mainCamera.transform.forward;
    }
}
