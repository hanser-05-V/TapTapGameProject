#if UNITY_EDITOR
using UnityEngine;

public class DialogueEndNodeView : BaseNodeView
{
    public DialogueEndNode endNode;

    public DialogueEndNodeView(DialogueEndNode node) : base(node)
    {
        endNode = node;
        title = "End";
        viewDataKey = node.nodeGUID;

        // 设置输入端口名
        if (InputPort != null)
            InputPort.portName = "→ End";

        // 移除输出端口（End节点不应有输出）
        if (OutputPorts != null && OutputPorts.Count > 0)
        {
            foreach (var port in OutputPorts)
            {
                outputContainer.Remove(port);
            }
            OutputPorts.Clear();
        }
        SetPosition(new Rect(node.position, Vector2.zero)); // ✅ 正确设置位置
        RefreshPorts();
        RefreshExpandedState();
    }
}
#endif
