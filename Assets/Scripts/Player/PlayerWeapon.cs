using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    Weapon currentWeapon;
    [SerializeField]
    Weapon mainWeapon;
    [SerializeField]
    Weapon subWeapon;

    [SerializeField]
    float maxSwitchingGauge;
    int switchingGauge;

    public Animator animator;

    public int SwitchingGauge
    {
        set => switchingGauge = (int)Mathf.Clamp(value, 0, maxSwitchingGauge);
        get => switchingGauge;
    }

    void Awake()
    {
        //mainWeapon = ScriptableObject.CreateInstance<Sword>();
        mainWeapon.Setup(this);
        //subWeapon = ScriptableObject.CreateInstance<Sword>();
        subWeapon.Setup(this);
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentWeapon = mainWeapon;
        mainWeapon.EquipWeapon();
    }

    public void ChangeWeapon(InputAction.CallbackContext value)
    {
        currentWeapon?.UnequipWeapon();

        if (currentWeapon == mainWeapon)
        {
            currentWeapon = subWeapon;
        }
        else
        {
            currentWeapon = mainWeapon;
        }

        currentWeapon.EquipWeapon();
    }

    public void Attack(InputAction.CallbackContext value)
    {
        currentWeapon.AttackWeapon();
    }

    public void ChangeAnimator(AnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }
}
