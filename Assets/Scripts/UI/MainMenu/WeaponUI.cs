using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] main;
    [SerializeField]
    GameObject[] sub;

    public void SetWeapon(int main, int sub)
    {
        foreach (GameObject temp in this.main)
        {
            temp.SetActive(false);
        }
        foreach (GameObject temp in this.sub)
        {
            temp.SetActive(false);
        }
        if (main > 2 || sub > 2)
        {
            return;
        }
        this.main[main].SetActive(true);
        this.sub[sub].SetActive(true);
    }
}
