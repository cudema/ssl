using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextPos : MonoBehaviour
{
    [SerializeField]
    PrintData printData;
    [SerializeField]
    string saveName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt(saveName) == 0)
            {
                TextManager.instance.StartPrinting(printData, true);
                PlayerPrefs.SetInt(saveName, 1);
            }
            
        }
    }
}
