using UnityEngine;

[System.Serializable]
public class PoisonEffect : IAttackEffect
{
    Poison poison;

    public Poison Poison0 => poison;

    public void OnApply(Player player)
    {
        poison = Resources.Load<Poison>("Buff/Poison");
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnAttackEffect(BuffHandler enemy)
    {
        if (enemy == null) return;
        enemy.AddBuff(poison);
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
