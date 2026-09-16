using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[DisallowMultipleComponent]
public sealed class PlayerHealingAura : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    ParticleSystem bodyAura;
    ParticleSystem groundBurst;
    Material healingMaterial;
    Coroutine playCoroutine;

    public void Play()
    {
        if (bodyAura == null || groundBurst == null)
        {
            CreateEffect();
        }
        if (bodyAura == null || groundBurst == null) return;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }

        bodyAura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        groundBurst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        bodyAura.Play(true);
        groundBurst.Play(true);
        playCoroutine = StartCoroutine(PlayForDuration());
    }

    IEnumerator PlayForDuration()
    {
        yield return new WaitForSeconds(1.3f);
        bodyAura.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        playCoroutine = null;
    }

    void OnDisable()
    {
        if (bodyAura != null)
        {
            bodyAura.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (groundBurst != null)
        {
            groundBurst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        playCoroutine = null;
    }

    void OnDestroy()
    {
        if (healingMaterial != null)
        {
            Destroy(healingMaterial);
        }
    }

    void CreateEffect()
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
        float characterRadius = Mathf.Max(0.35f, Mathf.Max(bounds.extents.x, bounds.extents.z));

        healingMaterial = new Material(shader)
        {
            name = "Player Campfire Healing Aura (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        healingMaterial.SetColor(BaseColorId, new Color(0.22f, 1f, 0.38f, 0.75f));

        GameObject auraObject = new GameObject("Campfire Healing Body Aura");
        auraObject.layer = gameObject.layer;
        auraObject.transform.SetParent(transform, false);
        auraObject.transform.localPosition = localCenter;
        bodyAura = auraObject.AddComponent<ParticleSystem>();
        ConfigureBodyAura(bodyAura, characterHeight, characterRadius);

        GameObject burstObject = new GameObject("Campfire Healing Ground Burst");
        burstObject.layer = gameObject.layer;
        burstObject.transform.SetParent(transform, false);
        burstObject.transform.localPosition = localCenter - Vector3.up * bounds.extents.y * 0.88f;
        groundBurst = burstObject.AddComponent<ParticleSystem>();
        ConfigureGroundBurst(groundBurst, characterHeight, characterRadius);
    }

    void ConfigureBodyAura(ParticleSystem particle, float characterHeight, float characterRadius)
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particle.main;
        main.loop = true;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.55f, 0.95f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0f);
        main.startSize = new ParticleSystem.MinMaxCurve(characterHeight * 0.028f, characterHeight * 0.075f);
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(0.65f, 1f, 0.72f, 0.7f),
            new Color(0.12f, 0.78f, 0.3f, 0.42f));
        main.maxParticles = 90;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emission = particle.emission;
        emission.rateOverTime = 44f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 16) });

        ParticleSystem.ShapeModule shape = particle.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1f;
        shape.radiusThickness = 0.22f;
        shape.scale = new Vector3(
            characterRadius * 0.82f,
            characterHeight * 0.43f,
            characterRadius * 0.82f);

        ParticleSystem.VelocityOverLifetimeModule velocity = particle.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.x = new ParticleSystem.MinMaxCurve(0f);
        velocity.y = new ParticleSystem.MinMaxCurve(characterHeight * 0.42f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f);

        ParticleSystem.NoiseModule noise = particle.noise;
        noise.enabled = true;
        noise.separateAxes = false;
        noise.strength = characterRadius * 0.34f;
        noise.frequency = 1.35f;
        noise.scrollSpeed = 0.5f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particle.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = CreateHealingGradient(0.72f);

        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = particle.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.2f),
            new Keyframe(0.25f, 1f),
            new Keyframe(1f, 0.1f)));

        ConfigureRenderer(particle, 4);
    }

    void ConfigureGroundBurst(ParticleSystem particle, float characterHeight, float characterRadius)
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particle.main;
        main.loop = false;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.42f, 0.7f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(characterRadius * 1.1f, characterRadius * 2.2f);
        main.startSize = new ParticleSystem.MinMaxCurve(characterHeight * 0.025f, characterHeight * 0.065f);
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(0.82f, 1f, 0.84f, 0.78f),
            new Color(0.2f, 0.92f, 0.38f, 0.48f));
        main.maxParticles = 48;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emission = particle.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 32) });

        ParticleSystem.ShapeModule shape = particle.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = characterRadius * 0.46f;
        shape.radiusThickness = 0.1f;
        shape.rotation = new Vector3(90f, 0f, 0f);

        ParticleSystem.NoiseModule noise = particle.noise;
        noise.enabled = true;
        noise.separateAxes = false;
        noise.strength = characterRadius * 0.18f;
        noise.frequency = 1.8f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particle.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = CreateHealingGradient(0.8f);

        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = particle.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.35f),
            new Keyframe(0.2f, 1f),
            new Keyframe(1f, 0f)));

        ConfigureRenderer(particle, 5);
    }

    void ConfigureRenderer(ParticleSystem particle, int sortingOrder)
    {
        ParticleSystemRenderer particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        particleRenderer.sharedMaterial = healingMaterial;
        particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        particleRenderer.shadowCastingMode = ShadowCastingMode.Off;
        particleRenderer.receiveShadows = false;
        particleRenderer.lightProbeUsage = LightProbeUsage.Off;
        particleRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        particleRenderer.sortingOrder = sortingOrder;
    }

    static ParticleSystem.MinMaxGradient CreateHealingGradient(float peakAlpha)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.78f, 1f, 0.8f), 0f),
                new GradientColorKey(new Color(0.28f, 1f, 0.46f), 0.45f),
                new GradientColorKey(new Color(0.08f, 0.62f, 0.24f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(peakAlpha, 0.16f),
                new GradientAlphaKey(peakAlpha * 0.62f, 0.72f),
                new GradientAlphaKey(0f, 1f)
            });
        return new ParticleSystem.MinMaxGradient(gradient);
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
