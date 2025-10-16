using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField]
    protected float weaponDamege;
    [SerializeField]
    protected GameObject weaponPrefab;
    [SerializeField]
    protected AnimatorController weaponAnimator;
    [SerializeField]
    protected PlayerWeapon playerWeapon;
    [SerializeField]
    protected float attackDilay = 0;
    [SerializeField]
    float tempTime;

    protected int useSwitchingGauge;

    public void Setup(PlayerWeapon newPlayerWeapon)
    {
        playerWeapon = newPlayerWeapon;
    }

    public void EquipWeapon()
    {
        playerWeapon.ChangeAnimator(weaponAnimator);

        if (playerWeapon.SwitchingGauge >= useSwitchingGauge)
        {
            playerWeapon.SwitchingGauge -= useSwitchingGauge;

            SwitchingSkill();
        }
    }

    public void AttackWeapon()
    {
        if (Time.time - tempTime > attackDilay)
        {
            tempTime = Time.time;

            OnAttack();
        }
    }

    public void UnequipWeapon()
    {
        Debug.Log("NoChangeWeaponSkill");
    }

    protected virtual void OnAttack()
    {

    }

    protected virtual void OnSkill()
    {

    }

    protected virtual void SwitchingSkill()
    {

    }
}
