using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    PlayerMovement movement;
    PlayerWeapon playerWeapon;
    PlayerInputController playerInputController;

    public event Action ChangedHp;

    //PL = 레벨 당 스탯
    int level;
    //체력
    [SerializeField]
    float hpBasic;
    [SerializeField]
    float hpPL;
    float hpBonus;
    public float HpBonus
    {
        set
        {
            hpBonus = value;
            ChangedHp?.Invoke();
        }
        get => hpBonus;
    }
    [SerializeField]
    float currentHp;
    public float MaxHp
    {
        get => hpBasic + hpPL * level + HpBonus;
    }
    public float CurrentHp
    {
        set
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            ChangedHp?.Invoke();
        }
            
        get => currentHp;
    }
    //방어력
    [SerializeField]
    float defenseBasic;
    [SerializeField]
    float defensePL;
    //float defenseBonus;
    [HideInInspector]
    public float DefenseBonus = 0;
    public float Defense
    {
        get => Mathf.Clamp(defenseBasic + DefenseBonus + defensePL * level, 0, 100);
    }
    //공격력
    [SerializeField]
    float attackDamageBasic;
    [SerializeField]
    float attackDamagePL;
    //float attackDamageBonus;
    [HideInInspector]
    public float AttackDamageBonus = 0;
    public float AttackDamage
    {
        get => attackDamageBasic + attackDamagePL * level + AttackDamageBonus;
    }
    //스피드
    [SerializeField]
    float speedBasic;
    [HideInInspector]
    public float SpeedBonus = 0;
    public float Speed
    {
        get => speedBasic + SpeedBonus;
    }
    //치명타 확률
    [SerializeField]
    float criticalRangeBasic;
    [HideInInspector]
    public float CriticalRangeBonus = 0;
    public float CriticalRange
    {
        get => criticalRangeBasic + CriticalRangeBonus;
    }
    //치명타 데미지
    [SerializeField]
    float criticalDamageBasic;
    [HideInInspector]
    public float CriticalDamageBonus = 0;
    public float CriticalDamage
    {
        get => criticalDamageBasic + CriticalDamageBonus;
    }
    //관통력
    [SerializeField]
    float penetrationBasic;
    [HideInInspector]
    public float PenetrationBonus = 0;
    public float Penetration
    {
        get => Mathf.Clamp(penetrationBasic + PenetrationBonus, 0, 100);
    }

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

        movement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerInputController = GetComponent<PlayerInputController>();
    }

    public void OnPositionSet(Vector3 vector)
    {
        transform.position = vector;
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
        currentHp = MaxHp;
        playerWeapon.SetupWeapon(mainWeapon, subWeapon);
        playerInputController.Setup();
        SetupPlayer();
    }

    //플레이어 스탯 초기화
    public void OnPlayerStatReset()
    {
        HpBonus = 0;
        SpeedBonus = 0;
        DefenseBonus = 0;
        PenetrationBonus = 0;
        AttackDamageBonus = 0;
        CriticalRangeBonus = 0;
        CriticalDamageBonus = 0;
        StopPlayer();
    }
}
