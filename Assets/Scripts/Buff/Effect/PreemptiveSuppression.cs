using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreemptiveSuppression : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {

    }

    public void OnRemove(Player player)
    {

    }

    public float OnAttackAddDamagePerEffect(BuffHandler enemy)
    {
        if (enemy.GetComponent<EnemyBase>().GetHpPer() > 30f)
        {
            return 0.07f;
        }

        return 0;
    }
}
