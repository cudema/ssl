public class PoisonEffect : IAttackEffect
{
    public PoisonEffect(Poison poison)
    {
        this.poison = poison;
    }
    Poison poison;

    public Poison Poison0 => poison;

    public void OnApply(Player player)
    {
        
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
