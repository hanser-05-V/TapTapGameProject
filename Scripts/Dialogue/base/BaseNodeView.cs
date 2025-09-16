#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 编辑器中的节点视图类（运行时不能使用）
/// </summary>
public class BaseNodeView : Node
{
    public DialogueNode Data { get; protected set; }

    public virtual Port InputPort { get; protected set; }

    // 支持多输出端口（比如条件节点、选项节点）
    public virtual List<Port> OutputPorts { get; protected set; } = new List<Port>();

    public BaseNodeView(DialogueNode data)
    {
        Data = data;
        title = data.name;

        InitPorts();
        RefreshPorts();
        RefreshExpandedState();
    }

    protected virtual void InitPorts()
    {
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string));
        var defaultOutput = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(string));
        OutputPorts = new List<Port> { defaultOutput };

        InputPort.portName = "Input";
        defaultOutput.portName = "Output";

        inputContainer.Add(InputPort);
        outputContainer.Add(defaultOutput);
    }

    protected void AddOutputPort(string portName)
    {
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        port.portName = portName;
        outputContainer.Add(port);
        OutputPorts.Add(port);
    }

    protected void AddInputPort(string portName)
    {
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        port.portName = portName;
        inputContainer.Add(port);
        InputPort = port;
    }

    protected void ClearOutputPorts()
    {
        OutputPorts.Clear();
        outputContainer.Clear();
    }

    protected void ClearInputPort()
    {
        if (InputPort != null)
        {
            inputContainer.Remove(InputPort);
            InputPort = null;
        }
    }
}
#endif
