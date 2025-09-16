using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueGraphData", menuName = "Dialogue/Graph Data")]
public class DialogueGraphData : ScriptableObject
{
    public List<DialogueNode> allNodes = new List<DialogueNode>();
}
