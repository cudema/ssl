using System;
using System.Collections;
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

    [SerializeField]
    Camera mainCamera;

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

    [HideInInspector]
    public BattleAcceleration useAccelBuff;

    int switchingGauge;

    public int MaxSwitchingGauge
    {
        get => maxSwitchingGauge;
    }

    public int addSwitchingGaugeToAccel;

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

    CameraTrigger cameraTrigger;

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
        useAccelBuff = accelerationBuff;
        cameraTrigger = mainCamera.GetComponent<CameraTrigger>();
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

    public void OffCamera()
    {
        mainCamera.enabled = false;
    }

    public void OnCamera()
    {
        mainCamera.enabled = true;
    }

    Vector3 originalPosition;
    Coroutine shakeCoroutine;
    
    /// <summary>
    /// 펄린 노이즈 기반의 부드러운 화면 흔들림을 실행합니다.
    /// </summary>
    /// <param name="duration">지속 시간 (초)</param>
    /// <param name="magnitude">최대 진폭</param>
    public void Shake(float duration = 0.1f, float magnitude = 0.1f)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.localPosition = originalPosition; // 이전 위치 복구
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0.0f;

        // 노이즈 시작 지점을 무작위로 설정하여 매번 다른 패턴 생성
        float seedX = UnityEngine.Random.value * 100f;
        float seedY = UnityEngine.Random.value * 100f;
        cameraTrigger.isMove = false;

        while (elapsed < duration)
        {
            // 펄린 노이즈 값 범위는 [0, 1]이므로 [-1, 1]로 매핑
            float noiseX = (Mathf.PerlinNoise(seedX, Time.time * 30) * 2.0f) - 1.0f;
            float noiseY = (Mathf.PerlinNoise(seedY, Time.time * 30) * 2.0f) - 1.0f;

            Vector2 offset = new Vector2(noiseX, noiseY);

            // 대각선 거리 왜곡 방지를 위해 크기를 1로 제한
            if (offset.sqrMagnitude > 1.0f)
            {
                offset.Normalize();
            }

            Vector3 finalOffset = offset * (magnitude * ((elapsed / duration - 1) * (elapsed / duration - 1)));
            mainCamera.transform.localPosition = originalPosition + (Vector3)finalOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraTrigger.isMove = true;

        mainCamera.transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }

    public void OnPositionSet(Vector3 vector, Quaternion rotation)
    {
        transform.position = vector;
        movement.movement.renderTransform.rotation = rotation;
        movement.CameraSet();
    }

    public void SetupPlayer()
    {
        PossPlayerMove();
        playerWeapon.animator.Rebind();
        movement.SpeedSet();
        isInvincible = false;
        IsInputEnabled = true;
        //movement.movement.Controller.enabled = true;
    }

    public void StopPlayer()
    {
        movement.PlayerMoveable = false;
        InputManager.instance.StopControll();
        //movement.movement.Controller.enabled = false;
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
        UIManager.instance.weaponUI.SetWeapon(mainWeapon.weaponIndex, subWeapon.weaponIndex);
        SetupPlayer();
    }

    //플레이어 스탯 초기화
    public void OnPlayerStatReset()
    {
        playerStats.OnStatsReset();
        SwitchingGauge = 0;
        StopPlayer();
        movement.movement.Controller.enabled = false;
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
        playerWeapon.NotifyMovementUnlocked();
        playerWeapon.animator.SetBool("IsMoveable", true);
        //Debug.Log("MoveOn" + Time.time);
    }

    public void ImpossPlayerMove()
    {
        movement.PlayerMoveable = false;
        playerWeapon.animator.SetBool("IsMoveable", false);
        movement.StopMovementForActionLock();
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
        buffManager.AddBuff(useAccelBuff);
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
                movement.SetMovementSpeed(Player.instance.playerStats.stats[StatType.Speed].Value);
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
