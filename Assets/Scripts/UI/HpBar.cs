using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    Transform hpBackground;
    [SerializeField]
    Transform hpBar;
    [SerializeField]
    Transform hpEffect;

    Coroutine effect;

    void OnEnable()
    {
        Player.instance.ChangedHp += SetHp;
    }

    void OnDisable()
    {
        Player.instance.ChangedHp -= SetHp;
    }

    void SetHp()
    {
        //StopCoroutine(effect);
        hpBackground.localScale = new Vector3(1 + (Player.instance.HpBonus / 1000), 1, 1);
        hpBar.localScale = new Vector3(hpBackground.localScale.x * (Player.instance.CurrentHp / Player.instance.MaxHp), 1, 1);
        effect = StartCoroutine(HpEffecting());
    }

    IEnumerator HpEffecting()
    {
        while (hpEffect.localScale.x > hpBar.localScale.x)
        {
            float temp = hpEffect.localScale.x - hpBar.localScale.x > 0.1f ? hpEffect.localScale.x - hpBar.localScale.x : 0.1f;
            hpEffect.localScale = new Vector3(hpEffect.localScale.x - (temp * 3f * Time.deltaTime), 1, 1);

            yield return null;
        }
    }
}
