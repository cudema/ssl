using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    [SerializeField]
    GameObject panel;
    [SerializeField]
    PrintTextBax printTextBax;

    string[] strings;

    public bool isPlayingText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void OnText()
    {
        panel.SetActive(true);
        InputManager.instance.StopControll();
    }

    public void StartPrinting(PrintData data)
    {
        this.strings = data.strings;
        OnText();
        StartCoroutine(PlayText());
    }

    IEnumerator PlayText()
    {
        isPlayingText = true;
        for (int i = 0; i < strings.Length; i++)
        {
            yield return StartCoroutine(printTextBax.PrintTextCoroutine(strings[i]));
        }

        printTextBax.ResetText();
        panel.SetActive(false);
        isPlayingText = false;
        InputManager.instance.StartControll();
    }
}
