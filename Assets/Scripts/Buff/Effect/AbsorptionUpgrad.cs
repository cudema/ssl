using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// [AddTypeMenu("OldEffect/회복 강화")]
// public class AbsorptionUpgrad : Effect
// {
//     public void OnApply(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp += 0.1f;
//     }

//     public void OnRemove(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp -= 0.1f;
//     }
// }

// [System.Serializable]
// [AddTypeMenu("OldEffect/회색 체력 감소 시간")]
// public class LingeringPain : Effect
// {
//     public void OnApply(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().DeownSpeed -= 10;
//     }

//     public void OnRemove(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().DeownSpeed += 10;
//     }
// }

// [System.Serializable]
// [AddTypeMenu("OldEffect/회색 체력 시간 증가")]
// public class FirmHoldout : Effect
// {
//     public void OnApply(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp = 0.6f;
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime += 2;
//     }

//     public void OnRemove(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp = 0.7f;
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime -= 2;
//     }
// }

// [System.Serializable]
// [AddTypeMenu("OldEffect/시간 감소 회복 강화")]
// public class ImmediateCounterattack : Effect
// {
//     public void OnApply(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp += 0.2f;
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime = 0;
//     }

//     public void OnRemove(Player player)
//     {
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().perSetGrayHp -= 0.2f;
//         player.GetComponent<PlayerEffectHandler>().FindEffect<ChangeHPEffect>().startDownTime = 1;
//     }
// }