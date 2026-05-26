using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayText : StageNode
{
    [SerializeField]
    PrintData printData;

    bool isPlay = false;

    public override void VisitStageNode()
    {
        if (!isPlay)
        {
            isPlay = true;
            TextManager.instance.StartPrinting(printData);
            StartCoroutine(TempCoroutien());
            return;
        }
        StageManager.instance.SetStage(this);
        isVisited = true;
        StageManager.instance.MoveMiniMap();
    }

    IEnumerator TempCoroutien()
    {
        yield return new WaitWhile(() => TextManager.instance.isPlayingText);

        StageManager.instance.SetStage(this);
        isVisited = true;
        StageManager.instance.MoveMiniMap();
    }
}
