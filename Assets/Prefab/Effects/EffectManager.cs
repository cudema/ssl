using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Transform effectPoint;

    public void PlayAttackEffect(PlayerEffectData effectData)
    {
        Instantiate(effectData.EffectPrefab, effectPoint.position, effectPoint.rotation * effectData.EffectPrefab.transform.rotation);
    }
}