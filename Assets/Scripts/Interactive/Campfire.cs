using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Campfire : InteractiveObject
{
    [SerializeField]
    float value;

    public void SetValue(float value)
    {
        this.value = value;
    }

    protected override void OnAction()
    {
        Player.instance.CurrentHp += Player.instance.MaxHp * value;
        PlayerHealingAura healingAura = Player.instance.GetComponent<PlayerHealingAura>();
        if (healingAura == null)
        {
            healingAura = Player.instance.gameObject.AddComponent<PlayerHealingAura>();
        }
        healingAura.Play();
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
    }
}