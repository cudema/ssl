using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    PlayerMovement movement;
    [HideInInspector]
    public PlayerWeapon playerWeapon;
    [HideInInspector]
    public PlayerEffectHandler playerEffectHandler;
    public PlayerInputController playerInputController;
    PlayerStats playerStats;

    public event Action<float> ChangedHp;
    public event Action ChangedSwitchingGauge;

    [SerializeField]
    GameObject playerObject;

    [HideInInspector]
    public bool isInvincible = false;
    [HideInInspector]
    public bool IsInputEnabled = true;
    [HideInInspector]
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
    }

    public void OnPositionSet(Vector3 vector)
    {
        transform.position = vector;
        movement.CameraSet();
    }

    public void SetupPlayer()
    {
        movement.PlayerMoveable = true;
        InputManager.instance.StartControll();
    }

    public void StopPlayer()
    {
        movement.PlayerMoveable = false;
        InputManager.instance.StopControll();
    }

    public void SetupWeapon(Weapon mainWeapon, Weapon subWeapon)
    {
        currentHp = playerStats.stats[StatType.HP].Value;
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
    }

    public void OffPlayer()
    {
        playerObject.SetActive(false);
    }

    public void PossPlayerMove()
    {
        movement.PlayerMoveable = true;
        //Debug.Log("MoveOn" + Time.time);
    }

    public void ImpossPlayerMove()
    {
        movement.PlayerMoveable = false;
        //Debug.Log("MoveOff" + Time.time);
    }

    public void OnImmune()
    {
        IsImmune = true;
    }

    public void OffImmune()
    {
        IsImmune = false;
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
//----------------------------------------제압력 추가-------------------------------------------------
            default:
                break;
        }
    }
}
