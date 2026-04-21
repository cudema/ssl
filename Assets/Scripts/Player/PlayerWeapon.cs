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
    GameObject invincibleEffect;

    Collider mainWeaponObj;
    Collider subWeaponObj;

    [SerializeField]
    Transform weaponSocet;
    [SerializeField]
    Transform attackPos;

    public Animator animator;

    [SerializeField]
    float dashColldown;
    [SerializeField]
    float switchingColldown = 0;
    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;
    //SearchEnemy searchEnemy;

    //Coroutine deshCoroutine;

    //bool isDeshing = false;

    List<ParticleSystem> weaponParticles = new List<ParticleSystem>();

    [HideInInspector]
    public CollDown currentColldown;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        //searchEnemy = GetComponent<SearchEnemy>();
        StartCoroutine(HitPosSet());
    }

    public void ChangeWeapon(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;
        if (!Player.instance.movement.PlayerMoveable) return;
        if (!UIManager.instance.SwitchingColldown.OnCollDown(switchingColldown)) return;

        currentWeapon?.UnequipWeapon();

        if (currentWeapon == mainWeapon)
        {
            currentWeapon = subWeapon;
            currentColldown.OffImage();
            currentColldown = UIManager.instance.subSkillColldown;
            currentColldown.OnImage();
        }
        else
        {
            currentWeapon = mainWeapon;
            currentColldown.OffImage();
            currentColldown = UIManager.instance.mainSkillColldown;
            currentColldown.OnImage();
        }
        
        currentWeapon.EquipWeapon();
        Player.instance.SetStat(StatType.AttackDamage);
        Player.instance.SetStat(StatType.CriticalRange);
        //-----------------------------제압력 추가------------------------------------------
    }

    public void SetupWeaponSocet()
    {
        for (int i = 0; i < weaponSocet.childCount; i++)
        {
            Destroy(weaponSocet.GetChild(i).gameObject);
        }
        mainWeaponObj = Instantiate(mainWeapon.WeaponPrefab, weaponSocet).GetComponent<Collider>();
        subWeaponObj = Instantiate(subWeapon.WeaponPrefab, weaponSocet).GetComponent<Collider>();

        subWeaponObj.gameObject.SetActive(false);
        playerAttack.SetAttackCollider(mainWeaponObj);

        weaponParticles.Clear();

        weaponParticles.AddRange(mainWeaponObj.GetComponentsInChildren<ParticleSystem>());
        weaponParticles.AddRange(subWeaponObj.GetComponentsInChildren<ParticleSystem>());
    }

    public void ChangeWeaponSocet()
    {
        if (currentWeapon == subWeapon)
        {
            subWeaponObj.gameObject.SetActive(true);
            mainWeaponObj.gameObject.SetActive(false);
            playerAttack.SetAttackCollider(subWeaponObj);
        }
        else
        {
            subWeaponObj.gameObject.SetActive(false);
            mainWeaponObj.gameObject.SetActive(true);
            playerAttack.SetAttackCollider(mainWeaponObj);
        }
        playerAttack.OffAttack();
    }

    Coroutine attackChack;

    public void Attack(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;
        // if (isDeshing)
        // {
        //     StopCoroutine(deshCoroutine);
        //     //animator.SetBool("IsMove", true);
        //     currentWeapon.DeshAttack();
        //     isDeshing = false;
        // }
        //else
        {
            if (attackChack != null) StopCoroutine(attackChack);
            attackChack = StartCoroutine(AttackChacking());
            currentWeapon.AttackWeapon();
        }
    }

    IEnumerator AttackChacking()
    {
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("attack");
    }

    void AttackReset()
    {
        animator.ResetTrigger("attack");
    }

    public void ChangeAnimator(AnimatorController animatorController)
    {
        bool temp = animator.GetBool("IsMove");
        animator.runtimeAnimatorController = animatorController;
        animator.SetBool("IsMove", temp);
    }

    Coroutine skillChack;

    public void Skill(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;

        if (skillChack != null) StopCoroutine(skillChack);
        skillChack = StartCoroutine(SkillChacking());
        if (currentWeapon.isUseableSkill) currentWeapon.OnSkill();
    }

    IEnumerator SkillChacking()
    {
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("skill");
    }

    public void OnSkill()
    {
        currentWeapon.AttackSkill();
    }

    public void Desh(InputAction.CallbackContext value)
    {
        if (!Player.instance.IsInputEnabled) return;

        if (UIManager.instance.dechCollDown.OnCollDown(dashColldown))
        {
            //isDeshing = true;
            animator.SetTrigger("Dash");
            Player.instance.ImpossPlayerMove();
            Player.instance.isInvincible = true;
            invincibleEffect.SetActive(true);
            playerMovement.StopMovement();
            /*deshCoroutine = */StartCoroutine(Deshing());
            StartCoroutine(PerfectAvoidTime());
        }
    }

    [SerializeField]
    AnimationCurve dodgeCurve;
    [SerializeField]
    LayerMask layerMask;

    IEnumerator PerfectAvoidTime()
    {
        Player.instance.perfectAvoid = true;

        yield return new WaitForSeconds(0.008f);

        Player.instance.perfectAvoid = false;
    }

    IEnumerator Deshing()
    {
        Player.instance.OnTrueMove();
        animator.SetBool("IsMove", false);
        float tempDeshTime = Time.time;
        Vector3 playerVector = playerMovement.PlayerDirection;
        Rigidbody rigidbody = Player.instance.GetComponent<Rigidbody>();
        //Debug.Log(currentWeapon.deshRange / currentWeapon.deshTime);
        float previousCurveValue = 0;

        if (playerVector != Vector3.zero)
        {
            playerMovement.movement.FastLookAt(playerVector);
        }

        while (Time.time - tempDeshTime <= currentWeapon.deshTime)
        {
            float t = (Time.time - tempDeshTime) / currentWeapon.deshTime;
            float curveValue = dodgeCurve.Evaluate(t);
            float currentCurveValue = curveValue - previousCurveValue;
            float moveDist = currentCurveValue * currentWeapon.deshRange;
            Vector3 nextPosition = transform.position + (playerMovement.movement.renderTransform.forward * moveDist);

        // --- 레이캐스트를 이용한 터널링 방지 로직 ---
            RaycastHit hit;
            // 캐릭터의 콜라이더 크기(Radius)를 고려하여 레이를 쏩니다.
            if (Physics.SphereCast(transform.position, 0.45f, playerMovement.movement.renderTransform.forward, out hit, moveDist, layerMask))
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(playerMovement.movement.renderTransform.forward, hit.normal).normalized;

                nextPosition = rigidbody.position + (slideDirection * hit.distance);
            }

            rigidbody.MovePosition(nextPosition);
            previousCurveValue = curveValue;
            yield return new WaitForFixedUpdate();
        }

        //rb.velocity = Vector3.zero;
        animator.SetBool("IsMove", true);
        //isDeshing = false;
        Player.instance.OffTrueMove();
        Player.instance.PossPlayerMove();
        Player.instance.isInvincible = false;
        invincibleEffect.SetActive(false);
    }

    public void SetupWeapon(Weapon main, Weapon sub)
    {
        mainWeapon = main;
        subWeapon = sub;

        SetupWeaponSocet();
        
        mainWeapon.Setup(this);
        subWeapon.Setup(this);

        currentWeapon = mainWeapon;
        currentColldown = UIManager.instance.mainSkillColldown;
        currentColldown.OnImage();
        mainWeapon.EquipWeaponNoSkill();
    }

    public void StopParticle()
    {
        foreach (var temp in weaponParticles)
        {
            temp.Pause();
        }
    }

    public void PlayParticle()
    {
        foreach (var temp in weaponParticles)
        {
            temp.Play();
        }
    }

    Vector3 pos;

    IEnumerator HitPosSet()
    {
        while(true)
        {
            pos = weaponSocet.position;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public Vector3 GetWeaponRotate()
    {
        return (weaponSocet.position - pos).normalized;
    }
}
//CID2B9B237DAC59E