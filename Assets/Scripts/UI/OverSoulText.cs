using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverSoulText : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        text.text = (EconomyManager.Instance.GetCurrentSoul() - StageManager.instance.StartSoul).ToString();
    }
}
