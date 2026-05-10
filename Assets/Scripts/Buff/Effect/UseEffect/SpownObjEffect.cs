using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("오브젝트 소환")]
public class SpownObjEffect : BaseEffect
{
    [SerializeField]
    GameObject spownPerfab;

    GameObject spownedObj;

    public override void OnEffect(BuffManager buffmanager)
    {
        if (spownedObj == null)
        {
            spownedObj = Player.Instantiate(spownPerfab, Player.instance.transform.position - new Vector3(0, 1, 0), Player.instance.movement.movement.renderTransform.rotation);
            return;
        }

        spownedObj.transform.position = Player.instance.transform.position;
        spownedObj.transform.rotation = Player.instance.movement.movement.renderTransform.rotation;
        spownedObj.SetActive(true);
    }
}
