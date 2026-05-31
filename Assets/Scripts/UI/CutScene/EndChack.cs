using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class EndChack : MonoBehaviour
{
    [SerializeField]
    PlayableDirector cutsceneDirector;
    [SerializeField]
    string cutsceneSceneName;

    public static event Action OnCutsceneFinished;

    void OnEnable()
    {
        cutsceneDirector.stopped += OnCutsceneEnded;
        UIManager.instance.pause.isOnable = false;
    }

    void OnDisable()
    {
        cutsceneDirector.stopped -= OnCutsceneEnded;
    }

    void OnCutsceneEnded(PlayableDirector director)
    {
        gameObject.SetActive(false);
        InputManager.instance.StartControll();
        SoundManager.instance.PlayBGM();
        UIManager.instance.BattleUI.OnUI();
        OnCutsceneFinished?.Invoke();
        SceneManager.UnloadSceneAsync(cutsceneSceneName);
        UIManager.instance.pause.isOnable = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            InputManager.instance.StartControll();
            SoundManager.instance.PlayBGM();
            UIManager.instance.BattleUI.OnUI();
            OnCutsceneFinished?.Invoke();
            SceneManager.UnloadSceneAsync(cutsceneSceneName);
            UIManager.instance.pause.isOnable = true;
        }
    }
}
