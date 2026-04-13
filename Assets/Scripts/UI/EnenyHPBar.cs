using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnenyHPBar : MonoBehaviour
{
    [SerializeField]
    GameObject UI;
    [SerializeField]
    Image hpBarFill; // 위에서 만든 Filled 이미지 할당
    [SerializeField]
    EnemyBase enemy;

    void Update()
    {
        if (!UI.activeSelf && enemy.hp / enemy.maxHP != 1)
        {
            UI.SetActive(true);
        }
    }

    void OnDisable()
    {
        UI.SetActive(false);
    }

    public void UpdateHPBar()
    {
        // 이미지의 Fill Amount를 업데이트 (0 ~ 1 사이 값)
        hpBarFill.fillAmount = enemy.hp / enemy.maxHP;
    }
}
