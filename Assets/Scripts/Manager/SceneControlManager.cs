using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }

    IEnumerator Loading(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
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
