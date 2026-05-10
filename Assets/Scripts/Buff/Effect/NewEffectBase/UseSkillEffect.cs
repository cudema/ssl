[System.Serializable]
[AddTypeMenu("NewEffect/스킬/스킬 사용 시")]
public class StartSkillEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
        
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffManager)
    {
        useEffect.OnUseEffect(buffManager);
    }
}

[System.Serializable]
[AddTypeMenu("NewEffect/스킬/스킬 적중 시")]
public class HitSkillEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
        
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffManager)
    {
        useEffect.OnUseEffect(buffManager);
    }
}

[System.Serializable]
[AddTypeMenu("NewEffect/스킬/스킬 종료 시")]
public class EndSkillEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
        
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffManager)
    {
        useEffect.OnUseEffect(buffManager);
    }
}