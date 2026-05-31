using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempGameOverManager : MonoBehaviour
{
    // Update is called once per frame
    public void OnGoTo()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
