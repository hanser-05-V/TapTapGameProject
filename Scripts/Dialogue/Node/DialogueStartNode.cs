using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/StartNode")]
public class DialogueStartNode : DialogueNode
{
    // 不需要额外字段，自动开始执行
    public override void OnEnter()
    {
        
        throw new System.NotImplementedException();
    }
}
