using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/디버프 부여")]
public class GrantDebuffOfAttackEffect : IAttackEffect
{
    [SerializeField]
    BuffData debuff;
    
    public void OnApply(Player player)
    {
        
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnAttackEffect(BuffManager buffHandler)
    {
        
    }
}
