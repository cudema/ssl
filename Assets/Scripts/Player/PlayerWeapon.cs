using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    static public PlayerWeapon instance;

    Weapon currentWeapon;
    [SerializeField]
    Weapon mainWeapon;
    [SerializeField]
    Weapon subWeapon;

    [SerializeField]
    int maxSwitchingGauge;
    [SerializeField]
    int switchingGauge;
    [SerializeField]
    Transform weaponSocet;

    public Animator animator;

    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;

    Rigidbody rb;

    bool isDeshing = false;

    public int SwitchingGauge
    {
        set => switchingGauge = Mathf.Clamp(value, 0, maxSwitchingGauge);
        get => switchingGauge;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

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

    public void ChangeWeaponSocet(GameObject weaponPrefab)
    {
        Destroy(weaponSocet.GetChild(0)?.gameObject);
        
        Instantiate(weaponPrefab, weaponSocet);
    }

    public void Attack(InputAction.CallbackContext value)
    {
        if (isDeshing)
        {
            StopCoroutine("Deshing");
            animator.SetBool("IsMove", true);
            currentWeapon.DeshAttack();
            isDeshing = false;
        }
        else
        {
            currentWeapon.AttackWeapon();
        }
    }

    public void ChangeAnimator(AnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }

    public void Skill(InputAction.CallbackContext value)
    {
        currentWeapon.AttackSkill();
    }

    public void Desh(InputAction.CallbackContext value)
    {
        isDeshing = true;

        StartCoroutine("Deshing");
    }

    IEnumerator Deshing()
    {
        animator.SetBool("IsMove", false);
        float tempDeshTime = Time.time;

        while (Time.time - tempDeshTime < currentWeapon.deshTime)
        {
            transform.position += playerMovement.PlayerDirection * (currentWeapon.deshRange / currentWeapon.deshTime * Time.deltaTime);
            yield return null;
        }

        //rb.velocity = Vector3.zero;
        animator.SetBool("IsMove", true);

        isDeshing = false;
    }

    public void SetupWeapon(Weapon main, Weapon sub)
    {
        mainWeapon = main;
        subWeapon = sub;
        
        mainWeapon.Setup(this);
        subWeapon.Setup(this);

        currentWeapon = mainWeapon;
        mainWeapon.EquipWeapon();

    }
}
