using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextToInter : InteractiveAction
{
    [SerializeField]
    PrintData printData;
    [SerializeField]
    InteractiveAction nextAction;

    public override void OnAction()
    {
        if (nextAction != null)
        {
            TextManager.instance.StartPrinting(printData, false);
            StartCoroutine(NextAction());
            return;
        }
        TextManager.instance.StartPrinting(printData, true);
    }

    IEnumerator NextAction()
    {
        yield return new WaitWhile(() => TextManager.instance.isPlayingText);

        nextAction.OnAction();
    }
}
