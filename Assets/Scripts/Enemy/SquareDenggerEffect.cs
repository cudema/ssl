using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareDenggerEffect : DenggerEffectBase
{
    public override void Setup(Vector3 center, float radius, float time)
    {
        transform.position = center;
        transform.localScale = new Vector3(1, radius, 1);
        this.time = time;
        gameObject.SetActive(true);
        denggerTransform.localScale = new Vector3(1, 0, 1);
        StartCoroutine(Dengging());
    }

    public override void Setup(Vector3 center, float rangeX, float rangeZ, float time)
    {
        transform.position = center;
        transform.localScale = new Vector3(rangeX, rangeZ, 1);
        this.time = time;
        gameObject.SetActive(true);
        denggerTransform.localScale = new Vector3(1, 0, 1);
        StartCoroutine(Dengging());
    }

    protected override IEnumerator Dengging()
    {
        while (denggerTransform.localScale.y < 1)
        {
            denggerTransform.localScale += (Vector3.up / time) * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
