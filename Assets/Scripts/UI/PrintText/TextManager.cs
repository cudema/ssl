using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    [SerializeField]
    GameObject panel;
    [SerializeField]
    PrintTextBax printTextBax;
    [SerializeField]
    Image npcUI;

    string[] strings;

    public bool isPlayingText {get; private set;}

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

    public void StartPrinting(PrintData data, bool ismove)
    {
        this.strings = data.strings;
        if (data.npcSprite != null)
        {
            npcUI.sprite = data.npcSprite;
            npcUI.color = new Color(1, 1, 1, 1);
        }
        OnText();
        StartCoroutine(PlayText(ismove));
    }

    IEnumerator PlayText(bool ismove)
    {
        isPlayingText = true;
        for (int i = 0; i < strings.Length; i++)
        {
            yield return StartCoroutine(printTextBax.PrintTextCoroutine(strings[i]));
        }

        printTextBax.ResetText();
        panel.SetActive(false);
        isPlayingText = false;
        if (ismove)
        {
            InputManager.instance.StartControll();
        }
        EndText();
    }

    public void EndText()
    {
        npcUI.sprite = null;
        npcUI.color = new Color(1, 1, 1, 0);
    }
}
