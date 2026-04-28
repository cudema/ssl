using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    private List<IEffect> activeEffects = new List<IEffect>();
    private Player player = Player.instance;

    [SerializeField]
    Poison effect1;

    void Update()
    {
        foreach (var effect in activeEffects)
        {
            if (effect is IUpdateEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                Debug.Log("useUpdate");
                attackEffect.OnUpdateEffect();
            }
        }
    }

    public void AddEffect(IEffect effect)
    {
        Debug.Log($"Add effect: {effect}");
        foreach (IEffect tempEffect in activeEffects)
        {
            if (effect.GetType() == tempEffect.GetType())
            {
                return;
            }
        }
        activeEffects.Add(effect);
        effect.OnApply(player);
    }

    public void RemoveEffect(IEffect effect)
    {
        Debug.Log($"Remove effect: {effect}");

        if (activeEffects.Contains(effect))
        {
            effect.OnRemove(player); // 제거 전 반드시 구독 해제 로직 실행
            activeEffects.Remove(effect);
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

    public void OnCharacterAttack(EnemyBase enemy)
    {
        BuffHandler temp = enemy.GetComponent<BuffHandler>();
        // List<IEffect> 중에서 IAttackEffect를 구현한 것들만 골라서 실행
        foreach (var effect in activeEffects)
        {
            if (effect is IAttackEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                attackEffect.OnAttackEffect(temp);
            }
        }
    }

    public float OnAddDamage(EnemyBase enemy)
    {
        float temp = 0;
        BuffHandler tempHandler = enemy.GetComponent<BuffHandler>();
        foreach (var effect in activeEffects)
        {
            if (effect is IAttackAddDamageEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                temp += attackEffect.OnAttackAddDamageEffect(tempHandler);
            }
        }

        return temp;
    }

    public float OnAddDamagePer(EnemyBase enemy)
    {
        float temp = 0;
        BuffHandler tempHandler = enemy.GetComponent<BuffHandler>();
        foreach (var effect in activeEffects)
        {
            if (effect is IAttackAddDamagePerEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                temp += attackEffect.OnAttackAddDamagePerEffect(tempHandler);
            }
        }

        return temp;
    }

    public float OnAddDefance()
    {
        float temp = 0;
        foreach (var effect in activeEffects)
        {
            if (effect is IAddDefenceEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                temp += attackEffect.OnDefenceEffect();
            }
        }

        return temp;
    }

    public float OnAddDefancePer()
    {
        float temp = 0;
        foreach (var effect in activeEffects)
        {
            if (effect is IAddDefencePerEffect attackEffect) // 패턴 매칭 (C# 7.0+)
            {
                temp += attackEffect.OnDefencePerEffect();
            }
        }

        return temp;
    }
}
