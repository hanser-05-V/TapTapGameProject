using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/Delegate/ChangeBG")]
public class DialogueBGNode : DialogueDelegateNode
{
    public string bgName = "default";
    private const string BaseDiaLohPath = "dialogBG";
    public override void Execute()
    {
        
    }
    public override DialogueNode GetNextNode(Dictionary<string, DialogueNode> allNodes)
    {
        if (nextNodes.Count > 0 && allNodes.TryGetValue(nextNodes[0], out var nextNode))
        {
            return nextNode;
        }
        return null;
    }
}
