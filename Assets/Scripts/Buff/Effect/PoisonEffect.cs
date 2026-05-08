using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/독 부여")]
public class PoisonEffect : IAttackEffect
{
    [SerializeField]
    Poison poison;

    public Poison Poison0 => poison;

    public void OnApply(Player player)
    {
        poison = Resources.Load<Poison>("Buff/Poison");
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnEffect(BuffManager enemy)
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
