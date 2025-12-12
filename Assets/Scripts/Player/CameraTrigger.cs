using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    LayerMask layerMask;

    void LateUpdate()
    {
        Vector3 rayStart = playerMovement.transform.position;
        Vector3 rayDirection = transform.position - rayStart;

        playerMovement.MoveCamaraDistance(playerMovement.MaxCameraDistance);

        RaycastHit hit;

        if (Physics.Raycast(rayStart, rayDirection, out hit, playerMovement.MaxCameraDistance, layerMask))
        {
            float newDistance = Vector3.Distance(hit.point, playerMovement.transform.position) - 0.2f;

            playerMovement.MoveCamaraDistance(newDistance);
        }
    }
}
