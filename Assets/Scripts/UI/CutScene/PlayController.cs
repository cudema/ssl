using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;

    [SerializeField]
    StageNode node;

    [SerializeField]
    Transform playerPoint;

    void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer source)
    {
        gameObject.SetActive(false);
        Player.instance.OnPositionSet(playerPoint.position, playerPoint.rotation);
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
        node.VisitStageNode();
    }
}
