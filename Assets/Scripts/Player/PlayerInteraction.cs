using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    LayerMask layer;
    [SerializeField]
    float range;

    int collidersMaxIndex;
    Collider[] colliders = new Collider[5];

    void Update()
    {
        collidersMaxIndex = collidersMaxIndex = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, layer);
    }

    public void OnInteraction(InputAction.CallbackContext value)
    {
        if (collidersMaxIndex == 0)
        {
            return;
        }
        colliders[0].GetComponent<InteractiveObject>().OnInteraction();
    }
}
