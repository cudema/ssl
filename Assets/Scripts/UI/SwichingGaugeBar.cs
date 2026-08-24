using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwichingGaugeBar : MonoBehaviour
{
    [SerializeField]
    RectTransform gaugeBar;

    [SerializeField, Header("활성화 색")]
    Image image;
    [SerializeField]
    Color nomalColor;
    [SerializeField]
    Color activeColor;

    [SerializeField, Header("활성화 아이콘")]
    Image switchingImage;
    [SerializeField]
    Sprite activeSwitchingImage;
    [SerializeField]
    Sprite deactiveSwitchingImage;

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
            switchingImage.sprite = deactiveSwitchingImage;
        }
        else
        {
            image.color = activeColor;
            switchingImage.sprite = activeSwitchingImage;
        }
    }
}