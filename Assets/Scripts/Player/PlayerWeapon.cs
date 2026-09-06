using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class PlayerWeapon : MonoBehaviour
{
    enum BufferedCombatAction
    {
        Dodge,
        Skill,
        Switching,
        Attack
    }

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
    [SerializeField, Range(1f, 2f)]
    float normalAttackAnimationSpeed = 1.2f;
    [SerializeField, Range(1f, 1.5f)]
    float axeNormalAttackSpeedMultiplier = 1.1f;
    [SerializeField, Range(1f, 2f)]
    float skillAnimationSpeed = 1.15f;
    [SerializeField, Range(1f, 2f)]
    float switchingAnimationSpeed = 1.15f;
    [SerializeField, Range(1f, 2f)]
    float dodgeAnimationSpeed = 1.1f;
    [SerializeField, Range(0.05f, 0.25f)]
    float inputBufferDuration = 0.15f;
    [SerializeField, Min(0f)]
    float swordDodgeCancelDelay = 0.05f;
    [SerializeField, Min(0f)]
    float spearDodgeCancelDelay = 0.09f;
    [HideInInspector]
    public PlayerAttack playerAttack;
    PlayerMovement playerMovement;
    
    [SerializeField]
    ParticleSystem switchingEffect;

    float requestedAnimationSpeed = 1f;
    readonly float[] bufferedActionExpiresAt = new float[4];
    bool normalAttackActive;
    bool normalAttackHitboxOpened;
    float normalAttackStartedAt;
    float currentNormalAttackAnimationSpeed = 1f;

    //SearchEnemy searchEnemy;

    //Coroutine deshCoroutine;

    //bool isDeshing = false;

    List<ParticleSystem> weaponParticles = new List<ParticleSystem>();

    [HideInInspector]
    public CollDown currentColldown;

    public Action ChangedWeapon;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        for (int i = 0; i < bufferedActionExpiresAt.Length; i++)
        {
            bufferedActionExpiresAt[i] = float.NegativeInfinity;
        }
        //searchEnemy = GetComponent<SearchEnemy>();
        StartCoroutine(HitPosSet());
    }

    void Update()
    {
        TryConsumeBufferedAction();
    }

    [SerializeField]
    public int useSwitchingGauge = 100;

    public void ChangeWeapon(InputAction.CallbackContext value)
    {
        if (!CanExecuteBufferedAction(BufferedCombatAction.Switching))
        {
            BufferAction(BufferedCombatAction.Switching);
            return;
        }

        ExecuteChangeWeapon();
    }

    void ExecuteChangeWeapon()
    {
        //if (!Player.instance.movement.PlayerMoveable) return;
        if (!UIManager.instance.SwitchingColldown.OnCollDown(switchingColldown)) return;

        if (Player.instance.SwitchingGauge >= useSwitchingGauge)
        {
            CancelNormalAttack();
            playerMovement.EndNormalAttackMove();
            SetAnimationSpeed(switchingAnimationSpeed);
            Player.instance.SwitchingGauge -= useSwitchingGauge;
            currentWeapon.SwitchingSkill();
            return;
        }

        //Change();
        //-----------------------------제압력 추가------------------------------------------
    }

    public void SwitchingAttack()
    {
        Change();
        animator.Play("Switching_Last");
    }

    public void Change()
    {
        playerMovement.StopMovement();
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
        
        switchingEffect.Play();

        currentWeapon.EquipWeapon();
        ChangedWeapon?.Invoke();
        Player.instance.SetStat(StatType.AttackDamage);
        Player.instance.SetStat(StatType.CriticalRange);
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
    bool isDashing;

    public void Attack(InputAction.CallbackContext value)
    {
        if (!CanExecuteBufferedAction(BufferedCombatAction.Attack))
        {
            BufferAction(BufferedCombatAction.Attack);
            return;
        }

        ExecuteNormalAttack();
    }

    void ExecuteNormalAttack()
    {
        currentNormalAttackAnimationSpeed = GetNormalAttackAnimationSpeed();
        playerMovement.PrepareNormalAttackMove(currentNormalAttackAnimationSpeed);
        SetAnimationSpeed(currentNormalAttackAnimationSpeed);
        normalAttackActive = true;
        normalAttackHitboxOpened = false;
        normalAttackStartedAt = Time.time;

        if (attackChack != null)
        {
            StopCoroutine(attackChack);
        }

        attackChack = StartCoroutine(AttackChacking());
        currentWeapon.AttackWeapon();
    }

    IEnumerator AttackChacking()
    {
        yield return new WaitForSeconds(0.5f / Mathf.Max(currentNormalAttackAnimationSpeed, Mathf.Epsilon));
        animator.ResetTrigger("attack");
        playerMovement.EndNormalAttackMove();
        normalAttackActive = false;
        normalAttackHitboxOpened = false;
        attackChack = null;
        ResetAnimationSpeed();
    }

    void AttackReset()
    {
        animator.ResetTrigger("attack");
    }

    public void ChangeAnimator(RuntimeAnimatorController animatorController)
    {
        bool temp = animator.GetBool("IsMove");
        animator.runtimeAnimatorController = animatorController;
        animator.SetBool("IsMove", temp);
    }

    Coroutine skillChack;

    public void Skill(InputAction.CallbackContext value)
    {
        if (!CanExecuteBufferedAction(BufferedCombatAction.Skill))
        {
            BufferAction(BufferedCombatAction.Skill);
            return;
        }

        ExecuteSkill();
    }

    void ExecuteSkill()
    {

        if (skillChack != null) StopCoroutine(skillChack);
        skillChack = StartCoroutine(SkillChacking());
        if (currentWeapon.isUseableSkill)
        {
            CancelNormalAttack();
            playerMovement.EndNormalAttackMove();
            SetAnimationSpeed(skillAnimationSpeed);
            Change();
            currentWeapon.OnSkill();
        } 
    }

    IEnumerator SkillChacking()
    {
        yield return new WaitForSeconds(0.5f / Mathf.Max(skillAnimationSpeed, Mathf.Epsilon));
        animator.ResetTrigger("skill");
    }

    public void OnSkill()
    {
        currentWeapon.AttackSkill();
    }

    public void Desh(InputAction.CallbackContext value)
    {
        if (!CanExecuteBufferedAction(BufferedCombatAction.Dodge))
        {
            BufferAction(BufferedCombatAction.Dodge);
            return;
        }

        ExecuteDodge();
    }

    void ExecuteDodge()
    {
        if (!UIManager.instance.dechCollDown.OnCollDown(dashColldown)) return;

        if (normalAttackActive)
        {
            CancelNormalAttack();
        }

        isDashing = true;
        playerMovement.EndNormalAttackMove();
        SetAnimationSpeed(dodgeAnimationSpeed);
        //isDeshing = true;

        Player.instance.playerEffectHandler.OnUseEffect<UseDeshEffect>(Player.instance.searchEnemy.GetEnemy());
        animator.SetTrigger("Dash");
        Player.instance.ImpossPlayerMove();
        invincibleEffect.SetActive(true);
        /*deshCoroutine = */StartCoroutine(Deshing());
        StartCoroutine(InvincibleTime());
        StartCoroutine(PerfectAvoidTime());
    }

    [SerializeField]
    AnimationCurve dodgeCurve;
    [SerializeField]
    LayerMask layerMask;

    public float perfectAvoidTime = 0.01f;

    IEnumerator PerfectAvoidTime()
    {
        Player.instance.perfectAvoid = true;

        yield return new WaitForSeconds(perfectAvoidTime);

        Player.instance.perfectAvoid = false;
    }

    public float invincibleTime = 0.2f;

    IEnumerator InvincibleTime()
    {
        Player.instance.isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        Player.instance.isInvincible = false;
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
        invincibleEffect.SetActive(false);
        playerMovement.NotifyEvadeEnded();
        isDashing = false;
        TryConsumeBufferedAction();
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
        Player.instance.SetStat(StatType.AttackDamage);
        Player.instance.SetStat(StatType.CriticalRange);
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

    void SetAnimationSpeed(float speed)
    {
        requestedAnimationSpeed = speed;
        animator.speed = requestedAnimationSpeed;
    }

    public void PauseAnimation()
    {
        animator.speed = 0f;
    }

    public void ResumeAnimation()
    {
        animator.speed = requestedAnimationSpeed;
    }

    public void ResetAnimationSpeed()
    {
        requestedAnimationSpeed = 1f;
        animator.speed = requestedAnimationSpeed;
    }

    float GetNormalAttackAnimationSpeed()
    {
        float weaponMultiplier = currentWeapon != null && currentWeapon.weaponType == WeaponType.Axe
            ? axeNormalAttackSpeedMultiplier
            : 1f;
        return normalAttackAnimationSpeed * weaponMultiplier;
    }

    public void NotifyNormalAttackHitboxOpened()
    {
        if (normalAttackActive)
        {
            normalAttackHitboxOpened = true;
        }
    }

    public void NotifyMovementUnlocked()
    {
        bool wasNormalAttack = normalAttackActive;
        normalAttackActive = false;
        normalAttackHitboxOpened = false;

        if (!wasNormalAttack)
        {
            ResetAnimationSpeed();
        }

        TryConsumeBufferedAction();
    }

    void CancelNormalAttack()
    {
        if (attackChack != null)
        {
            StopCoroutine(attackChack);
            attackChack = null;
        }

        animator.ResetTrigger("attack");
        playerAttack.OffAttack();
        playerMovement.EndNormalAttackMove();
        playerMovement.StopMovement();
        normalAttackActive = false;
        normalAttackHitboxOpened = false;
    }

    bool CanDodgeCancelNormalAttack()
    {
        if (!normalAttackActive) return true;
        if (currentWeapon == null) return false;

        if (currentWeapon.weaponType == WeaponType.Axe)
        {
            return normalAttackHitboxOpened;
        }

        float baseDelay = currentWeapon.weaponType == WeaponType.Sword
            ? swordDodgeCancelDelay
            : spearDodgeCancelDelay;
        float adjustedDelay = baseDelay / Mathf.Max(currentNormalAttackAnimationSpeed, Mathf.Epsilon);
        return Time.time - normalAttackStartedAt >= adjustedDelay;
    }

    void BufferAction(BufferedCombatAction action)
    {
        bufferedActionExpiresAt[(int)action] = Time.unscaledTime + inputBufferDuration;
    }

    bool HasBufferedAction(BufferedCombatAction action)
    {
        return bufferedActionExpiresAt[(int)action] >= Time.unscaledTime;
    }

    void ClearBufferedAction(BufferedCombatAction action)
    {
        bufferedActionExpiresAt[(int)action] = float.NegativeInfinity;
    }

    void ClearAllBufferedActions()
    {
        for (int i = 0; i < bufferedActionExpiresAt.Length; i++)
        {
            bufferedActionExpiresAt[i] = float.NegativeInfinity;
        }
    }

    bool CanExecuteBufferedAction(BufferedCombatAction action)
    {
        if (Player.instance == null || !Player.instance.IsInputEnabled) return false;

        switch (action)
        {
            case BufferedCombatAction.Dodge:
                return !isDashing && CanDodgeCancelNormalAttack();
            case BufferedCombatAction.Attack:
                return !isDashing && !normalAttackActive;
            case BufferedCombatAction.Skill:
            case BufferedCombatAction.Switching:
                return !isDashing;
            default:
                return false;
        }
    }

    void TryConsumeBufferedAction()
    {
        // 회피 > 스킬 > 교체 > 일반 공격 순으로 한 프레임에 하나만 소비한다.
        for (int i = 0; i < bufferedActionExpiresAt.Length; i++)
        {
            BufferedCombatAction action = (BufferedCombatAction)i;
            if (!HasBufferedAction(action) || !CanExecuteBufferedAction(action)) continue;

            ClearAllBufferedActions();
            switch (action)
            {
                case BufferedCombatAction.Dodge:
                    ExecuteDodge();
                    break;
                case BufferedCombatAction.Skill:
                    ExecuteSkill();
                    break;
                case BufferedCombatAction.Switching:
                    ExecuteChangeWeapon();
                    break;
                case BufferedCombatAction.Attack:
                    ExecuteNormalAttack();
                    break;
            }
            return;
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

    public void SetAttackType(int attackType)
    {
        switch (attackType)
        {
            case 0:
                currentWeapon.SetAttackType(AttackType.Nomal);
                break;
            case 1:
                currentWeapon.SetAttackType(AttackType.Skill);
                Player.instance.playerEffectHandler.OnUseEffect<StartSkillEffect>(Player.instance.searchEnemy.GetEnemy());
                break;
            case 2:
                currentWeapon.SetAttackType(AttackType.Switching);
                Player.instance.playerEffectHandler.OnUseEffect<StartSwichingSkillEffect>(Player.instance.searchEnemy.GetEnemy());
                break;
        }
    }

    public void EndSkillAttack(int attackType)
    {
        switch (attackType)
        {
            case 0:
                break;
            case 1:
                Player.instance.playerEffectHandler.OnUseEffect<EndSkillEffect>(Player.instance.searchEnemy.GetEnemy());
                break;
            case 2:
                Player.instance.playerEffectHandler.OnUseEffect<EndSwichingSkillEffect>(Player.instance.searchEnemy.GetEnemy());
                break;
        }
    }
}
//CID2B9B237DAC59E
