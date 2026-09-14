using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerAfterimageSnapshot : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    Renderer[] renderers;
    Mesh[] ownedMeshes;
    MaterialPropertyBlock propertyBlock;
    Color color;
    float lifetime;
    float elapsed;

    public void Initialize(List<Renderer> renderers, List<Mesh> ownedMeshes, Color color, float lifetime)
    {
        this.renderers = renderers.ToArray();
        this.ownedMeshes = ownedMeshes.ToArray();
        this.color = color;
        this.lifetime = Mathf.Max(lifetime, 0.01f);
        propertyBlock = new MaterialPropertyBlock();
        ApplyColor(color);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float remaining = 1f - Mathf.Clamp01(elapsed / lifetime);
        Color fadedColor = color;
        fadedColor.a *= remaining * remaining;
        ApplyColor(fadedColor);

        if (elapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void ApplyColor(Color value)
    {
        propertyBlock.SetColor(BaseColorId, value);
        foreach (Renderer snapshotRenderer in renderers)
        {
            if (snapshotRenderer != null) snapshotRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    void OnDestroy()
    {
        if (ownedMeshes == null) return;
        foreach (Mesh mesh in ownedMeshes)
        {
            if (mesh != null) Destroy(mesh);
        }
    }
}
