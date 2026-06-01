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

    public bool isLoading = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    Coroutine loading;

    public void LoadScene(SceneName sceneName)
    {
        if (loading != null) StopCoroutine(loading);
        loading = StartCoroutine(Loading(sceneName));
    }

    public IEnumerator Loading(SceneName sceneName)
    {
        Player.instance.StopPlayer();
        Player.instance.movement.movement.Controller.enabled = false;
        Player.instance.GetComponent<EffectManager>().ResetEffects();
        UIManager.instance.gameMenuUI.isOnable = false;
        UIManager.instance.pause.isOnable = false;

        yield return StartCoroutine(FadeOut());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.progress + "%");

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        //Debug.Log("로딩 끝");
        tempBool = StageManager.instance.StartScene();

        StartCoroutine(FadeIn());
    }

    bool tempBool;

    IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
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

        fadeImage.gameObject.SetActive(false);
        yield return null;
    }
}
