using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public enum AttackType
{
    Nomal = 0, Skill, Switching
}

[System.Serializable]
public class WeaponAttackData
{
    [SerializeField]
    AttackType damageType;
    [SerializeField]
    float damage;
    [SerializeField]
    int switchingGauge;
    [SerializeField]
    float stiffenTime;
    [SerializeField]
    float knockbackRange;

    [HideInInspector]
    public int index;

    public float Damage
    {
        get => damage;
    }
    public int SwitchingGauge
    {
        get => switchingGauge;
    }
    public float StiffenTime
    {
        get => stiffenTime;
    }
    public float KnockbackRange
    {
        get => knockbackRange;
    }
    public AttackType DamageType
    {
        get => damageType;
    }
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Weapon")]
public class Weapon : ScriptableObject
{
    [HideInInspector]
    public int level = 0;
    [SerializeField, Header("무기 정보")]
    protected GameObject weaponPrefab;
    public WeaponType weaponType;
    public GameObject WeaponPrefab
    {
        get => weaponPrefab;
    }    
    [SerializeField]
    protected AnimatorController weaponAnimator;
    [SerializeField]
    protected Sprite weaponIcon;
    [SerializeField, Header("데미지 분류")]
    protected WeaponAttackData[] attackDatas;
    [SerializeField]
    protected WeaponAttackData[] skillData;
    [SerializeField]
    protected WeaponAttackData[] switchingSkillData;

    [SerializeField, Header("쿨다운")]
    // protected float switchingColldown = 0;
    // [SerializeField]
    protected float skillColldown = 0;

    [SerializeField, Header("대쉬")]
    public float deshRange = 2;
    [SerializeField]
    public float deshTime = 0.2f;

    [Header("스탯")]
    public List<StatEntry> initialStats = new List<StatEntry>();
    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    AttackType currentAttackType;

    [System.Serializable]
    public class StatEntry
    {
        public StatType type;
        public float baseValue;
    }

    PlayerWeapon playerWeapon;

    public void Setup(PlayerWeapon newPlayerWeapon)
    {
        level = 0;
        playerWeapon = newPlayerWeapon;
        isUseableSkill = true;
        foreach (var entry in initialStats)
        {
            stats[entry.type] = new Stat { baseValue = entry.baseValue };
        }
    }

    public void EquipWeaponNoSkill()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);
        //playerWeapon.ChangeWeaponSocet();

        UIManager.instance.weaponIcon.ChangeIcon(weaponIcon);
    }

    public void EquipWeapon()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);
        playerWeapon.ChangeWeaponSocet();

        UIManager.instance.weaponIcon.ChangeIcon(weaponIcon);
        //UIManager.instance.skillCollDown.OnCollDownReset();
        //isUseableSkill = true;

        //playerWeapon.playerAttack.SetupAttackData(switchingSkillData);

    }

    public void AttackWeapon()
    {
        OnAttack();
    }

    public void DeshAttack()
    {
        playerWeapon.animator.SetTrigger("deshAttack");
        //playerWeapon.StartCoroutine(DeshAttackTimer());
    }

    IEnumerator DeshAttackTimer()
    {
        bool temp = playerWeapon.animator.GetBool("IsMove");
        playerWeapon.animator.SetBool("IsMove", false);
        
        Debug.Log(playerWeapon.animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(4f / 24f);

        playerWeapon.animator.SetBool("IsMove", temp);
    }

    [HideInInspector]
    public bool isUseableSkill = true;

    public void AttackSkill()
    {
        if (isUseableSkill)
        {
            isUseableSkill = false;
            playerWeapon.StartCoroutine(SkillCollDown(skillColldown));
            //playerWeapon.playerAttack.SetupAttackData(skillData);
        }
    }

    public IEnumerator SkillCollDown(float collDown)
    {
        playerWeapon.currentColldown.OnCollDown(collDown);

        yield return playerWeapon.StartCoroutine(WaitForSecondsOfSkill(collDown));

        isUseableSkill = true;
    }

    float timer;

    IEnumerator WaitForSecondsOfSkill(float second)
    {
        timer = 0;
    
        while (timer <= second)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        yield break;
    }

    public void ReduceCollDown(float percent)
    {
        timer += skillColldown * percent;
        playerWeapon.currentColldown.ReduceTime(percent);
    }

    public void UnequipWeapon()
    {
        //isUseableSkill = true;
        Debug.Log("무기 교체");
    }

    protected virtual void OnAttack()
    {
        playerWeapon.animator.SetTrigger("attack");
    }

    public virtual void OnSkill()
    {
        playerWeapon.animator.SetTrigger("skill");
    }

    public virtual void SwitchingSkill()
    {
        playerWeapon.animator.SetTrigger("switching");
    }

    public void SetAttackIndex(int index = 0)
    {
        //Debug.Log(currentAttackType);
        switch (currentAttackType)
        {
            case AttackType.Nomal :
                attackDatas[index].index = index;
                playerWeapon.playerAttack.SetupAttackData(attackDatas[index]);
                break;
            case AttackType.Skill :
                skillData[index].index = index;
                playerWeapon.playerAttack.SetupAttackData(skillData[index]);
                break;
            case AttackType.Switching :
                switchingSkillData[index].index = index;
                playerWeapon.playerAttack.SetupAttackData(switchingSkillData[index]);
                break;
        }
    }

    public void SetAttackType(AttackType attackType)
    {
        currentAttackType = attackType;
    }
}
