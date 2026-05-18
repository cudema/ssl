using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDenggerEffect : DenggerEffectBase
{
    public override void Setup(Vector3 center, float radius, float time)
    {
        transform.position = center;
        transform.localScale = new Vector3(2 * radius, 2 * radius, 2 * radius);
        this.time = time;
        gameObject.SetActive(true);
        denggerTransform.localScale *= 0;
        StartCoroutine(Dengging());

    }

    public override void Setup(Vector3 center, float rangeX, float rangeZ, float time)
    {
        transform.position = center;
        transform.localScale = new Vector3(2 * rangeX, 2 * rangeX, 2 * rangeX);
        this.time = time;
        gameObject.SetActive(true);
        denggerTransform.localScale *= 0;
        StartCoroutine(Dengging());

    }

    protected override IEnumerator Dengging()
    {
        while (denggerTransform.localScale.y < 1)
        {
            denggerTransform.localScale += (Vector3.one / time) * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
