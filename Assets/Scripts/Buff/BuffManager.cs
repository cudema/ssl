using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public BuffData data {get; private set;}
    public float remainingDuration;
    public float nextTickTime;
    public int currentStack;
    public long timestamp;
    public BuffModifier modifier;

    public BuffInstance(BuffData data)
    {
        this.data = data;
        this.remainingDuration = data.duration;
        this.nextTickTime = data.tickInterval;
        this.currentStack = 1;
    }
}

public class BuffManager : MonoBehaviour
{
    private List<BuffInstance> activeBuffs = new List<BuffInstance>();

    PlayerStats playerStats;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // 버프 추가 (OnApply)
    public void AddBuff(BuffData data)
    {
        // 1. 기존에 같은 종류의 버프가 있는지 확인
        BuffInstance existingBuff = activeBuffs.Find(b => b.data.id == data.id);

        if (existingBuff != null)
        {
            // 이미 버프가 존재한다면 정책에 따라 처리
            HandleStacking(existingBuff, data);
        }
        else
        {
            // 신규 적용
            ApplyNewBuff(data);
        }
    }

    private void HandleStacking(BuffInstance existing, BuffData newData)
    {
        switch (newData.stackPolicy)
        {
            case StackPolicy.Refresh:
                // 시간만 초기화
                existing.remainingDuration = newData.duration;
                newData.OnBuffEffect();
                break;

            case StackPolicy.Additive:
                // 중첩 횟수 증가 및 스탯 강화
                if (existing.currentStack < newData.maxStack)
                {
                    existing.currentStack++;
                    // 수정자의 값을 중첩 횟수에 맞춰 갱신 (예: 10 * 2스택 = 20)
                    existing.modifier.value = newData.value * existing.currentStack;
                    // 스탯 재계산 강제 요청
                    playerStats.stats[newData.targetStat].ForceDirty();
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
        }
    }

    void ApplyNewBuff(BuffData data)
    {
        // 일단은 신규 생성으로 진행
        BuffInstance newBuff = new BuffInstance(data);
        activeBuffs.Add(newBuff);

        // 2. 스탯 수정자 생성 및 적용
        BuffModifier mod = new BuffModifier(data.value, data.addType, newBuff);
        newBuff.modifier = mod; // 인스턴스에 보관해둬야 나중에 삭제 가능

        if (playerStats.stats.TryGetValue(data.targetStat, out Stat targetStat))
        {
            targetStat.AddModifier(mod);
            Debug.Log($"{data.id} 적용됨! 현재 {data.targetStat}: {targetStat.Value}");
        }

        data.OnBuffEffect();

        SortBuffs();
    }

    // 버프 제거 (OnRemove / Rollback)
    public void RemoveBuff(BuffInstance buff)
    {
        if (playerStats.stats.TryGetValue(buff.data.targetStat, out Stat targetStat))
        {
            // 보관하고 있던 수정자를 제거하여 스탯 롤백
            targetStat.RemoveModifier(buff.modifier);
            buff.data.OffBuffEffect();
            Debug.Log($"{buff.data.id} 해제됨! 현재 {targetStat.Value}");
        }

        activeBuffs.Remove(buff);
    }

    private void SortBuffs()
    {
        // 기획서: Priority(오름차순) -> Timestamp(내림차순)
        activeBuffs.Sort((a, b) => {
            if (a.data.priority != b.data.priority)
                return a.data.priority.CompareTo(b.data.priority);
            return b.timestamp.CompareTo(a.timestamp);
        });
    }

    void Update()
    {
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
    private void ExecuteTickEffect(BuffInstance buff)
    {
        // 예: 독 데미지라면 여기서 체력을 깎음
        Debug.Log($"{buff.data.id} 틱 효과 발동!");
    }
}