using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneStarter : MonoBehaviour
{
    [SerializeField]
    Transform playerPoint;
    [SerializeField]
    string sceneName;
    [SerializeField]
    StageNode node;
    bool isPlay = false;

    void OnEnable()
    {
        EndChack.OnCutsceneFinished += End;
    }

    void OnDisable()
    {
        EndChack.OnCutsceneFinished -= End;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlay)
        {
            Player.instance.OnPositionSet(playerPoint.position, playerPoint.rotation);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            SoundManager.instance.StopBGM();
            UIManager.instance.BattleUI.OffUI();
            Player.instance.StopPlayer();
            Player.instance.OffCamera();
            Player.instance.movement.movement.Controller.enabled = false;
            isPlay = true;
        }
    }

    void End()
    {
        node.VisitStageNode();
    }
}