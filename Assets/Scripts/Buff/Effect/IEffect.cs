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

public interface IHPChanged : IEffect
{
    public void ChangedHP(float value);
}

public interface IUpdateEffect : IEffect
{
    public void OnUpdateEffect();
}

public interface IAddDefenceEffect : IEffect
{
    public float OnDefenceEffect();
}

public interface IAddDefencePerEffect : IEffect
{
    public float OnDefencePerEffect();
}