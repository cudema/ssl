using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff System/BattleAcceleration")]
public class BattleAcceleration : BuffData
{
    public int addGauge;

    public override void OnBuffEffect(BuffManager buffManager)
    {
        base.OnBuffEffect(buffManager);
        Player.instance.isBattleAcceleration = true;
        Player.instance.addSwitchingGaugeToAccel += addGauge;
        PlayerSwitchingBuffAura aura = Player.instance.GetComponent<PlayerSwitchingBuffAura>();
        if (aura == null) aura = Player.instance.gameObject.AddComponent<PlayerSwitchingBuffAura>();
        aura.Play();
        //Player.instance.SwitchingGauge = 0;
    }

    public override void OffBuffEffect(BuffManager buffManager)
    {
        base.OffBuffEffect(buffManager);
        Player.instance.isBattleAcceleration = false;
        Player.instance.addSwitchingGaugeToAccel -= addGauge;
        PlayerSwitchingBuffAura aura = Player.instance.GetComponent<PlayerSwitchingBuffAura>();
        if (aura != null) aura.StopAura();
    }
}

[DisallowMultipleComponent]
public sealed class PlayerSwitchingBuffAura : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    ParticleSystem aura;
    Material auraMaterial;
    readonly Dictionary<int, ParticleSystem> traitAuras = new Dictionary<int, ParticleSystem>();
    readonly HashSet<int> activeTraits = new HashSet<int>();
    readonly List<Material> traitMaterials = new List<Material>();

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
        foreach (Material material in traitMaterials)
        {
            if (material != null) Destroy(material);
        }
    }

    public void Play()
    {
        if (aura == null || aura.isPlaying) return;
        aura.Clear(true);
        aura.Play(true);
        foreach (int effectId in activeTraits)
        {
            ParticleSystem traitAura = GetOrCreateTraitAura(effectId);
            if (traitAura == null) continue;
            traitAura.Clear(true);
            traitAura.Play(true);
        }
    }

    public void StopAura()
    {
        if (aura != null)
        {
            aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        foreach (ParticleSystem traitAura in traitAuras.Values)
        {
            if (traitAura != null)
            {
                traitAura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    public void SetTraitActive(int effectId, bool active)
    {
        if (!TryGetTraitPalette(effectId, out _, out _)) return;

        if (active)
        {
            activeTraits.Add(effectId);
            ParticleSystem traitAura = GetOrCreateTraitAura(effectId);
            if (traitAura != null && aura != null && aura.isPlaying && !traitAura.isPlaying)
            {
                traitAura.Clear(true);
                traitAura.Play(true);
            }
        }
        else
        {
            activeTraits.Remove(effectId);
            if (traitAuras.TryGetValue(effectId, out ParticleSystem traitAura) && traitAura != null)
            {
                traitAura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
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
            name = "Player Switching Buff Aura (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        auraMaterial.SetColor(BaseColorId, new Color(0.94f, 0.98f, 1f, 0.58f));

        GameObject auraObject = new GameObject("Switching Buff White Aura");
        auraObject.layer = gameObject.layer;
        auraObject.transform.SetParent(transform, false);
        auraObject.transform.localPosition = localCenter;

        aura = auraObject.AddComponent<ParticleSystem>();
        aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = aura.main;
        main.loop = true;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.65f, 1.1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0f);
        main.startSize = new ParticleSystem.MinMaxCurve(characterHeight * 0.032f, characterHeight * 0.072f);
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(1f, 1f, 1f, 0.38f),
            new Color(0.78f, 0.9f, 1f, 0.24f));
        main.maxParticles = 80;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emission = aura.emission;
        emission.rateOverTime = 32f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10) });

        ParticleSystem.ShapeModule shape = aura.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1f;
        shape.radiusThickness = 0.18f;
        shape.scale = new Vector3(
            Mathf.Max(0.28f, bounds.extents.x * 0.85f),
            Mathf.Max(0.65f, bounds.extents.y * 0.82f),
            Mathf.Max(0.28f, bounds.extents.z * 0.85f));

        ParticleSystem.VelocityOverLifetimeModule velocity = aura.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.x = new ParticleSystem.MinMaxCurve(0f);
        velocity.y = new ParticleSystem.MinMaxCurve(characterHeight * 0.34f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f);

        ParticleSystem.NoiseModule noise = aura.noise;
        noise.enabled = true;
        noise.separateAxes = false;
        noise.strength = characterHeight * 0.07f;
        noise.frequency = 1.35f;
        noise.scrollSpeed = 0.45f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = aura.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.88f, 0.94f, 1f), 0f),
                new GradientColorKey(Color.white, 0.55f),
                new GradientColorKey(new Color(0.72f, 0.84f, 1f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.42f, 0.18f),
                new GradientAlphaKey(0.3f, 0.7f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = aura.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.35f),
            new Keyframe(0.3f, 1f),
            new Keyframe(1f, 0.2f)));

        ParticleSystemRenderer particleRenderer = auraObject.GetComponent<ParticleSystemRenderer>();
        particleRenderer.sharedMaterial = auraMaterial;
        particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        particleRenderer.shadowCastingMode = ShadowCastingMode.Off;
        particleRenderer.receiveShadows = false;
        particleRenderer.lightProbeUsage = LightProbeUsage.Off;
        particleRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

        aura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    ParticleSystem GetOrCreateTraitAura(int effectId)
    {
        if (traitAuras.TryGetValue(effectId, out ParticleSystem existing)) return existing;
        if (aura == null || !TryGetTraitPalette(effectId, out Color primary, out Color secondary)) return null;

        GameObject traitObject = Instantiate(aura.gameObject, transform, false);
        traitObject.name = $"Switching Trait Aura {effectId}";
        ParticleSystem traitAura = traitObject.GetComponent<ParticleSystem>();
        traitAura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = traitAura.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.55f, 0.95f);
        main.startSize = new ParticleSystem.MinMaxCurve(
            aura.main.startSize.constantMin * 0.72f,
            aura.main.startSize.constantMax * 0.82f);
        primary.a = 0.34f;
        secondary.a = 0.24f;
        main.startColor = new ParticleSystem.MinMaxGradient(primary, secondary);
        main.maxParticles = 24;

        ParticleSystem.EmissionModule emission = traitAura.emission;
        emission.rateOverTime = 9f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 3) });

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = traitAura.colorOverLifetime;
        Gradient traitGradient = new Gradient();
        traitGradient.SetKeys(
            new[]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.36f, 0.2f),
                new GradientAlphaKey(0.22f, 0.72f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = traitGradient;

        Material material = new Material(auraMaterial.shader)
        {
            name = $"Switching Trait Aura {effectId} (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        material.SetColor(BaseColorId, new Color(0.96f, 0.98f, 1f, 0.52f));
        traitObject.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;
        traitMaterials.Add(material);

        traitAuras.Add(effectId, traitAura);
        return traitAura;
    }

    static bool TryGetTraitPalette(int effectId, out Color primary, out Color secondary)
    {
        switch (effectId)
        {
            case 2401: // 특수 효율 - 민트
                primary = new Color(0.48f, 1f, 0.82f);
                secondary = new Color(0.68f, 0.9f, 1f);
                return true;
            case 2402: // 전투 지속력 - 하늘색
                primary = new Color(0.5f, 0.74f, 1f);
                secondary = new Color(0.72f, 0.84f, 1f);
                return true;
            case 2404: // 전투 가속 - 옅은 금색
                primary = new Color(1f, 0.82f, 0.46f);
                secondary = new Color(1f, 0.68f, 0.48f);
                return true;
            case 2408: // 교체의 반동 - 연분홍/보라
                primary = new Color(1f, 0.55f, 0.72f);
                secondary = new Color(0.72f, 0.58f, 1f);
                return true;
            default:
                primary = Color.white;
                secondary = Color.white;
                return false;
        }
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
