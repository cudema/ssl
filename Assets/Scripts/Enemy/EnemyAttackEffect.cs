using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEffect : EnemyAttack
{
    [SerializeField]
    ParticleSystem particle;

    public override void OnAttack()
    {
        base.OnAttack();
        particle.Play();
    }
}
