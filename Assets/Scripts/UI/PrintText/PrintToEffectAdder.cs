using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintToEffectAdder : MonoBehaviour
{
    [SerializeField]
    PrintData printData;

    void OnEnable()
    {
        UIManager.instance.effectAdder.AddedEffect += PrintText;
    }

    void OnDisable()
    {
        UIManager.instance.effectAdder.AddedEffect -= PrintText;
    }

    void PrintText()
    {
        TextManager.instance.StartPrinting(printData, true);
    }
}
