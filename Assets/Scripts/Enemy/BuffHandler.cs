using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : BuffManager
{
    private List<BuffInstance> activeBuffs = new List<BuffInstance>();

    void Awake()
    {
        healthable = GetComponent<IHealthable>();
    }

    protected override void ApplyNewBuff(BuffData data)
    {
        // 일단은 신규 생성으로 진행
        BuffInstance newBuff = new BuffInstance(data);
        activeBuffs.Add(newBuff);

        // 2. 스탯 수정자 생성 및 적용
        BuffModifier mod = new BuffModifier(data.value, data.addType, newBuff);
        newBuff.modifier = mod; // 인스턴스에 보관해둬야 나중에 삭제 가능

        data.OnBuffEffect(this);

        SortBuffs();
    }

    protected override void HandleStacking(BuffInstance existing, BuffData newData)
    {
        switch (newData.stackPolicy)
        {
            case StackPolicy.Refresh:
                // 시간만 초기화
                existing.remainingDuration = newData.duration;
                newData.OnBuffEffect(this);
                break;

            case StackPolicy.Additive:
                // 중첩 횟수 증가 및 스탯 강화
                if (existing.currentStack < newData.maxStack)
                {
                    existing.currentStack++;
                    // 수정자의 값을 중첩 횟수에 맞춰 갱신 (예: 10 * 2스택 = 20)
                    existing.modifier.value = newData.value * existing.currentStack;
                }
                existing.remainingDuration = newData.duration; // 시간도 갱신
                break;

            case StackPolicy.Replace:
                // 더 강한 효과일 때만 덮어쓰기
                if (newData.value > existing.data.value)
                {
                    RemoveBuff(existing);
                    ApplyNewBuff(newData);
                }
                break;

            case StackPolicy.Independent:
                // 독립적으로 하나 더 추가
                ApplyNewBuff(newData);
                break;
                
            case StackPolicy.AdditiveNotTimeReset:
                // 중첩 횟수 증가 및 스탯 강화
                if (existing.currentStack < newData.maxStack)
                {
                    existing.currentStack++;
                    // 수정자의 값을 중첩 횟수에 맞춰 갱신 (예: 10 * 2스택 = 20)
                    existing.modifier.value = newData.value * existing.currentStack;
                }   
                break;
        }
    }
    void Update()
    {
        Debug.Log(activeBuffs);
        float dt = Time.deltaTime;
        // 역순 순회하며 지속시간 체크
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            var buff = activeBuffs[i];
            
            // 영구 버프(-1)가 아니면 시간 차감
            if (buff.data.duration > 0)
            {
                buff.remainingDuration -= dt;
                if (buff.remainingDuration <= 0)
                {
                    RemoveBuff(buff);
                    continue;
                }
            }

            // OnTick 처리
            if (buff.data.tickInterval > 0)
            {
                buff.nextTickTime -= dt;
                if (buff.nextTickTime <= 0)
                {
                    ExecuteTickEffect(buff);
                    buff.nextTickTime = buff.data.tickInterval;
                }
            }
        }
    }
    
    public override void RemoveBuff(BuffInstance buff)
    {
        activeBuffs.Remove(buff);
    }

    protected override void ExecuteTickEffect(BuffInstance buff)
    {
        buff.data.TickBuffEffect(healthable);
        Debug.Log($"{buff.data.id} 틱 효과 발동!");
    }
}
