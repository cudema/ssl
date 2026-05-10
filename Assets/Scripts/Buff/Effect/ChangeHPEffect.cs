using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Serializable]
// [AddTypeMenu("OldEffect/공격 시 회복")]
// public class ChangeHPEffect : IAttackEffect, IHPChanged, IUpdateEffect
// {
//     public event Action<float> ChangedGrayHp;

//     float grayHp;

//     bool IsDownGrayHp = false;
//     float timer = 0;
//     public float startDownTime = 1f;
//     public float perSetGrayHp = 0.7f;
//     public float DeownSpeed = 20f;
//     public float recoveryPer = 0.05f;

//     public float GrayHp
//     {
//         private set
//         {
//             float temp = grayHp;
//             grayHp = Mathf.Clamp(value, 0, Player.instance.MaxHp);

//             ChangedGrayHp?.Invoke(value);
//         }
//         get => grayHp;
//     }

//     public void OnApply(Player player)
//     {
//         Player.instance.ChangedHp += ChangedHP;
//         UIManager.instance.hpBar.OnGrayHPEffect();
//         //grayHp = Player.instance.CurrentHp;
//     }

//     public void OnRemove(Player player)
//     {
//         Player.instance.ChangedHp -= ChangedHP;
//         UIManager.instance.hpBar.OffGrayHPEffect();
//     }

//     public void OnEffect(BuffManager enemy)
//     {
//         Player.instance.CurrentHp += GrayHp * recoveryPer;
//     }

//     public void ChangedHP(float value)
//     {
//         if (value > 0)
//         {
//             GrayHp += -value;
//             return;
//         }
//         timer = Time.time;
//         IsDownGrayHp = false;
//         GrayHp += -value * perSetGrayHp;
//     }

//     public void OnUpdateEffect()
//     {
//         if (IsDownGrayHp)
//         {
//             GrayHp -= DeownSpeed * Time.deltaTime;
//         }
//         else if (Time.time - timer > startDownTime)
//         {
//             IsDownGrayHp = true;
//         }
//     }
// }
