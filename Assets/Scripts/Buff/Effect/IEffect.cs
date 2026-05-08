using System;

public interface IEffect
{
    public void OnApply(Player player);
    public void OnRemove(Player player);
}

public interface IUseEffect : IEffect
{
    public void OnEffect(BuffManager enemy);
}

public interface IAddValueEffect : IEffect
{
    public float OnEffect(BuffManager enemy);
}

public interface IAttackEffect : IUseEffect
{
    
}

public interface IDeshEffect : IUseEffect
{
    
}

public interface IAttackAddDamageEffect : IAddValueEffect
{
    
}

public interface ISuccessEvasionEffect : IUseEffect
{
    
}

public interface IHPChanged : IUseEffect
{
    
}

public interface IUpdateEffect : IUseEffect
{
    
}

public interface IAddDefenceEffect : IAddValueEffect
{
    
}

public interface IAddDefencePerEffect : IAddValueEffect
{
    
}