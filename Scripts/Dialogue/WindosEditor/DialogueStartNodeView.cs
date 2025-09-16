#if UNITY_EDITOR
using UnityEngine;

public class DialogueStartNodeView : BaseNodeView
{
    public DialogueStartNode startNode;

    public DialogueStartNodeView(DialogueStartNode node) : base(node)
    {
        this.startNode = node;
        this.title = "Start";
        this.viewDataKey = node.nodeGUID;

        // 移除输入端口
        if (InputPort != null)
        {
            inputContainer.Remove(InputPort);
            InputPort = null;
        }

        // 设置输出端口名称（Start节点应有一个输出）
        if (OutputPorts != null && OutputPorts.Count > 0)
        {
            OutputPorts[0].portName = "Start →";
        }
        SetPosition(new Rect(node.position, Vector2.zero)); // ✅ 正确设置位置
        RefreshPorts();
        RefreshExpandedState();
    }
}
#endif
