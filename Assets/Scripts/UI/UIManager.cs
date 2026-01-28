using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    public WeaponIcon weaponIcon;
    public CollDown skillCollDown;
    public CollDown dechCollDown;
    public CollDown SwitchingColldown;
    
    public StatAdder statAdder;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
