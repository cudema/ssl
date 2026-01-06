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
    Transform weaponSocet;

    public Animator animator;

    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;

    SearchEnemy searchEnemy;

    Rigidbody rb;

    bool isDeshing = false;

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
        searchEnemy = GetComponent<SearchEnemy>();
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
        
        playerAttack.SetAttackCollider(Instantiate(weaponPrefab, weaponSocet).GetComponent<Collider>());
    }

    public void Attack(InputAction.CallbackContext value)
    {
        if (isDeshing)
        {
            StopCoroutine("Deshing");
            //animator.SetBool("IsMove", true);
            currentWeapon.DeshAttack();
            isDeshing = false;
        }
        else
        {
            currentWeapon.AttackWeapon();
            playerMovement.movement.LookAtTarget(searchEnemy.GetEnemyPos());
        }
    }

    public void ChangeAnimator(AnimatorController animatorController)
    {
        bool temp = animator.GetBool("IsMove");
        animator.runtimeAnimatorController = animatorController;
        animator.SetBool("IsMove", temp);
    }

    public void Skill(InputAction.CallbackContext value)
    {
        currentWeapon.AttackSkill();
    }

    public void Desh(InputAction.CallbackContext value)
    {
        isDeshing = true;

        StartCoroutine(Deshing());
    }

    IEnumerator Deshing()
    {
        //animator.SetBool("IsMove", false);
        float tempDeshTime = Time.time;

        while (Time.time - tempDeshTime < currentWeapon.deshTime)
        {
            if (playerMovement.PlayerDirection != Vector3.zero)
            {
                transform.position += playerMovement.PlayerDirection * (currentWeapon.deshRange / currentWeapon.deshTime * Time.deltaTime);
                yield return null;
            }
            else
            {
                transform.position += playerMovement.movement.renderTransform.forward * (currentWeapon.deshRange / currentWeapon.deshTime * Time.deltaTime);
                yield return null;
            }
        }

        //rb.velocity = Vector3.zero;
        //animator.SetBool("IsMove", true);

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
