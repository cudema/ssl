using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    StartMenu = 0, SampleScene, GameOver, StartStage, Stage_1, Stage_1_new_1, Stage_1_new_2, Stage_1_new_3,Stage_2_new_1, Stage_2_new_2, Stage_2_new_3, Map_Base, Boss_1, Boss_2, Boss_3, Stage_Tutorial, GameClear
}

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager instance;
    [SerializeField]
    Image fadeImage;
    [SerializeField]
    GameObject loadingImage;
    [SerializeField]
    float fadeSpeed;

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
        UIManager.instance.gameMenuUI.isOnable = false;
        UIManager.instance.pause.isOnable = false;

        yield return StartCoroutine(FadeOut());

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
        tempBool = StageManager.instance.StartScene();

        StartCoroutine(FadeIn());
    }

    bool tempBool;

    IEnumerator FadeOut()
    {
        while (fadeImage.color.a <= 1)
        {
            fadeImage.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        loadingImage.SetActive(true);
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        
        loadingImage.SetActive(false);

        while (fadeImage.color.a >= 0)
        {
            fadeImage.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        
        if (tempBool)
        {
            InputManager.instance.StartControll();
            UIManager.instance.gameMenuUI.isOnable = true;
            UIManager.instance.pause.isOnable = true;
        }    
        else 
        {
            Player.instance.movement.ResetCameraSet();
        }

        yield return null;
    }
}
