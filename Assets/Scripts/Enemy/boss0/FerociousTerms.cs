using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum Boss0Patten
{
    None = -1,
    BearSlash = 0,
    GroundSmash,
    FinalStrike,
    PhantomCharge,
    GroundBomb
}

public class FerociousTerms : EnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;    
    [SerializeField]
    DenggerEffectBase squareDengger;
    bool IsPatternLocked = false;

    Boss0Patten lastUsedPatten = Boss0Patten.None;
    Coroutine currentPatten = null;
    BossPhaseAura phaseAura;

    public bool isPattern = false;

    List<int> ranges = new List<int>();

    void Start()
    {
        phaseAura = GetComponent<BossPhaseAura>();
        if (phaseAura == null) phaseAura = gameObject.AddComponent<BossPhaseAura>();

        enemyStates[2] = new Boss0Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
        currentState = enemyStates[0];
        phantomChargeChackCooldown = Time.time;
        hp = stats.stats[StatType.HP].Value;

        for (int i = 0; i < range.Length; i++)
        {
            for (int j = 0; j < range[i]; j++)
            {
                ranges.Add(i);
            }
        }
    } 

    protected override void ChangedHP()
    {
        base.ChangedHP();

        if (!IsPatternLocked && hp < stats.stats[StatType.HP].Value * 0.3)
        {
            IsPatternLocked = true;
            animator.speed = 2f;
            timeScale = 2f;
            phaseAura.Play();
        }
    }

    [Header("BearSlash")]
    // [SerializeField]
    // float bearSlashStartingRange = 6.0f;
    [SerializeField]
    float bearSlashAttackRange = 2.5f;
    [SerializeField]
    Collider bearSlashAttackCollider;
    [SerializeField]
    float bearSlashRecoveryTime = 0.1f;

    IEnumerator BearSlash()
    {
        lastUsedPatten = Boss0Patten.BearSlash;
        Vector3 dir = Player.instance.transform.position - transform.position;

        while (dir.magnitude > bearSlashAttackRange)
        {
            animator.SetBool("isMove", true);
            dir = Player.instance.transform.position - transform.position;
            movement.ToMove(dir.normalized);

            yield return null;
        }
        
        animator.SetBool("isMove", false);
        PlayAttackAnimation("BearSlash");
        isLookAtPlayer = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(51f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        isLookAtPlayer = true;

        yield return StartCoroutine(WaitForSecondsOfPertten(46f / 60f));

        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(30f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        isLookAtPlayer = true;

        yield return StartCoroutine(WaitForSecondsOfPertten(29f / 60f));

        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(30f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(65f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("GroundSmash")]
    // [SerializeField]
    // float groundSmashStartingRange = 4.0f;
    [SerializeField]
    Collider groundSmashAttackCollider;
    [SerializeField]
    float groundSmashRecoveryTime = 0.1f;

    IEnumerator GroundSmash()
    {
        PlayAttackAnimation("GroundSmash");
        lastUsedPatten = Boss0Patten.GroundSmash;
     
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        isLookAtPlayer = false;
        circleDengger.Setup(groundSmashAttackCollider.transform.position + Vector3.down, 1.8f, 50f / 60f / timeScale);
        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));

        groundSmashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundSmashAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(140f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(groundSmashRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("GroundBomb")]
    // [SerializeField]
    // float groundBombStartingRange = 4.0f;
    [SerializeField]
    Collider groundBombAttackCollider;
    [SerializeField]
    float groundBombRecoveryTime = 0.1f;

    IEnumerator GroundBomb()
    {
        PlayAttackAnimation("GroundBomb");
        lastUsedPatten = Boss0Patten.GroundBomb;
     
        OnAttackMove(30f, -2.5f, false);

        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));
        isLookAtPlayer = false;
        circleDengger.Setup(groundBombAttackCollider.transform.position + Vector3.down, 2.5f, 85f / 60f / timeScale);
        yield return StartCoroutine(WaitForSecondsOfPertten(85f / 60f));

        groundBombAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundBombAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(25f / 60f));

        circleDengger.Setup(groundBombAttackCollider.transform.position + Vector3.down, 2.5f, 59f / 60f / timeScale);

        yield return StartCoroutine(WaitForSecondsOfPertten(59f / 60f));

        OnAttackMove(30f, 2f, false);

        groundBombAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundBombAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(51f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(groundBombRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("CrushCharge")]
    // [SerializeField]
    // float crushChargeStartingRange = 4.0f;
    [SerializeField]
    Collider crushChargeAttack0Collider;
    [SerializeField]
    Collider crushChargeAttack1Collider;
    [SerializeField]
    float crushChargeRecoveryTime = 0.1f;

    IEnumerator CrushCharge()
    {
        PlayAttackAnimation("CrushCharge");
        lastUsedPatten = Boss0Patten.GroundBomb;
     
        isLookAtPlayer = false;
        OnAttackMove(30f, 5.8f, false);

        yield return StartCoroutine(WaitForSecondsOfPertten(32f / 60f));

        crushChargeAttack0Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        OnAttackMove(30f, 1.02f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        crushChargeAttack0Collider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        OnAttackMove(30f, 0.55f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(46f / 60f));

        OnAttackMove(30f, 5f, false);
        crushChargeAttack1Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        crushChargeAttack1Collider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(82f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(crushChargeRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("FinalStrike")]
    // [SerializeField]
    // float finalStrikeStartingRange = 7.0f;
    [SerializeField]
    Collider finalStrikeAttackCollider;
    [SerializeField]
    float finalStrikeRecoveryTime = 3.0f;

    IEnumerator FinalStrike()
    {
        PlayAttackAnimation("FinalStrike");
        lastUsedPatten = Boss0Patten.FinalStrike;
        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(98f / 60f));

        finalStrikeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        finalStrikeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(148f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(finalStrikeRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("PhantomCharge")]
    // [SerializeField]
    // float phantomChargeStartingRange = 8.0f;
    [SerializeField]
    Collider phantomChargeAttackCollider;
    // [SerializeField]
    // float phantomChargeStartupTime = 1.5f;
    // [SerializeField]
    // float phantomChargeActiveTime = 0.7f;
    [SerializeField]
    float phantomChargeRecoveryTime = 2.5f;
    [SerializeField]
    float phantomChargeCooldown = 15.0f;
    [SerializeField]
    float phantomChargeSpeed = 12f;
    // [SerializeField]
    // float phantomChargeRushTime = 1.5f;
    [SerializeField]
    GameObject agoPrefab;

    float phantomChargeChackCooldown;

    IEnumerator PhantomCharge()
    {
        lastUsedPatten = Boss0Patten.PhantomCharge;
        AlterEgo ego0 = Instantiate(agoPrefab).GetComponent<AlterEgo>();
        AlterEgo ego1 = Instantiate(agoPrefab).GetComponent<AlterEgo>();

        ego0.Setup(phantomChargeSpeed, this);
        ego1.Setup(phantomChargeSpeed, this);

        isLookAtPlayer = false;

        Vector3 target = transform.position - Player.instance.transform.position;
        target.y = 0;

        Vector3 temp = Quaternion.Euler(new Vector3(0, 30, 0)) * target;

        ego0.transform.position = Player.instance.transform.position + temp;

        temp = Quaternion.Euler(new Vector3(0, -30, 0)) * target;

        ego1.transform.position = Player.instance.transform.position + temp;

        animator.SetTrigger("Ready");

        int i = 0;
        while (i < 80)
        {
            ego0.transform.LookAt(Player.instance.transform.position);
            ego1.transform.LookAt(Player.instance.transform.position);
            i++;
            yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        }


        yield return StartCoroutine(WaitForSecondsOfPertten(13f / 60f));

        ego0.OnGo();
        ego1.OnGo();

        yield return StartCoroutine(WaitForSecondsOfPertten(30f / 60f));

        animator.SetTrigger("End");

        Destroy(ego0);
        Destroy(ego1);

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime));

        phantomChargeChackCooldown = Time.time;

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [SerializeField]
    int[] range;

    public bool ChackPatten()
    {
        if (currentPatten != null)
        {
            //Debug.Log("IsPatten");
            return false;
        }
        if (isPattern) return true;

        float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, transform.position);

        if (lastUsedPatten == Boss0Patten.FinalStrike)
        {
            //베어가르기 사용
            currentPatten = StartCoroutine(BearSlash());
            StopMoveAnimation();
            return false;
        }
        
        if (distanceToPlayer > 6 && phantomChargeCooldown < Time.time - phantomChargeChackCooldown)
        {
             //영혼돌진 사용
             currentPatten = StartCoroutine(PhantomCharge());
             StopMoveAnimation();
             return false;
        }
        int temp = Random.Range(0, ranges.Count);

        switch (ranges[temp])
        {
            case 0:
                currentPatten = StartCoroutine(GroundSmash());
                break;
            case 1:
                currentPatten = StartCoroutine(GroundBomb());
                break;
            case 2:
                currentPatten = StartCoroutine(CrushCharge());
                break;
            case 3:
                currentPatten = StartCoroutine(FinalStrike());
                break;
            case 4:
                currentPatten = StartCoroutine(BearSlash());
                break;
        }
    
        StopMoveAnimation();
        return false;
    }

    protected override void OnDead()
    {
        if (phaseAura != null) phaseAura.StopAura();
        base.OnDead();
        animator.speed = 1f;
        timeScale = 1f;
    }
}

[DisallowMultipleComponent]
public sealed class BossPhaseAura : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    ParticleSystem aura;
    Material auraMaterial;

    void Awake()
    {
        CreateAura();
    }

    void OnDisable()
    {
        StopAura();
    }

    void OnDestroy()
    {
        if (auraMaterial != null) Destroy(auraMaterial);
    }

    public void Play()
    {
        if (aura == null || aura.isPlaying) return;
        aura.Clear(true);
        aura.Play(true);
    }

    public void StopAura()
    {
        if (aura != null)
        {
            aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void CreateAura()
    {
        Shader shader = Shader.Find("Hidden/SSL/EnemyAttackHandGlow");
        if (shader == null)
        {
            enabled = false;
            return;
        }

        Bounds bounds = CalculateCharacterBounds();
        Vector3 localCenter = transform.InverseTransformPoint(bounds.center);
        float characterHeight = Mathf.Max(1f, bounds.size.y);

        auraMaterial = new Material(shader)
        {
            name = "Boss Phase Red Aura (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        auraMaterial.SetColor(BaseColorId, new Color(1f, 0.035f, 0.015f, 0.78f));

        GameObject auraObject = new GameObject("Phase 2 Red Aura");
        auraObject.layer = gameObject.layer;
        auraObject.transform.SetParent(transform, false);
        auraObject.transform.localPosition = localCenter;

        aura = auraObject.AddComponent<ParticleSystem>();
        aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = aura.main;
        main.loop = true;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.45f, 0.9f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(characterHeight * 0.025f, characterHeight * 0.11f);
        main.startSize = new ParticleSystem.MinMaxCurve(characterHeight * 0.045f, characterHeight * 0.105f);
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(1f, 0.08f, 0.025f, 0.68f),
            new Color(0.48f, 0.005f, 0.005f, 0.42f));
        main.maxParticles = 150;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emission = aura.emission;
        emission.rateOverTime = 72f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 18) });

        ParticleSystem.ShapeModule shape = aura.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1f;
        shape.radiusThickness = 0.22f;
        shape.scale = new Vector3(
            Mathf.Max(0.35f, bounds.extents.x * 1.15f),
            Mathf.Max(0.7f, bounds.extents.y * 0.95f),
            Mathf.Max(0.35f, bounds.extents.z * 1.15f));

        ParticleSystem.VelocityOverLifetimeModule velocity = aura.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.x = new ParticleSystem.MinMaxCurve(0f);
        velocity.y = new ParticleSystem.MinMaxCurve(characterHeight * 0.21f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f);

        ParticleSystem.NoiseModule noise = aura.noise;
        noise.enabled = true;
        noise.separateAxes = true;
        noise.strengthX = characterHeight * 0.1f;
        noise.strengthY = characterHeight * 0.04f;
        noise.strengthZ = characterHeight * 0.1f;
        noise.frequency = 1.8f;
        noise.scrollSpeed = 0.7f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = aura.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(1f, 0.22f, 0.06f), 0f),
                new GradientColorKey(new Color(0.78f, 0.015f, 0.01f), 0.55f),
                new GradientColorKey(new Color(0.3f, 0f, 0f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.78f, 0.15f),
                new GradientAlphaKey(0.5f, 0.7f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = aura.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.25f),
            new Keyframe(0.25f, 1f),
            new Keyframe(1f, 0.15f)));

        ParticleSystemRenderer particleRenderer = auraObject.GetComponent<ParticleSystemRenderer>();
        particleRenderer.sharedMaterial = auraMaterial;
        particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        particleRenderer.shadowCastingMode = ShadowCastingMode.Off;
        particleRenderer.receiveShadows = false;
        particleRenderer.lightProbeUsage = LightProbeUsage.Off;
        particleRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

        aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    Bounds CalculateCharacterBounds()
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        Bounds bounds = new Bounds(transform.position + transform.up, Vector3.one * 2f);
        bool initialized = false;

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (!initialized)
            {
                bounds = renderer.bounds;
                initialized = true;
            }
            else
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }
        return bounds;
    }
}
