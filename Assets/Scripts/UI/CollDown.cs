using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDown : MonoBehaviour
{
    [SerializeField]
    Transform collDownPlane;
    [SerializeField]
    GameObject gameObj;

    Coroutine collDownCoruotine;

    float time = 0;
    float tempTime;

    public bool OnCollDown(float time)
    {
        if (this.time > 0)
        {
            return false;
        }

        this.time = time;
        collDownCoruotine = StartCoroutine(BeingCollDown());

        return true;
    }

    public void OnCollDownReset()
    {
        if (collDownCoruotine != null) StopCoroutine(collDownCoruotine);
        collDownPlane.localScale = new Vector3(1, 0, 1);
        time = 0;
    }

    IEnumerator BeingCollDown()
    {
        tempTime = time;
        collDownPlane.localScale = new Vector3(1, 1, 1);

        while (time > 0)
        {
            time -= Time.deltaTime;
            collDownPlane.localScale = new Vector3(1, time / tempTime, 1);

            yield return null;
        }

        collDownPlane.localScale = new Vector3(1, 0, 1);
        yield return null;
    }

    public void OnImage()
    {
        gameObj.SetActive(true);
    }

    public void OffImage()
    {
        gameObj.SetActive(false);
    }

    public void ReduceTime(float percent)
    {
        time -= tempTime * percent;
    }
}