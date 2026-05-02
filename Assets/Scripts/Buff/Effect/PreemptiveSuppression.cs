using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/체력 낮으면 데미지 증가")]
public class PreemptiveSuppression : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {

    }

    public void OnRemove(Player player)
    {

    }

    public float OnAttackAddDamagePerEffect(BuffManager enemy)
    {
        if (enemy.GetComponent<EnemyBase>().GetHpPer() > 30f)
        {
            return 0.07f;
        }

        return 0;
    }
}
