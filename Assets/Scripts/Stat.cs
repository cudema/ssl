using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Stat
{
    public float baseValue; // 기본값

    [SerializeField]
    private List<BuffModifier> modifiers = new List<BuffModifier>();
    
    // 값이 변경되었을 때만 재계산하기 위한 캐싱
    private bool isDirty = true;
    private float _value;

    public float Value {
        get {
            if (isDirty) {
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    public void AddModifier(BuffModifier mod)
    {
        modifiers.Add(mod);
        isDirty = true;
    }

    public bool RemoveModifier(BuffModifier mod)
    {
        if (modifiers.Remove(mod)) {
            isDirty = true;
            return true;
        }
        return false;
    }

    // 모든 특정 소스(예: 특정 버프 아이템)로부터 온 수정자 제거
    public void RemoveAllModifiersFromSource(object source)
    {
        int removedCount = modifiers.RemoveAll(m => m.source == source);
        if (removedCount > 0) isDirty = true;
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue;

        // 1. Flat 계산 (고정값 합산)
        float sumFlat = 0;
        foreach (var mod in modifiers.Where(m => m.type == BuffAddType.Flat))
            sumFlat += mod.value;
        
        finalValue += sumFlat;

        // 2. Additive 계산 (비율 합연산)
        float sumAdditive = 0;
        foreach (var mod in modifiers.Where(m => m.type == BuffAddType.Additive))
            sumAdditive += mod.value;
        
        finalValue *= (1.0f + sumAdditive);

        // 3. Multiplicative 계산 (최종 곱연산)
        foreach (var mod in modifiers.Where(m => m.type == BuffAddType.Multiplicative))
            finalValue *= mod.value;

        return Mathf.Max(finalValue, 0); // 최소값 0 보정
    }

    public void ForceDirty()
    {
        isDirty = true;
    }
}
