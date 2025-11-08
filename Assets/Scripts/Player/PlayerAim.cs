using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float range;
    [SerializeField] LayerMask layerMask;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HitEnemy())
                print("HIT!");
            else
                print("Miss");
        }
    }

    bool HitEnemy()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0.0f));
        if(Physics.Raycast(ray, out RaycastHit hit, range, layerMask))
        {
            return hit.collider.CompareTag("Enemy");
        }
        return false;
    }
}
