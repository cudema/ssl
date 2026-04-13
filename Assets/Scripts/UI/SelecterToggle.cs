using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecterToggle : UIBase
{
    Toggle toggle;

    [SerializeField]
    Image[] images;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public override void OnUI()
    {
        UI.SetActive(true);
    }

    public override void OffUI()
    {
        UI.SetActive(false);
    }

    public void SetImage(int index)
    {
        images[0].gameObject.SetActive(false);
        images[1].gameObject.SetActive(false);
        images[2].gameObject.SetActive(false);

        if (index == -1) return;

        images[index].gameObject.SetActive(true);
    }
}
