using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneStarter : MonoBehaviour
{
    [SerializeField]
    GameObject cutSceneObj;

    bool isPlay = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlay)
        {
            cutSceneObj.SetActive(true);
            Player.instance.StopPlayer();
            isPlay = true;
        }
    }
}
