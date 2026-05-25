using System;
using System.Collections;
using UnityEngine;

public class SwichingGaugeBar : MonoBehaviour
{
    [SerializeField]
    RectTransform gaugeBar;

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
    }
}