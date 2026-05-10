[System.Serializable]
[AddTypeMenu("NewEffect/스위칭스킬/스위칭스킬 사용 시")]
public class StartSwichingSkillEffect : UseEffect
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
[AddTypeMenu("NewEffect/스위칭스킬/스위칭스킬 적중 시")]
public class HitSwichingSkillEffect : UseEffect
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
[AddTypeMenu("NewEffect/스위칭스킬/스위칭스킬 종료 시")]
public class EndSwichingSkillEffect : UseEffect
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