using UnityEngine;
using System.Collections.Generic;

public enum BuffEffectType
{
    Buff = 0,
    Debuff,
    Hidden
}

public enum BuffAddType
{
    Flat = 100,
    Additive = 200,
    Multiplicative = 300
}

public enum StackPolicy
{
    Refresh = 0,
    Additive,
    Replace,
    Independent
}

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff System/Buff Data")]
public class BuffData: ScriptableObject
{
    [Header("기본 정보")]
    public int id;
    public BuffEffectType type;
    public int priority;
    [Header("생명 주기")]
    public float duration;
    public float tickInterval;
    [Header("효과 설정")]
    public StatType targetStat;
    public BuffAddType addType;
    public float value;
    [Header("중첩 정책")]
    public StackPolicy stackPolicy;
    public int maxStack;

    public virtual void OnBuffEffect()
    {
        
    }

    public virtual void TickBuffEffect()
    {
        
    }

    public virtual void OffBuffEffect()
    {
        
    }
}