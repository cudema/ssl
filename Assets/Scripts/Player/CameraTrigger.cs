using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    Transform startPos;
    [SerializeField]
    LayerMask layerMask;

    void LateUpdate()
    {
        Vector3 rayStart = startPos.position + transform.forward;
        Vector3 rayDirection = transform.position - rayStart;

        playerMovement.MoveCamaraDistance(playerMovement.MaxCameraDistance);

        RaycastHit hit;

        if (Physics.Raycast(rayStart, rayDirection, out hit, playerMovement.MaxCameraDistance, layerMask))
        {
            float newDistance = Vector3.Distance(hit.point, startPos.position) - 0.5f;
            Debug.DrawRay(rayStart, rayDirection.normalized * newDistance, Color.blue, 0.3f);

            playerMovement.MoveCamaraDistance(newDistance);
        }
        else
        {
            Debug.DrawRay(rayStart, rayDirection.normalized * playerMovement.MaxCameraDistance, Color.blue, 0.3f);
        }

    }
}
