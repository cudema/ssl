using UnityEngine;

[System.Serializable]
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

    public void OnEffect(BuffManager buffHandler)
    {
        
    }
}
