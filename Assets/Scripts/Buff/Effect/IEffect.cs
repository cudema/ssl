public interface IEffect
{
    public void OnApply(Player player);
    public void OnRemove(Player player);
}

public interface IAttackEffect : IEffect
{
    public void OnAttackEffect(BuffHandler enemy);
}

public interface IAttackAddDamageEffect : IEffect
{
    public float OnAttackAddDamageEffect(BuffHandler enemy);
}

public interface IAttackAddDamagePerEffect : IEffect
{
    public float OnAttackAddDamagePerEffect(BuffHandler enemy);
}