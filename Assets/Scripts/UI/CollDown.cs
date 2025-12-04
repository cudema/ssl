using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDown : MonoBehaviour
{
    [SerializeField]
    Transform collDownPlane;

    float time;

    public void OnCollDown(float time)
    {
        this.time = time;
        StartCoroutine(BeingCollDown());
    }

    IEnumerator BeingCollDown()
    {
        float tempTime = Time.time;
        collDownPlane.localScale = new Vector3(1, 1, 1);

        while (Time.time - tempTime < time)
        {
            collDownPlane.localScale = new Vector3(1, 1f - ((Time.time - tempTime) / time), 1);

            yield return null;
        }

        yield return null;
    }
}
