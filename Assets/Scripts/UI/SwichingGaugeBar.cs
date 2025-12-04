using System;
using System.Collections;
using UnityEngine;

public class SwichingGaugeBar : MonoBehaviour
{
    [SerializeField]
    Transform gaugeBar;

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
        gaugeBar.localScale = new Vector3((float)Player.instance.SwitchingGauge / (float)Player.instance.MaxSwitchingGauge, 1, 1);
    }
}
