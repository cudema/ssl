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

    public void OnGrayHPEffect()
    {
        //Player.instance.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().ChangedGrayHp += SetGrayHp;
    }

    public void OffGrayHPEffect()
    {
        //Player.instance.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().ChangedGrayHp -= SetGrayHp;
    }

    void SetHp(float value)
    {
        //StopCoroutine(effect);
        //hpBackground.localScale = new Vector3(1 + (Player.instance.CurrentHp / 1000), 1, 1);
        hpBar.localScale = new Vector3(Player.instance.CurrentHp / Player.instance.MaxHp, 1, 1);
        //effect = StartCoroutine(HpEffecting(value));
    }

    void SetGrayHp(float value)
    {
        if (effect != null) StopCoroutine(effect);

        effect = StartCoroutine(HpEffecting(value));
        hpEffect.localScale = new Vector3(Player.instance.CurrentHp / Player.instance.MaxHp + value / Player.instance.MaxHp, 1, 1);
    }

    IEnumerator HpEffecting(float value)
    {
        hpEffect.localScale = new Vector3(Player.instance.CurrentHp / Player.instance.MaxHp - value / Player.instance.MaxHp, 1, 1);

        Debug.Log(value);
        Debug.Log(Player.instance.CurrentHp / Player.instance.MaxHp - value / Player.instance.MaxHp);

        yield return new WaitForSeconds(1f);
        while (hpEffect.localScale.x > hpBar.localScale.x)
        {
            hpEffect.localScale = new Vector3(hpEffect.localScale.x - (1f / Player.instance.MaxHp * Time.deltaTime), 1, 1);

            yield return null;
        }
    }
}
