using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDown : MonoBehaviour
{
    [SerializeField]
    Transform collDownPlane;

    float time = 0;

    public bool OnCollDown(float time)
    {
        if (this.time > 0)
        {
            return false;
        }

        this.time = time;
        StartCoroutine(BeingCollDown());

        return true;
    }

    IEnumerator BeingCollDown()
    {
        float tempTime = time;
        collDownPlane.localScale = new Vector3(1, 1, 1);

        while (time > 0)
        {
            time -= Time.deltaTime;
            collDownPlane.localScale = new Vector3(1, time / tempTime, 1);

            yield return null;
        }

        yield return null;
    }
}
