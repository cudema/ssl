using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text", menuName = "TextData/Text")]
public class PrintData : ScriptableObject
{
    [Multiline(5)]
    public string[] strings;
}
