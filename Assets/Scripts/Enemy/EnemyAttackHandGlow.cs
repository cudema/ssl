using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public sealed class EnemyAttackHandGlow : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    readonly Color glowColor = new Color(1f, 0.72f, 0.08f, 0.9f);

    EnemyBase enemy;
    Transform leftHand;
    Transform rightHand;
    ParticleSystem leftGlow;
    ParticleSystem rightGlow;
    Material glowMaterial;
    Coroutine playCoroutine;
    Bounds characterBounds;
    Vector3 fallbackCenterLocal;
    bool useLeftFallback;
    bool useRightFallback;

    void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        FindHands();
        characterBounds = CalculateCharacterBounds();
        fallbackCenterLocal = transform.InverseTransformPoint(characterBounds.center);

        Shader shader = Shader.Find("Hidden/SSL/EnemyAttackHandGlow");
        if (shader == null)
        {
            enabled = false;
            return;
        }

        glowMaterial = new Material(shader)
        {
            name = "Enemy Attack Hand Glow (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        glowMaterial.SetColor(BaseColorId, glowColor);

        float effectRadius = Mathf.Clamp(characterBounds.size.y * 0.055f, 0.12f, 0.34f);
        leftGlow = CreateGlow("Left Hand Attack Warning", effectRadius);
        rightGlow = CreateGlow("Right Hand Attack Warning", effectRadius);
        UpdateGlowPositions();
    }

    void LateUpdate()
    {
        UpdateGlowPositions();
    }

    void OnDisable()
    {
        StopGlow();
    }

    void OnDestroy()
    {
        if (glowMaterial != null)
        {
            Destroy(glowMaterial);
        }
    }

    public void Play(float duration)
    {
        if (!enabled || leftGlow == null || rightGlow == null) return;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine(PlayForDuration(Mathf.Max(0.05f, duration)));
    }

    IEnumerator PlayForDuration(float duration)
    {
        UpdateGlowPositions();
        leftGlow.Play(true);
        rightGlow.Play(true);
        yield return new WaitForSeconds(duration);
        StopEmission();
        playCoroutine = null;
    }

    void StopEmission()
    {
        if (leftGlow != null)
        {
            leftGlow.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (rightGlow != null)
        {
            rightGlow.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void StopGlow()
    {
        if (leftGlow != null)
        {
            leftGlow.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (rightGlow != null)
        {
            rightGlow.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        playCoroutine = null;
    }

    void FindHands()
    {
        Animator animator = enemy != null ? enemy.animator : GetComponentInChildren<Animator>(true);
        if (animator != null && animator.isHuman)
        {
            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        }

        Transform searchRoot = animator != null ? animator.transform : transform;
        Transform[] bones = searchRoot.GetComponentsInChildren<Transform>(true);
        if (leftHand == null) leftHand = FindNamedHand(bones, true);
        if (rightHand == null) rightHand = FindNamedHand(bones, false);

        useLeftFallback = leftHand == null;
        useRightFallback = rightHand == null;
    }

    static Transform FindNamedHand(Transform[] bones, bool left)
    {
        Transform best = null;
        int bestScore = 0;
        foreach (Transform bone in bones)
        {
            string name = bone.name.ToLowerInvariant()
                .Replace("_", string.Empty)
                .Replace("-", string.Empty)
                .Replace(":", string.Empty)
                .Replace(".", string.Empty)
                .Replace(" ", string.Empty);

            int score = 0;
            string sideHand = left ? "lefthand" : "righthand";
            string handSide = left ? "handl" : "handr";
            if (name.EndsWith(sideHand)) score = 10;
            else if (name.Contains(sideHand)) score = 8;
            else if (name.EndsWith(handSide)) score = 7;

            if (score > bestScore)
            {
                best = bone;
                bestScore = score;
            }
        }
        return best;
    }

    ParticleSystem CreateGlow(string objectName, float radius)
    {
        GameObject glowObject = new GameObject(objectName);
        glowObject.transform.SetParent(transform, false);

        ParticleSystem particle = glowObject.AddComponent<ParticleSystem>();
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule main = particle.main;
        main.loop = true;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.16f, 0.28f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(radius * 0.5f, radius * 1.4f);
        main.startSize = new ParticleSystem.MinMaxCurve(radius * 0.55f, radius * 1.05f);
        main.startColor = glowColor;
        main.maxParticles = 28;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.scalingMode = ParticleSystemScalingMode.Shape;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emission = particle.emission;
        emission.rateOverTime = 32f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 7) });

        ParticleSystem.ShapeModule shape = particle.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = radius;
        shape.radiusThickness = 1f;

        ParticleSystem.NoiseModule noise = particle.noise;
        noise.enabled = true;
        noise.strength = radius * 0.8f;
        noise.frequency = 1.5f;
        noise.scrollSpeed = 0.4f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particle.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(new Color(1f, 0.55f, 0.05f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.9f, 0f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ParticleSystemRenderer particleRenderer = glowObject.GetComponent<ParticleSystemRenderer>();
        particleRenderer.sharedMaterial = glowMaterial;
        particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        particleRenderer.shadowCastingMode = ShadowCastingMode.Off;
        particleRenderer.receiveShadows = false;
        particleRenderer.lightProbeUsage = LightProbeUsage.Off;
        particleRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        return particle;
    }

    void UpdateGlowPositions()
    {
        if (leftGlow == null || rightGlow == null) return;

        Vector3 center = transform.TransformPoint(fallbackCenterLocal);
        float sideOffset = Mathf.Max(0.18f, characterBounds.size.x * 0.32f);
        float heightOffset = characterBounds.size.y * 0.05f;

        leftGlow.transform.position = useLeftFallback
            ? center - transform.right * sideOffset + transform.up * heightOffset
            : leftHand.position;
        rightGlow.transform.position = useRightFallback
            ? center + transform.right * sideOffset + transform.up * heightOffset
            : rightHand.position;
    }

    Bounds CalculateCharacterBounds()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        Bounds bounds = new Bounds(transform.position + transform.up, Vector3.one * 2f);
        bool initialized = false;
        foreach (Renderer renderer in renderers)
        {
            if (renderer is ParticleSystemRenderer) continue;
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
