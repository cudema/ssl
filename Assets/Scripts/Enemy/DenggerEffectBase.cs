using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DenggerEffectBase : MonoBehaviour
{
    protected float time;
    [SerializeField]
    protected Transform denggerTransform;
    public abstract void Setup(Vector3 center, float radius, float time);
    public abstract void Setup(Vector3 center, float rangeX, float rangeZ, float time);

    protected abstract IEnumerator Dengging();
}
