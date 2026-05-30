using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("무적 부여")]
public class AddInvincble : BaseEffect
{
    [SerializeField]
    float time;

    public override void OnEffect(BuffManager enemy)
    {
        Player.instance.StartCoroutine(InvincbleTime());
    }

    IEnumerator InvincbleTime()
    {
        Player.instance.isInvincible = true;

        yield return new WaitForSeconds(time);

        Player.instance.isInvincible = false;
    }
}
