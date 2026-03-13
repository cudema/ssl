[System.Serializable]
public class SureExecution : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {

    }

    public void OnRemove(Player player)
    {

    }

    public float OnAttackAddDamagePerEffect(BuffHandler enemy)
    {
        if (enemy.GetComponent<EnemyBase>().GetHpPer() < 15f)
        {
            return 0.15f;
        }

        return 0;
    }
}
