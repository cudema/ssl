using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [SerializeField] private int currentGold;
    public int CurrentGold => currentGold; // 외부에서 읽기 전용

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 재화 추가 (몬스터 처치 시 호출)
    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    // 재화 사용 (강화 시 호출)
    public bool TrySpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
        Debug.Log("Lack Of Coin");
        return false; // 잔액 부족
    }

    private void UpdateUI()
    {
        // UI 매니저에게 골드 갱신 알림
    }
}