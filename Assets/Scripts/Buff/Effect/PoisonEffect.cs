using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/독 부여")]
public class PoisonEffect : UseEffect
{
    [SerializeField]
    Poison poison;

    public Poison Poison0 => poison;

    public override void OnApply(Player player)
    {
        poison = Resources.Load<Poison>("Buff/Poison");
    }

    public override void OnRemove(Player player)
    {
        
    }

    public override void OnEffect(BuffManager enemy)
    {
        if (enemy == null) return;
        enemy.AddBuff(poison);
        Debug.Log("독무침");
    }

    public void ChangePoisonValue(float value)
    {
        poison.damageValue += value;
    }

    public void ChangePoisonBuff(Poison poison)
    {
        this.poison = poison;
    }
}
