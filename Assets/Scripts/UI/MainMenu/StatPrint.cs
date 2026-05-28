using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPrint : MonoBehaviour
{
    [SerializeField]
    StatType statType;

    [SerializeField]
    TextMeshProUGUI text;

    void OnEnable()
    {
        text.text = Player.instance.playerStats.stats[statType].Value.ToString();
    }
}
