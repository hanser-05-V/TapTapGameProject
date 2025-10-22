using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PollListData", menuName = "Data/PollListData", order = 0)]
public class PollListData : BugData
{
     [LabelText("污染群体列表"), InlineEditor, SerializeField]
     public List<PollData> pollList;
}
