using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CrawListData", menuName = "Data/CrawListData")]
public class CrawListData : BugData
{
    [LabelText("虫子列表"), InlineEditor, SerializeField]
     public List<CrawData> crawList;
}
