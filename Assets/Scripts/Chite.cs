using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chite : MonoBehaviour
{
    public static Chite instance;

    [SerializeField]
    EffectItem attackChite;
    [SerializeField]
    EffectItem defanceChite;

    bool haveItem = false;

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
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (!haveItem && Input.GetKeyDown(KeyCode.F10))
        {
            InventoryManager.instance.AddItem(Instantiate(attackChite, UIManager.instance.transform));
            InventoryManager.instance.AddItem(Instantiate(defanceChite, UIManager.instance.transform));

            haveItem = true;
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            StageManager.instance.OnStageChite();
        }
    }
}
