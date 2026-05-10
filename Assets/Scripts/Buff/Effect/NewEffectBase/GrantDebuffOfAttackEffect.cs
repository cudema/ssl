using UnityEngine;

[System.Serializable]
public class GrantDebuffOfAttackEffect : UseEffect
{
    [SerializeField]
    BuffData debuff;
    
    public override void OnApply(Player player)
    {
        
    }

    public override void OnRemove(Player player)
    {
        
    }

    public override void OnEffect(BuffManager buffHandler)
    {
        
    }
}
