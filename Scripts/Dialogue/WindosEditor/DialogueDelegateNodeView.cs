#if UNITY_EDITOR
public class DialogueDelegateNodeView : BaseNodeView
{
    public DialogueDelegateNode DelegateNode => Data as DialogueDelegateNode;

    public DialogueDelegateNodeView(DialogueDelegateNode node) : base(node)
    {
        title = "委托节点（基类）";

        // 清理之前的所有端口，避免重复
        ClearInputPort();
        ClearOutputPorts();

        // 添加一个输入端口和一个输出端口
        AddInputPort("输入");
        AddOutputPort("下一节点");

        RefreshPorts();
        RefreshExpandedState();
    }
}
#endif
