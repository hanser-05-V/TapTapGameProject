#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePauseNodeView : DialogueDelegateNodeView
{
    public DialoguePauseNodeView(DialoguePauseNode node) : base(node)
    {
        title = "暂停节点";
        // 暂无输入字段，只是一个行为节点
    }
}
#endif
