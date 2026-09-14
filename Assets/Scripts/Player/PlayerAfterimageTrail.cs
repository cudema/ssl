using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public sealed class PlayerAfterimageTrail : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField]
    Color afterimageColor = new Color(0.78f, 0.8f, 0.84f, 0.5f);
    [SerializeField, Min(0.05f)]
    float lifetime = 0.26f;
    [SerializeField, Min(0.1f)]
    float minimumSpacing = 0.35f;
    [SerializeField, Range(2, 3)]
    int maximumSnapshots = 3;

    Material afterimageMaterial;
    float activeSpacing;
    float distanceSinceSnapshot;
    int emittedSnapshots;

    void Awake()
    {
        Shader shader = Shader.Find("Hidden/SSL/PlayerAfterimage");
        if (shader == null)
        {
            Debug.LogError("Player afterimage shader could not be found.", this);
            enabled = false;
            return;
        }

        afterimageMaterial = new Material(shader)
        {
            name = "Player Afterimage (Runtime)",
            hideFlags = HideFlags.HideAndDontSave
        };
        afterimageMaterial.SetColor(BaseColorId, afterimageColor);
    }

    void OnDestroy()
    {
        if (afterimageMaterial != null)
        {
            Destroy(afterimageMaterial);
        }
    }

    public void BeginTrail(Vector3 playerPosition, float travelDistance)
    {
        if (!enabled || afterimageMaterial == null) return;

        distanceSinceSnapshot = 0f;
        emittedSnapshots = 0;
        activeSpacing = Mathf.Max(
            minimumSpacing,
            Mathf.Abs(travelDistance) / Mathf.Max(1, maximumSnapshots));
        CaptureAt(playerPosition);
    }

    public void EmitSegment(Vector3 from, Vector3 to)
    {
        if (!enabled || afterimageMaterial == null || emittedSnapshots >= maximumSnapshots) return;

        float segmentLength = Vector3.Distance(from, to);
        if (segmentLength <= Mathf.Epsilon) return;

        float travelled = 0f;
        while (emittedSnapshots < maximumSnapshots && distanceSinceSnapshot + segmentLength - travelled >= activeSpacing)
        {
            float step = activeSpacing - distanceSinceSnapshot;
            travelled += step;
            float t = Mathf.Clamp01(travelled / segmentLength);
            CaptureAt(Vector3.Lerp(from, to, t));
            distanceSinceSnapshot = 0f;
        }

        distanceSinceSnapshot += Mathf.Max(0f, segmentLength - travelled);
    }

    void CaptureAt(Vector3 playerPosition)
    {
        GameObject snapshotObject = new GameObject("Player Afterimage");
        snapshotObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        Vector3 positionOffset = playerPosition - transform.position;

        List<Renderer> snapshotRenderers = new List<Renderer>();
        List<Mesh> ownedMeshes = new List<Mesh>();

        SkinnedMeshRenderer[] skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(false);
        foreach (SkinnedMeshRenderer source in skinnedRenderers)
        {
            if (!source.enabled || source.sharedMesh == null) continue;

            Mesh bakedMesh = new Mesh { name = source.sharedMesh.name + " Afterimage" };
            source.BakeMesh(bakedMesh);
            Vector3[] vertices = bakedMesh.vertices;
            Matrix4x4 localToWorld = source.transform.localToWorldMatrix;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = localToWorld.MultiplyPoint3x4(vertices[i]) + positionOffset;
            }

            if (vertices.Length == 0)
            {
                Destroy(bakedMesh);
                continue;
            }

            Bounds bakedWorldBounds = new Bounds(vertices[0], Vector3.zero);
            for (int i = 1; i < vertices.Length; i++)
            {
                bakedWorldBounds.Encapsulate(vertices[i]);
            }

            Bounds sourceWorldBounds = source.bounds;
            sourceWorldBounds.center += positionOffset;
            float bakedSize = Mathf.Max(
                bakedWorldBounds.size.x,
                Mathf.Max(bakedWorldBounds.size.y, bakedWorldBounds.size.z));
            float sourceSize = Mathf.Max(
                sourceWorldBounds.size.x,
                Mathf.Max(sourceWorldBounds.size.y, sourceWorldBounds.size.z));

            if (bakedSize > Mathf.Epsilon && sourceSize > Mathf.Epsilon)
            {
                float scaleCorrection = sourceSize / bakedSize;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = sourceWorldBounds.center
                        + (vertices[i] - bakedWorldBounds.center) * scaleCorrection;
                }
            }

            bakedMesh.vertices = vertices;
            bakedMesh.RecalculateBounds();
            ownedMeshes.Add(bakedMesh);
            snapshotRenderers.Add(CreateSnapshotRenderer(
                snapshotObject.transform,
                source,
                bakedMesh,
                Vector3.zero,
                Quaternion.identity,
                Vector3.one));
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(false);
        foreach (MeshRenderer source in meshRenderers)
        {
            if (!source.enabled) continue;
            MeshFilter sourceFilter = source.GetComponent<MeshFilter>();
            if (sourceFilter == null || sourceFilter.sharedMesh == null) continue;

            snapshotRenderers.Add(CreateSnapshotRenderer(
                snapshotObject.transform,
                source,
                sourceFilter.sharedMesh,
                source.transform.position + positionOffset,
                source.transform.rotation,
                source.transform.lossyScale));
        }

        if (snapshotRenderers.Count == 0)
        {
            Destroy(snapshotObject);
            foreach (Mesh mesh in ownedMeshes) Destroy(mesh);
            return;
        }

        PlayerAfterimageSnapshot snapshot = snapshotObject.AddComponent<PlayerAfterimageSnapshot>();
        snapshot.Initialize(snapshotRenderers, ownedMeshes, afterimageColor, lifetime);
        emittedSnapshots++;
    }

    MeshRenderer CreateSnapshotRenderer(
        Transform parent,
        Renderer source,
        Mesh mesh,
        Vector3 position,
        Quaternion rotation,
        Vector3 scale)
    {
        GameObject part = new GameObject(source.gameObject.name + " Afterimage");
        part.layer = source.gameObject.layer;
        part.transform.SetParent(parent, false);
        part.transform.SetPositionAndRotation(position, rotation);
        part.transform.localScale = scale;

        MeshFilter filter = part.AddComponent<MeshFilter>();
        filter.sharedMesh = mesh;

        MeshRenderer renderer = part.AddComponent<MeshRenderer>();
        Material[] materials = new Material[Mathf.Max(1, mesh.subMeshCount)];
        for (int i = 0; i < materials.Length; i++) materials[i] = afterimageMaterial;
        renderer.sharedMaterials = materials;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = LightProbeUsage.Off;
        renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        renderer.sortingLayerID = source.sortingLayerID;
        renderer.sortingOrder = source.sortingOrder - 1;
        return renderer;
    }

}
