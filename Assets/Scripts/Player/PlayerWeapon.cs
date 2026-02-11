using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon currentWeapon;
    [SerializeField]
    Weapon mainWeapon;
    [SerializeField]
    Weapon subWeapon;

    [SerializeField]
    Transform weaponSocet;

    public Animator animator;

    [SerializeField]
    float dashColldown;
    [SerializeField]
    float switchingColldown = 0;
    [SerializeField]
    BattleAcceleration accelerationBuff;
    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;
    BuffManager buffManager;
    //SearchEnemy searchEnemy;

    Coroutine deshCoroutine;

    bool isDeshing = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        buffManager = GetComponent<BuffManager>();
        //searchEnemy = GetComponent<SearchEnemy>();
    }

    public void ChangeWeapon(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;
        if (!UIManager.instance.SwitchingColldown.OnCollDown(switchingColldown)) return;

        buffManager.AddBuff(accelerationBuff);

        currentWeapon?.UnequipWeapon();

        if (currentWeapon == mainWeapon)
        {
            currentWeapon = subWeapon;
        }
        else
        {
            currentWeapon = mainWeapon;
        }
        
        playerMovement.LookAtEnemy();
        currentWeapon.EquipWeapon();
        Player.instance.SetStat(StatType.AttackDamage);
        Player.instance.SetStat(StatType.CriticalRange);
        //-----------------------------제압력 추가------------------------------------------
    }

    public void ChangeWeaponSocet(GameObject weaponPrefab)
    {
        Destroy(weaponSocet.GetChild(0)?.gameObject);
        
        playerAttack.SetAttackCollider(Instantiate(weaponPrefab, weaponSocet).GetComponent<Collider>());
        playerAttack.OffAttack();
    }

    public void Attack(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;
        if (isDeshing)
        {
            StopCoroutine(deshCoroutine);
            animator.SetBool("IsMove", true);
            currentWeapon.DeshAttack();
            isDeshing = false;
        }
        else
        {
            currentWeapon.AttackWeapon();
            playerMovement.LookAtEnemy();
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
        if (!Player.instance.IsInputEnabled) return;

        currentWeapon.AttackSkill();
    }

    public void Desh(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;

        if (UIManager.instance.dechCollDown.OnCollDown(dashColldown))
        {
            isDeshing = true;
            animator.SetTrigger("Dash");
            Player.instance.ImpossPlayerMove();
            Player.instance.isInvincible = true;
            deshCoroutine = StartCoroutine(Deshing());
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

        yield return null;
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
