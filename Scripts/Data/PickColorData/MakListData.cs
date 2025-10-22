using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "MakListData", menuName = "Data/Mask/MakListData")]
public class MaskListData : BugData
{
     [ InlineEditor, SerializeField]
    public List<MaskData> maskDataList;
}
