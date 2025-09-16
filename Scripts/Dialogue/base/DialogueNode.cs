using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    public string nodeGUID;
    public Vector2 position;
    public string speakerName;

    // 输入连接
    public string inputNodeID;

    // 多个输出连接（子节点）
    public string[] outputNodeIDs;
    public List<string> nextNodes = new List<string>();
    public abstract void OnEnter(); // 进入节点时调用
    public virtual DialogueNode GetNextNode(Dictionary<string, DialogueNode> allNodes)
    {
        if (nextNodes != null && nextNodes.Count > 0)
        {
            string nextID = nextNodes[0]; // 默认取第一个输出
            if (allNodes.TryGetValue(nextID, out var nextNode))
            {
                return nextNode;
            }
        }
        return null;
    }
}
