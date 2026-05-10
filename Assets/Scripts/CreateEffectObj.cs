using UnityEngine;
using UnityEditor;
using System;

public class DynamicPrefabCreator : EditorWindow
{
    // --- 설정 변수들 ---
    private string newPrefabName = "NewEntity";
    private GameObject effectParticlePrefab;

    // 드롭다운 설정
    private string[] classOptions = { "SpownObjAttack" };           //여기에 클래스 추가
    private int selectedClassIndex = 0;

    // [공통 변수]

    // [SpownObjAttack 전용 변수]
    private float damage = 1f;
    private float range = 5f;
    private int attackCount = 1;
    private float attackTime = 0f;
    private float stiffenTime = 1f;
    private bool isFollowingPlayer = false;

    [MenuItem("Tools/Dynamic Prefab Creator")]
    public static void ShowWindow()
    {
        GetWindow<DynamicPrefabCreator>("Dynamic Creator");
    }

    private void OnGUI()
    {
        // 1. 기본 정보 섹션
        GUILayout.Label("1. 기본 정보 설정", EditorStyles.boldLabel);
        newPrefabName = EditorGUILayout.TextField("프리팹 이름", newPrefabName);
        effectParticlePrefab = (GameObject)EditorGUILayout.ObjectField("파티클 프리팹", effectParticlePrefab, typeof(GameObject), false);

        EditorGUILayout.Space(10);

        // 2. 클래스 선택 섹션
        GUILayout.Label("2. 클래스 및 능력치 설정", EditorStyles.boldLabel);
        selectedClassIndex = EditorGUILayout.Popup("적용할 클래스", selectedClassIndex, classOptions);

        EditorGUILayout.Space(5);

        // --- 선택한 클래스에 따라 UI가 동적으로 변하는 부분 ---
        DrawClassSpecificFields();

        EditorGUILayout.Space(20);

        // 3. 실행 버튼
        if (GUILayout.Button("커스텀 프리팹 생성", GUILayout.Height(40)))
        {
            CreatePrefab();
        }
    }

    private void DrawClassSpecificFields()
    {
        // 박스 형태로 감싸서 시각적으로 구분
        EditorGUILayout.BeginVertical(GUI.skin.box);
        
        string currentClass = classOptions[selectedClassIndex];
        GUILayout.Label($"[{currentClass}] 전용 능력치", EditorStyles.miniBoldLabel);

        switch (currentClass)
        {
            case "SpownObjAttack":
                damage = EditorGUILayout.FloatField("공격력", damage);
                range = EditorGUILayout.FloatField("범위(반지름)", range);
                attackCount = EditorGUILayout.IntField("공격 횟수", attackCount);
                attackTime = EditorGUILayout.FloatField("공격 시간", attackTime);
                stiffenTime = EditorGUILayout.FloatField("경직 시간", stiffenTime);
                isFollowingPlayer = EditorGUILayout.Toggle("경직 시간", isFollowingPlayer);
                break;

            // case "MageLogic":
            //     manaAmount = EditorGUILayout.Slider("최대 마나", manaAmount, 0, 1000);
            //     spellCount = EditorGUILayout.IntSlider("보유 주문 수", spellCount, 1, 10);
            //     break;

            // case "HealerLogic":
            //     healRange = EditorGUILayout.FloatField("힐 사거리", healRange);
            //     canRevive = EditorGUILayout.Toggle("부활 기능 포함", canRevive);
            //     break;
        }

        EditorGUILayout.EndVertical();
    }

    private void CreatePrefab()
    {
        // 이름이 비어있는지 먼저 확인
        if (string.IsNullOrEmpty(newPrefabName))
        {
            EditorUtility.DisplayDialog("오류", "프리팹 이름을 입력해주세요!", "확인");
            return;
        }

        if (effectParticlePrefab == null)
        {
            EditorUtility.DisplayDialog("경고", "파티클 프리닥을 등록해야 합니다!", "확인");
            return;
        }

        // 경로 생성 및 검증
        string folderPath = "Assets/Prefab/EffectItemSpownObj"; // 혹은 "Assets/Prefabs" 등 실제 존재하는 폴더
        string localPath = $"{folderPath}/{newPrefabName}.prefab";
        
        // 유니티가 인식할 수 있는 고유한 경로로 변환 (이게 핵심입니다)
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // 1. 부모 오브젝트 생성
        GameObject parentObj = new GameObject(newPrefabName);
        string selectedClassName = classOptions[selectedClassIndex];
        
        // 2. 선택된 클래스 컴포넌트 추가
        Type type = Type.GetType(selectedClassName);
        if (type == null)
        {
            Debug.LogError($"{selectedClassName} 클래스가 프로젝트에 존재하지 않습니다. 클래스 이름을 확인하세요.");
            DestroyImmediate(parentObj);
            return;
        }

        Component script = parentObj.AddComponent(type);

        // 3. 자식 객체로 이펙트 생성 및 연결
        GameObject childEffect = (GameObject)PrefabUtility.InstantiatePrefab(effectParticlePrefab);
        childEffect.transform.SetParent(parentObj.transform);
        childEffect.transform.localPosition = Vector3.zero;

        // 4. 추가된 스크립트에 값 할당 (캐스팅 활용)
        // 각 클래스에 speed, atk, def 등의 변수가 public으로 선언되어 있어야 합니다.
        if (script is SpownObjAttack temp) 
        {
            temp.damage = damage;
            temp.range = range;
            temp.attackCount = attackCount;
            temp.attackTime = attackTime;
            temp.stiffenTime = stiffenTime;
            temp.isFollowingPlayer = isFollowingPlayer;
            temp.particle = childEffect.GetComponent<ParticleSystem>();
        }
        // else if (script is MageLogic mage) {
        //     mage.speed = moveSpeed;
        //     mage.mana = manaAmount;
        //     mage.spells = spellCount;
        // }
        // else if (script is HealerLogic healer) {
        //     healer.speed = moveSpeed;
        //     healer.range = healRange;
        //     healer.revive = canRevive;
        // }

        // 5. 프리팹 파일로 저장
        // string localPath = $"Assets/Profab/EffectItemSpownObj/{newPrefabName}.prefab";
        // localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAsset(parentObj, localPath);

        // 6. 하이어라키 정리
        DestroyImmediate(parentObj);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("완료", $"프리팹 생성이 완료되었습니다!\n경로: {localPath}", "확인");
    }
}