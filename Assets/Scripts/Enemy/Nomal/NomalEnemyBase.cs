using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemyBase : EnemyBase
{
    [SerializeField]
    protected float[] patternWeights;
    protected int currentPattenIndex = -1;
    
    public override void OnAttack()
    {
        base.OnAttack();

        if (patternWeights.Length > 1)
        {
            float randomFloat = Random.Range(0, patternWeights.Length * 100);

            currentPattenIndex = randomFloat switch
            {
                var x when x <= patternWeights[0] => 0,
                var x when x <= patternWeights[0] + patternWeights[1] => 1,
                var x when x <= patternWeights[0] + patternWeights[1] + patternWeights[2] => 2,
                var x when x <= patternWeights[0] + patternWeights[1] + patternWeights[2] + patternWeights[3] => 3,
                var x when x <= patternWeights[0] + patternWeights[1] + patternWeights[2] + patternWeights[3] + patternWeights[4] => 4,
                _ => -1
            };

            float tempFloat = patternWeights[currentPattenIndex];
            animator.SetInteger("AttackIndex", currentPattenIndex);

            for (int i = 0; i < patternWeights.Length; i++)
            {
                if (i == currentPattenIndex)
                {
                    patternWeights[i] *= 2f / 3f;
                    continue;
                }

                patternWeights[i] += tempFloat * (1f / 3f) / (patternWeights.Length - 1);
            }
            
        }

        PlayAttackAnimation();
    }
}
