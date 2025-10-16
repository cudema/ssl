using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Collider attackCollider;

    public void OnAttack()
    {
        attackCollider.enabled = true;
    }
    
    public void OffAttack()
    {
        attackCollider.enabled = false;
    }
}
