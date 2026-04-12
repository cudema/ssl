using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    StartMenu = 0, SampleScene, GameOver, StartStage, Stage_1, Map_Base, Boss_1, Boss_2, Boss_3
}

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(SceneName sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }

    public IEnumerator Loading(SceneName sceneName)
    {
        Player.instance.StopPlayer();
        Player.instance.GetComponent<EffectManager>().ResetEffects();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress + "%");

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("로딩 끝");
        StageManager.instance.StartScene();

        yield return null;
    }
}
