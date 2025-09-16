using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class DialogueDelegateNode : DialogueNode
{   
    public override void OnEnter()
    {
        
        Execute(); // 把逻辑下放给子类
        DialogueContral.instance.MoveToNextNode();
    }

    public abstract void Execute(); // 子类实现具体功能

    public override DialogueNode GetNextNode(Dictionary<string, DialogueNode> allNodes)
    {
        if (nextNodes.Count > 0 && allNodes.TryGetValue(nextNodes[0], out var nextNode))
        {
            return nextNode;
        }
        return null;
    }
}
