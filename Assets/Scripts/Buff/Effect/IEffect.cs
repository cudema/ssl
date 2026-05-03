using System;

public interface IEffect
{
    public void OnApply(Player player);
    public void OnRemove(Player player);
}

public interface IAttackEffect : IEffect
{
    public void OnAttackEffect(BuffManager enemy);
}

public interface IAttackAddDamageEffect : IEffect
{
    public float OnAttackAddDamageEffect();
}

public interface IAttackAddDamagePerEffect : IEffect
{
    public float OnAttackAddDamagePerEffect(BuffManager enemy);
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