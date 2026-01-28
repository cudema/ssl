using System.Collections;
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
    protected float switchingColldown = 0;
    [SerializeField]
    protected GameObject weaponPrefab;
    [SerializeField]
    protected AnimatorController weaponAnimator;
    [SerializeField]
    public float deshRange = 2;
    [SerializeField]
    public float deshTime = 0.2f;

    float tempTime;

    [SerializeField]
    protected int useSwitchingGauge;

    protected PlayerWeapon playerWeapon;

    public void Setup(PlayerWeapon newPlayerWeapon)
    {
        tempTime = -100;
        playerWeapon = newPlayerWeapon;
    }

    public void EquipWeaponNoSkill()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);
        playerWeapon.ChangeWeaponSocet(weaponPrefab);

        UIManager.instance.weaponIcon.ChangeIcon(weaponIcon);
    }

    public void EquipWeapon()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);
        playerWeapon.ChangeWeaponSocet(weaponPrefab);

        UIManager.instance.weaponIcon.ChangeIcon(weaponIcon);

        Player.instance.SwitchingGauge -= useSwitchingGauge;

        playerWeapon.playerAttack.SetupAttackData(switchingSkillData);

        SwitchingSkill();
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
        playerWeapon.StartCoroutine(DeshAttackTimer());
    }

    IEnumerator DeshAttackTimer()
    {
        bool temp = playerWeapon.animator.GetBool("IsMove");
        playerWeapon.animator.SetBool("IsMove", false);
        
        Debug.Log(playerWeapon.animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(4f / 24f);

        playerWeapon.animator.SetBool("IsMove", temp);
    }

    public void AttackSkill()
    {
        // if (UIManager.instance.skillCollDown.OnCollDown(skillColldown))
        // {
        //     playerWeapon.playerAttack.SetupAttackData(skillData);

        //     OnSkill();
        // }
    }

    public void UnequipWeapon()
    {
        
        Debug.Log("무기 교체");
    }

    protected virtual void OnAttack()
    {
        playerWeapon.animator.SetTrigger("attack");
    }

    protected virtual void OnSkill()
    {
        playerWeapon.animator.SetTrigger("skill");
    }

    protected virtual void SwitchingSkill()
    {
        playerWeapon.animator.SetTrigger("switching");
    }
}
