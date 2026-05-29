using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextToInter : MonoBehaviour
{
    InteractiveObject inter;

    [SerializeField]
    PrintData printData;

    void Awake()
    {
        inter = GetComponent<InteractiveObject>();
    }

    void OnEnable()
    {
        inter.OnInteractionEvent += PrintText;
    }

    void OnDisable()
    {
        inter.OnInteractionEvent -= PrintText;
    }

    void PrintText()
    {
        TextManager.instance.StartPrinting(printData, false);
    }
}
