using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class WeaponAttackData
{
    [SerializeField]
    float damage;
    [SerializeField]
    int switchingGauge;
    [SerializeField]
    float attackDilay;
    [SerializeField]
    Vector3 attackRange;

    public float Damage
    {
        get => damage;
    }
    public int SwitchingGauge
    {
        get => switchingGauge;
    }
    public float AttackDilay
    {
        get => attackDilay;
    }
    public Vector3 AttackRange
    {
        get => attackRange;
    }
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField]
    protected Sprite weaponIcon;
    [SerializeField]
    protected WeaponAttackData[] attackDatas;
    [SerializeField]
    protected WeaponAttackData skillData;
    [SerializeField]
    protected WeaponAttackData switchingSkillData;
    [SerializeField]
    protected float skillColldown = 0;
    [SerializeField]
    protected GameObject weaponPrefab;
    [SerializeField]
    protected AnimatorController weaponAnimator;
    [SerializeField]
    public float deshRange = 2;
    [SerializeField]
    public float deshTime = 0.2f;

    float tempTime;
    float tempSkillTime;

    [SerializeField]
    protected int useSwitchingGauge;

    protected PlayerWeapon playerWeapon;

    public void Setup(PlayerWeapon newPlayerWeapon)
    {
        tempTime = -100;
        tempSkillTime = -100;
        playerWeapon = newPlayerWeapon;
    }

    public void EquipWeapon()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);
        playerWeapon.ChangeWeaponSocet(weaponPrefab);

        UIManager.instance.weaponIcon.ChangeIcon(weaponIcon);

        if (Player.instance.SwitchingGauge >= useSwitchingGauge)
        {
            Player.instance.SwitchingGauge -= useSwitchingGauge;

            playerWeapon.playerAttack.SetupAttackData(switchingSkillData);

            SwitchingSkill();
        }
    }

    public void AttackWeapon()
    {
        tempTime = Time.time;

        playerWeapon.playerAttack.SetupAttackData(attackDatas[0]);

        OnAttack();
    }

    public void DeshAttack()
    {
        playerWeapon.animator.SetTrigger("deshAttack");
    }

    public void AttackSkill()
    {
        if (Time.time - tempSkillTime > skillColldown)
        {
            tempSkillTime = Time.time;

            UIManager.instance.skillCollDown.OnCollDown(skillColldown);

            playerWeapon.playerAttack.SetupAttackData(skillData);

            OnSkill();
        }
    }

    public void UnequipWeapon()
    {
        
        Debug.Log("무기 교체");
    }

    protected virtual void OnAttack()
    {

    }

    protected virtual void OnSkill()
    {

    }

    protected virtual void SwitchingSkill()
    {
        Debug.Log("스위칭 스킬 사용");
    }
}
