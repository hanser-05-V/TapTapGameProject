#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNodeView : BaseNodeView
{
    public DialogueTextNode textNode;

    public DialogueNodeView(DialogueTextNode node) : base(node)  // 继承构造函数传参，绑定Data
    {
        textNode = node;

        title = node.speakerName;
        viewDataKey = node.nodeGUID;

        // 设置位置
        SetPosition(new Rect(node.position, new Vector2(250, 150)));

        // 添加对话内容输入框
        var dialogueField = new TextField("说话内容")
        {
            value = node.dialogueText
        };
        dialogueField.RegisterValueChangedCallback(evt =>
        {
            node.dialogueText = evt.newValue;
        });
        mainContainer.Add(dialogueField);

        RefreshPorts();
        RefreshExpandedState();
    }
}
#endif
