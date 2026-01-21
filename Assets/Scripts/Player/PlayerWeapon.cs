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

    [SerializeField]
    float dashColldown;

    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;

    SearchEnemy searchEnemy;

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
        
        playerMovement.movement.LookAtTarget(searchEnemy.GetEnemyPos());
        currentWeapon.EquipWeapon();
    }

    public void ChangeWeaponSocet(GameObject weaponPrefab)
    {
        Destroy(weaponSocet.GetChild(0)?.gameObject);
        
        playerAttack.SetAttackCollider(Instantiate(weaponPrefab, weaponSocet).GetComponent<Collider>());
        playerAttack.OffAttack();
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
        

        if (UIManager.instance.dechCollDown.OnCollDown(dashColldown))
        {
            isDeshing = true;
            animator.SetTrigger("Dash");
            Player.instance.ImpossPlayerMove();
            Player.instance.isInvincible = true;
            StartCoroutine(Deshing());
        }
    }

    IEnumerator Deshing()
    {
        //animator.SetBool("IsMove", false);
        float tempDeshTime = Time.time;
        Vector3 playerVector = playerMovement.PlayerDirection;
        Debug.Log(currentWeapon.deshRange / currentWeapon.deshTime);

        if (playerVector != Vector3.zero)
        {
            playerMovement.movement.LookAtTarget(transform.position + playerVector);
            while (Time.time - tempDeshTime <= currentWeapon.deshTime)
            {
                transform.position += playerVector * (currentWeapon.deshRange / currentWeapon.deshTime * Time.fixedDeltaTime);
                yield return null;
            }
        }
        else
        {
            while (Time.time - tempDeshTime <= currentWeapon.deshTime)
            {
                transform.position += playerMovement.movement.renderTransform.forward * (currentWeapon.deshRange / currentWeapon.deshTime * Time.fixedDeltaTime);
                yield return null;
            }
        }

        //rb.velocity = Vector3.zero;
        //animator.SetBool("IsMove", true);

        isDeshing = false;
        Player.instance.PossPlayerMove();
        Player.instance.isInvincible = false;
    }

    public void SetupWeapon(Weapon main, Weapon sub)
    {
        mainWeapon = main;
        subWeapon = sub;
        
        mainWeapon.Setup(this);
        subWeapon.Setup(this);

        currentWeapon = mainWeapon;
        mainWeapon.EquipWeaponNoSkill();
    }
}
