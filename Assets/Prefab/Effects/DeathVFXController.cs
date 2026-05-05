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
            Debug.LogWarning("Player Renderer가 비어있음");
            return;
        }

        if (deathVFXPrefab == null)
        {
            Debug.LogWarning("Death VFX Prefab이 비어있음");
            return;
        }

        // 현재 플레이어 포즈를 Mesh로 굳힘
        if (bakedMesh == null)
            bakedMesh = new Mesh();

        playerRenderer.BakeMesh(bakedMesh);

        // VFX 프리팹 생성
        GameObject vfxObj = Instantiate(
            deathVFXPrefab,
            playerRenderer.bounds.center,
            Quaternion.identity
        );

        // VFX Graph 컴포넌트 가져오기
        VisualEffect vfx = vfxObj.GetComponentInChildren<VisualEffect>();

        if (vfx == null)
        {
            Debug.LogWarning("Death VFX Prefab에 VisualEffect 컴포넌트가 없음");
            Destroy(vfxObj);
            return;
        }

        // VFX Graph Blackboard의 Mesh 이름이 DeathMesh여야 함
        vfx.SetMesh("DeathMesh", bakedMesh);

        // 실행
        vfx.Play();

        // 일정 시간 뒤 삭제
        Destroy(vfxObj, destroyDelay);
    }
}