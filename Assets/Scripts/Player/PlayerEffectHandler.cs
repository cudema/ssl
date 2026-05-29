using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    private List<Effect> activeEffects = new List<Effect>();
    private Player player = Player.instance;

    // void Update()
    // {
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is UpdateEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             Debug.Log("useUpdate");
    //             attackEffect.OnEffect(null);
    //         }
    //     }
    // }

    public void AddEffect(Effect effect)
    {
        Debug.Log($"Add effect: {effect}");
        foreach (Effect tempEffect in activeEffects)
        {
            if (effect.effectID == tempEffect.effectID)
            {
                return;
            }
        }
        activeEffects.Add(effect);
        effect.OnApply(player);
    }

    public void RemoveEffect(Effect effect)
    {
        Debug.Log($"Remove effect: {effect.effectID}");

        foreach(var tempEffect in activeEffects)
        {
            if (tempEffect.effectID == effect.effectID)
            {
                effect.OnRemove(player); // 제거 전 반드시 구독 해제 로직 실행
                activeEffects.Remove(effect);    
                return;
            }
        }
    }

    public void ClearAllEffects()
    {
        // 모든 효과의 OnRemove를 먼저 실행하여 이벤트를 정리
        foreach (var effect in activeEffects)
        {
            effect.OnRemove(player);
        }
        activeEffects.Clear();
    }

    public T FindEffect<T>() where T : class
    {
        foreach (var effect in activeEffects)
        {
            if (effect is T temp)
            {
                return temp;
            }
        }
        return null;
    }

    // public void OnCharacterAttack(EnemyBase enemy)
    // {
    //     BuffManager temp = enemy.GetComponent<BuffManager>();
    //     // List<IEffect> 중에서 IAttackEffect를 구현한 것들만 골라서 실행
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IAttackEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             attackEffect.OnAttackEffect(temp);
    //         }
    //     }
    // }

    // public float OnAddDamage(EnemyBase enemy)
    // {
    //     float temp = 0;
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IAttackAddDamageEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             temp += attackEffect.OnAttackAddDamageEffect();
    //         }
    //     }

    //     return temp;
    // }

    // public float OnAddDamagePer(EnemyBase enemy)
    // {
    //     float temp = 0;
    //     BuffManager tempHandler = enemy.GetComponent<BuffManager>();
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IAttackAddDamagePerEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             temp += attackEffect.OnAttackAddDamagePerEffect(tempHandler);
    //         }
    //     }

    //     return temp;
    // }

    // public float OnDesh(EnemyBase enemy)
    // {
    //     float temp = 0;
    //     BuffManager tempHandler;
    //     if (enemy != null)
    //     {
    //         tempHandler = enemy.GetComponent<BuffManager>();
    //     }
    //     else
    //     {
    //         tempHandler = null;
    //     }
        
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IDeshEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             attackEffect.OnDeshEffect(tempHandler);
    //         }
    //     }

    //     return temp;
    // }

    public void OnUseEffect<T>(EnemyBase enemy) where T : UseEffect
    {
        BuffManager tempHandler;
        if (enemy != null)
        {
            tempHandler = enemy.GetComponent<BuffManager>();
        }
        else
        {
            tempHandler = null;
        }
        
        foreach (var effect in activeEffects)
        {
            if (effect is T attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                attackEffect.OnEffect(tempHandler);
            }
        }
    }

    public float GetEffectValue<T>(EnemyBase enemy) where T : AddValueEffect
    {
        float temp = 0;
        BuffManager tempHandler = enemy.GetComponent<BuffManager>();
        foreach (var effect in activeEffects)
        {
            if (effect is T attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                temp += attackEffect.OnEffect(tempHandler);
            }
        }

        return temp;
    }

    // public float OnSuccessEvasion(EnemyBase enemy)
    // {
    //     float temp = 0;
    //     BuffManager tempHandler;
    //     if (enemy != null)
    //     {
    //         tempHandler = enemy.GetComponent<BuffManager>();
    //     }
    //     else
    //     {
    //         tempHandler = null;
    //     }
        
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is ISuccessEvasionEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             attackEffect.OnSuccessEvasionEffect(tempHandler);
    //         }
    //     }

    //     return temp;
    // }

    // public float OnAddDefance()
    // {
    //     float temp = 0;
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IAddDefenceEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             temp += attackEffect.OnDefenceEffect();
    //         }
    //     }

    //     return temp;
    // }

    // public float OnAddDefancePer()
    // {
    //     float temp = 0;
    //     foreach (var effect in activeEffects)
    //     {
    //         if (effect is IAddDefencePerEffect attackEffect) // 패턴 매칭 (C# 7.0+)
    //         {
    //             temp += attackEffect.OnDefencePerEffect();
    //         }
    //     }

    //     return temp;
    // }
}
