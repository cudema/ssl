using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Transform effectPoint;

    List<ParticleSystem> effects = new List<ParticleSystem>();

    public void PlayAttackEffect(PlayerEffectData effectData)
    {
        foreach (ParticleSystem temp in effects)
        {
            if (temp.gameObject.name == effectData.EffectPrefab.name)
            {
                temp.transform.position = effectPoint.position;
                temp.transform.rotation = effectPoint.rotation * effectData.EffectPrefab.transform.rotation;
                temp.Play();
                return;
            }
        }

        GameObject tempObj = Instantiate(effectData.EffectPrefab, effectPoint.position, effectPoint.rotation * effectData.EffectPrefab.transform.rotation);
        tempObj.name = effectData.EffectPrefab.name;
        effects.Add(tempObj.GetComponent<ParticleSystem>());
    }
}