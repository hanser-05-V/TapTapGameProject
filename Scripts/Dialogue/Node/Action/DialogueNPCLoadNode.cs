using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Delegate/LoadNPC")]
public class DialogueNPCLoadNode : DialogueDelegateNode
{
    public int npcID = 0;
    public int loadPositionID = 0;

    public override void Execute()
    {
        
        //放置需要执行的逻辑 加载NPC到位置
       
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
