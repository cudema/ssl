using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerWeapon playerWeapon;
    [HideInInspector]
    public PlayerEffectHandler playerEffectHandler;
    public PlayerInputController playerInputController;
    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public BuffManager buffManager;
    [HideInInspector]
    public SearchEnemy searchEnemy;
    
    public event Action<float> ChangedHp;
    public event Action ChangedSwitchingGauge;

    [SerializeField]
    GameObject playerObject;
    [SerializeField]
    Collider hitCollider;

    [HideInInspector]
    public bool isInvincible = false;
    [HideInInspector]
    public bool perfectAvoid = false;
    [HideInInspector]
    public bool IsInputEnabled = true;
    //[HideInInspector]
    public bool IsImmune = false;

    //PL = 레벨 당 스탯
    int level;
    //체력
    [SerializeField]
    float currentHp;
    public float MaxHp
    {
        get => playerStats.stats[StatType.HP].Value;
    }
    public float CurrentHp
    {
        set
        {
            float temp = currentHp;

            currentHp = Mathf.Clamp(value, 0, playerStats.stats[StatType.HP].Value);

            ChangedHp?.Invoke(currentHp - temp);
        }
            
        get => currentHp;
    }

    //방어력
    [HideInInspector]
    public float Defense;
    //공격력
    [HideInInspector]
    public float AttackDamage;
    //스피드
    [HideInInspector]
    public float Speed;
    //치명타 확률
    [HideInInspector]
    public float CriticalRange;
    //치명타 데미지
    [HideInInspector]
    public float CriticalDamage;
    //관통력
    [HideInInspector]
    public float Penetration;

    [SerializeField]
    int maxSwitchingGauge;
    [SerializeField]
    int cooldownResetVal;
    [SerializeField]
    BattleAcceleration accelerationBuff;

    int switchingGauge;

    public int MaxSwitchingGauge
    {
        get => maxSwitchingGauge;
    }

    public int SwitchingGauge
    {
        set
        {
            switchingGauge = Mathf.Clamp(value, 0, maxSwitchingGauge);
            ChangedSwitchingGauge?.Invoke();
            if (switchingGauge >= cooldownResetVal)
            {
                UIManager.instance.SwitchingColldown.OnCollDownReset();
            }
        }
        get => switchingGauge;
    }

    //[HideInInspector]
    public bool isBattleAcceleration = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        playerStats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerInputController = GetComponent<PlayerInputController>();
        playerEffectHandler = GetComponent<PlayerEffectHandler>();
        buffManager = GetComponent<BuffManager>();
        searchEnemy = GetComponent<SearchEnemy>();
    }

    void OnEnable()
    {
        OnSwichingSkill += OnSwichingbuff;
        OffSwichingSkill += OffSwichingbuff;
    }

    void OnDisable()
    {
        OnSwichingSkill -= OnSwichingbuff;
        OffSwichingSkill -= OffSwichingbuff;
    }

    public void OnPositionSet(Vector3 vector, Quaternion rotation)
    {
        transform.position = vector;
        movement.movement.renderTransform.rotation = rotation;
        movement.CameraSet();
    }

    public void SetupPlayer()
    {
        InputManager.instance.StartControll();
        PossPlayerMove();
        playerWeapon.animator.Rebind();
        movement.SpeedSet();
        isInvincible = false;
        movement.movement.Controller.enabled = true;
    }

    public void StopPlayer()
    {
        movement.PlayerMoveable = false;
        InputManager.instance.StopControll();
        movement.movement.Controller.enabled = false;
    }

    public void OnTrueMove()
    {
        movement.movement.Controller.enabled = false;
        hitCollider.enabled = true;
    }

    public void OffTrueMove()
    {
        movement.movement.Controller.enabled = true;
        hitCollider.enabled = false;
    }

    public void SetupWeapon(Weapon mainWeapon, Weapon subWeapon)
    {
        CurrentHp = playerStats.stats[StatType.HP].Value;
        playerWeapon.SetupWeapon(mainWeapon, subWeapon);
        playerInputController.Setup();
        SetupPlayer();
    }

    //플레이어 스탯 초기화
    public void OnPlayerStatReset()
    {
        playerStats.OnStatsReset();
        SwitchingGauge = 0;
        StopPlayer();
    }

    public void OnPlayer()
    {
        playerObject.SetActive(true);
        movement.movement.Controller.enabled = true;
    }

    public void OffPlayer()
    {
        playerObject.SetActive(false);
        movement.movement.Controller.enabled = false;
    }

    public void PossPlayerMove()
    {
        movement.PlayerMoveable = true;
        playerWeapon.animator.SetBool("IsMoveable", true);
        //Debug.Log("MoveOn" + Time.time);
    }

    public void ImpossPlayerMove()
    {
        movement.PlayerMoveable = false;
        playerWeapon.animator.SetBool("IsMoveable", false);
        movement.StopMovement();
        //Debug.Log("MoveOff" + Time.time);
    }

    public Action OnSwichingSkill;
    public Action OffSwichingSkill;

    public void OnImmune()
    {
        IsImmune = true;
        OnSwichingSkill?.Invoke();
    }

    public void OffImmune()
    {
        IsImmune = false;
        OffSwichingSkill?.Invoke();
        buffManager.AddBuff(accelerationBuff);
    }

    [SerializeField]
    BuffData reductionOfDamage;

    public void OnSwichingbuff()
    {
        buffManager.AddBuff(reductionOfDamage);
    }

    public void OffSwichingbuff()
    {
        buffManager.RemoveBuff(reductionOfDamage);
    }

    public void SetStat(StatType type)
    {
        switch (type)
        {
            case StatType.AttackDamage:
                AttackDamage = playerStats.stats[type].Value + playerWeapon.currentWeapon.stats[type].Value;
                break;
            case StatType.CriticalRange:
                CriticalRange = playerStats.stats[type].Value + playerWeapon.currentWeapon.stats[type].Value;
                break;
            case StatType.Speed:
                movement.movement.SetSpeed(Player.instance.playerStats.stats[StatType.Speed].Value, movement.rotationSpeed);
                break;
//----------------------------------------제압력 추가-------------------------------------------------
            default:
                break;
        }
    }

    public WeaponAttackData GetCurrentAttackData()
    {
        return playerWeapon.playerAttack.currentAttackData;
    }
}
