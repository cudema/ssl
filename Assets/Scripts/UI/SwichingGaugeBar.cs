using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwichingGaugeBar : MonoBehaviour
{
    [SerializeField]
    RectTransform gaugeBar;

    [SerializeField]
    Image image;
    [SerializeField]
    Color nomalColor;
    [SerializeField]
    Color activeColor;

    void OnEnable()
    {
        Player.instance.ChangedSwitchingGauge += ChangeGaugeBar;
    }

    void OnDisable()
    {
        Player.instance.ChangedSwitchingGauge -= ChangeGaugeBar;
    }
    void ChangeGaugeBar()
    {
        //gaugeBar.localScale = new Vector3((float)Player.instance.SwitchingGauge / (float)Player.instance.MaxSwitchingGauge, 1, 1);
        gaugeBar.sizeDelta = new Vector2((float)Player.instance.SwitchingGauge / (float)Player.instance.MaxSwitchingGauge * 440f + 140f, gaugeBar.sizeDelta.y);

        if (Player.instance.SwitchingGauge < Player.instance.playerWeapon.useSwitchingGauge)
        {
            image.color = nomalColor;
        }
        else
        {
            image.color = activeColor;
        }
    }
}