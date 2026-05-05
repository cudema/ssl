using UnityEngine;
using UnityEngine.VFX;

public class DeathVFXController : MonoBehaviour
{
    [Header("References")]
    public SkinnedMeshRenderer playerRenderer;
    public GameObject deathVFXPrefab;

    [Header("Settings")]
    public float destroyDelay = 3f;

    private Mesh bakedMesh;

    public void PlayDeathVFX()
    {
        if (playerRenderer == null)
        {
            Debug.LogWarning("Player Renderer�� �������");
            return;
        }

        if (deathVFXPrefab == null)
        {
            Debug.LogWarning("Death VFX Prefab�� �������");
            return;
        }

        // ���� �÷��̾� ��� Mesh�� ����
        if (bakedMesh == null)
            bakedMesh = new Mesh();

        playerRenderer.BakeMesh(bakedMesh);

        // VFX ������ ����
        GameObject vfxObj = Instantiate(
            deathVFXPrefab,
            playerRenderer.bounds.center,
            playerRenderer.GetComponentInParent<Transform>().rotation
        );

        // VFX Graph ������Ʈ ��������
        VisualEffect vfx = vfxObj.GetComponentInChildren<VisualEffect>();

        if (vfx == null)
        {
            Debug.LogWarning("Death VFX Prefab�� VisualEffect ������Ʈ�� ����");
            Destroy(vfxObj);
            return;
        }

        // VFX Graph Blackboard�� Mesh �̸��� DeathMesh���� ��
        vfx.SetMesh("DeathMesh", bakedMesh);

        // ����
        vfx.Play();

        // ���� �ð� �� ����
        Destroy(vfxObj, destroyDelay);
    }
}