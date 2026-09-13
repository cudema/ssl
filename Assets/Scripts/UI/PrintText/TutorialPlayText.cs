using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayText : StageNode
{
    [SerializeField]
    PrintData printData;

    [SerializeField]
    bool isPlay = false;

    public override void VisitStageNode()
    {
        if (!isPlay)
        {
            isPlay = true;
            TextManager.instance.StartPrinting(printData, true);
            StartCoroutine(TempCoroutien());
            return;
        }
        StageManager.instance.SetStage(this);
        isVisited = true;
        StageManager.instance.MoveMiniMap();
    }

    public void StartText()
    {
        isPlay = true;
        TextManager.instance.StartPrinting(printData, true);
        StartCoroutine(TempCoroutien());
        return;
    }

    IEnumerator TempCoroutien()
    {
        yield return new WaitWhile(() => TextManager.instance.isPlayingText);

        StageManager.instance.SetStage(this);
        isVisited = true;
        StageManager.instance.MoveMiniMap();
    }
}
