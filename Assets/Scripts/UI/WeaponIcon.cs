using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIcon : MonoBehaviour
{
    [SerializeField]
    Image image;

    public void ChangeIcon(Sprite newIcon)
    {
        image.sprite = newIcon;
    }
}
