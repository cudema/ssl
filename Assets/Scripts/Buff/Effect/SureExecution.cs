[System.Serializable]
[AddTypeMenu("OldEffect/체력 데미지 증가 강화")]
public class SureExecution : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {

    }

    public void OnRemove(Player player)
    {

    }

    public float OnAttackAddDamagePerEffect(BuffManager enemy)
    {
        if (enemy.GetComponent<EnemyBase>().GetHpPer() < 15f)
        {
            return 0.15f;
        }

        return 0;
    }
}
