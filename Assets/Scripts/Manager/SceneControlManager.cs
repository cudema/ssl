using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    StartMenu = 0, SampleScene, GameOver, StartStage, Stage_1, Map_Base
}

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    public void LoadScene(SceneName sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }

    public IEnumerator Loading(SceneName sceneName)
    {
        Player.instance.StopPlayer();
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

        yield return null;
    }
}
