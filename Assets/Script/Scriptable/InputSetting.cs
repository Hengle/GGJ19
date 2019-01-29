using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player Setting", order = 1)]
public class InputSetting : ScriptableObject
{
    public string keyHorizontal;
    public string keyVertical;

    public string keyJump;
}
