using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameControll : MonoBehaviour
{
    [SerializeField]
    Weapon[] weapons;

    Weapon mainWeapon;
    Weapon subWeapon;

    [SerializeField]
    ToggleGroup subGroup;

    Toggle[] subToggles;

    void Awake()
    {
        subToggles = subGroup.GetComponentsInChildren<Toggle>();
    }

    public void SetMainWeapon(int index)
    {
        mainWeapon = weapons[index];

        subToggles[0].interactable = true;
        subToggles[1].interactable = true;
        subToggles[2].interactable = true;

        subToggles[index].interactable = false;
        subToggles[index].isOn = false;
    }

    public void SetSubWeapon(int index)
    {
        subWeapon = weapons[index];
    }

    public void StartGame()
    {
        if (mainWeapon == null || subWeapon == null)
        {
            Debug.Log("실행 실패");
            return;
        }

        Player.instance.OnPositionSet(new UnityEngine.Vector3(0, 0.5f, 0));
        
        SceneManager.LoadScene("SampleScene");
    }

    void OnDestroy()
    {
        Player.instance.SetupWeapon(mainWeapon, subWeapon);
    }
}
