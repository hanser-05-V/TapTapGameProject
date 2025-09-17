using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Delegate/Pause")]
public class DialoguePauseNode : DialogueDelegateNode
{
    public override void Execute()
    {
        
    }

    public override DialogueNode GetNextNode(Dictionary<string, DialogueNode> allNodes)
    {
        // 暂时不返回下一个节点，Resume 时再由 Controller 手动调用
        if (nextNodes.Count > 0 && allNodes.TryGetValue(nextNodes[0], out var nextNode))
        {
            return nextNode;
        }

        return null;
    }
}
