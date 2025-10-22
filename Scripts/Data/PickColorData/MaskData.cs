using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MaskDta", menuName = "Data/Mask/Mask Data")]
public class MaskData : BugData
{

    public int posId; //位置坐标ID
    
    public GameObject maskPrefab;

    // public Sprite lockPrefab;
    public string tipText;
    public Color unlockColor;
    // public Sprite unlockSprite;

}
