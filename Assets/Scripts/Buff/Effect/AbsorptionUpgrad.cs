using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/회복 강화")]
public class AbsorptionUpgrad : IEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp += 0.1f;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp -= 0.1f;
    }
}

[System.Serializable]
[AddTypeMenu("OldEffect/회색 체력 감소 시간")]
public class LingeringPain : IEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().DeownSpeed -= 10;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().DeownSpeed += 10;
    }
}

[System.Serializable]
[AddTypeMenu("OldEffect/회색 체력 시간 증가")]
public class FirmHoldout : IEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp = 0.6f;
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime += 2;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp = 0.7f;
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime -= 2;
    }
}

[System.Serializable]
[AddTypeMenu("OldEffect/시간 감소 회복 강화")]
public class ImmediateCounterattack : IEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp += 0.2f;
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime = 0;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp -= 0.2f;
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime = 1;
    }
}

[System.Serializable]
[AddTypeMenu("OldEffect/회색 체력 생성")]
public class IndomitableWill : IAddDefencePerEffect
{
    public void OnApply(Player player)
    {

    }

    public void OnRemove(Player player)
    {

    }

    public float OnDefencePerEffect()
    {
        float temp = 0;

        temp = 0.3f * (1 - Player.instance.CurrentHp / Player.instance.MaxHp);

        if (temp > 0.21f)
        {
            temp = 0.21f;
        }

        if (Player.instance.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().GrayHp > 0)
        {
            temp += 0.15f;
        }

        return temp;
    }
}

[System.Serializable]
[AddTypeMenu("OldEffect/방어력 증가")]
public class DevastatingBlow : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().recoveryPer += 0.03f;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().recoveryPer -= 0.03f;
    }

    public float OnAttackAddDamagePerEffect(BuffManager enemy)
    {
        float temp = 0;

        temp = 0.4f * (1 - Player.instance.CurrentHp / Player.instance.MaxHp);

        if (temp > 0.28f)
        {
            temp = 0.28f;
        }

        return temp;
    }
}