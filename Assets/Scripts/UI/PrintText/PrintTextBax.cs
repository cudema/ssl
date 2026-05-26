using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintTextBax : MonoBehaviour
{
    [SerializeField]
    float printDelay;

    [SerializeField]
    TextMeshProUGUI text;

    bool isPlay = false;

    bool isSkipPrint;

    void Update()
    {
        if (isPlay && Input.anyKeyDown)
        {
            isSkipPrint = true;
        }
    }

    public void ResetText()
    {
        text.text = "";
    }

    public IEnumerator PrintTextCoroutine(string printText)
    {
        if (isPlay || printText == null)
        {
            yield break;
        }

        isSkipPrint = false;

        ResetText();
        string tempText = printText;

        yield return null;

        isPlay = true;

        for (int i = 0; i < tempText.Length; i++)
        {
            if (isSkipPrint)
            {
                text.text = tempText;
                yield return null;
                break;
            }

            text.text += tempText[i];

            yield return new WaitForSeconds(printDelay);
        }

        isPlay = false;
        isSkipPrint = false;
        yield return new WaitUntil(() => Input.anyKeyDown);
    }
}
