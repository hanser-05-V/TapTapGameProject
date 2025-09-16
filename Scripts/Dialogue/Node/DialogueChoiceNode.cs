using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/DialogueChoiceNode")]
[System.Serializable]
public class DialogueChoiceNode : DialogueNode
{
    // 这里你可以定义分支选项数组或者其他字段
    public List<string> choices = new List<string>(); 
    public List<string> nextNodes; // 与 choices 一一对应的跳转节点 GUID
    public override void OnEnter()
    {
        if (choices == null) choices = new List<string>();
        if (nextNodes == null) nextNodes = new List<string>();
    }
}
