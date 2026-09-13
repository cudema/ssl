using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text", menuName = "TextData/Text")]
public class PrintData : ScriptableObject
{
    [SerializeField]
    public Sprite npcSprite;
    [Multiline(5)]
    public string[] strings;
}
