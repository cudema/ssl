using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneM : MonoBehaviour
{
    [SerializeField]
    GameObject MainUI;
    [SerializeField]
    SemiCut cutUI0;
    [SerializeField]
    SemiCut cutUI1;
    [SerializeField]
    SemiCut cutUI2;
    [SerializeField]
    SemiCut cutUI3;
    

    [SerializeField]
    TutorialPlayText playerset;

    [SerializeField]
    float fadeInTime = 1;
    [SerializeField]
    float fadeOutTime = 1;

    void Start()
    {
        InputManager.instance.StopControll();
        MainUI.SetActive(true);
        UIManager.instance.pause.isOnable = false;
        StartCoroutine(CutM());
    }

    IEnumerator CutM()
    {
        cutUI0.gameObject.SetActive(true);

        yield return StartCoroutine(cutUI0.PlayCut(fadeInTime, fadeOutTime));

        cutUI0.gameObject.SetActive(false);
        cutUI1.gameObject.SetActive(true);

        yield return StartCoroutine(cutUI1.PlayCut(fadeInTime, fadeOutTime));

        cutUI1.gameObject.SetActive(false);
        cutUI2.gameObject.SetActive(true);

        yield return StartCoroutine(cutUI2.PlayCut(fadeInTime, fadeOutTime));

        cutUI2.gameObject.SetActive(false);
        cutUI3.gameObject.SetActive(true);

        yield return StartCoroutine(cutUI3.PlayCut(fadeInTime, fadeOutTime));

        cutUI3.gameObject.SetActive(false);

        endCut();
    }

    void endCut()
    {
        playerset.StartText();
        MainUI.SetActive(false);
        UIManager.instance.pause.isOnable = true;
        //Player.instance.SetupPlayer();
    }
}
