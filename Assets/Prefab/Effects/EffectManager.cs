using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject slashEffectPrefab;
    [SerializeField] GameObject slash2EffectPrefab;
    [SerializeField] GameObject Sword_Skill_1EffectPrefab;
    [SerializeField] GameObject Sword_Skill_2EffectPrefab;
    [SerializeField] GameObject Sword_SwitchEffectPrefab;
    [SerializeField] GameObject Sword_Switch_SlashEffectPrefab;
    [SerializeField] GameObject Sword_Switch_Slash_2EffectPrefab;
    [SerializeField] GameObject Axe_SkillEffectPrefab;
    [SerializeField] GameObject Axe_SlashEffectPrefab;
    [SerializeField] GameObject Axe_Slash2EffectPrefab;
    [SerializeField] GameObject DashEffectPrefab;

    public void SpawnAttackEffect(string effectType, Vector3 pos, Quaternion rot)
    {
        switch (effectType)
        {
            case "Slash":
                Instantiate(
                    slashEffectPrefab,
                    pos,
                    rot * slashEffectPrefab.transform.rotation
                );
                break;

            case "Slash2":
                Instantiate(
                    slash2EffectPrefab,
                    pos,
                    rot * slash2EffectPrefab.transform.rotation
                );
                break;

            case "Sword_Skill_1":
                Instantiate(
                    Sword_Skill_1EffectPrefab,
                    pos,
                    rot * Sword_Skill_1EffectPrefab.transform.rotation
                );
                break;

            case "Sword_Skill_2":
                Instantiate(
                    Sword_Skill_2EffectPrefab,
                    pos,
                    rot * Sword_Skill_2EffectPrefab.transform.rotation
                );
                break;

            case "Sword_Switch":
                Instantiate(
                    Sword_SwitchEffectPrefab,
                    pos,
                    rot * Sword_SwitchEffectPrefab.transform.rotation
                );
                break;

            case "Sword_Switch_Slash":
                Instantiate(
                    Sword_Switch_SlashEffectPrefab,
                    pos,
                    rot * Sword_Switch_SlashEffectPrefab.transform.rotation
                );
                break;

            case "Sword_Switch_Slash_2":
                Instantiate(
                    Sword_Switch_Slash_2EffectPrefab,
                    pos,
                    rot * Sword_Switch_Slash_2EffectPrefab.transform.rotation
                );
                break;

            case "Axe_Skill":
                Instantiate(
                    Axe_SkillEffectPrefab,
                    pos,
                    rot * Axe_SkillEffectPrefab.transform.rotation
                );
                break;

            case "Axe_Slash":
                Instantiate(
                    Axe_SlashEffectPrefab,
                    pos,
                    rot * Axe_SlashEffectPrefab.transform.rotation
                );
                break;

            case "Axe_Slash2":
                Instantiate(
                    Axe_Slash2EffectPrefab,
                    pos,
                    rot * Axe_Slash2EffectPrefab.transform.rotation
                );
                break;

            case "Dash":
                Instantiate(
                    DashEffectPrefab,
                    pos,
                    rot * DashEffectPrefab.transform.rotation
                );
                break;
        }
    }
}