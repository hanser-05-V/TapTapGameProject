#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
public class DialogueChoiceNodeView : BaseNodeView
{
    public DialogueChoiceNode choiceNode;
    public VisualElement choicesContainer;
    public List<TextField> choiceFields = new List<TextField>();
    public List<Port> choicePorts = new List<Port>();

    public DialogueChoiceNodeView(DialogueChoiceNode node) : base(node)
    {
        choiceNode = node;

        title = node.name;
        viewDataKey = node.nodeGUID;

        style.left = node.position.x;
        style.top = node.position.y;

        // 移除默认输出端口
        foreach (var port in outputContainer.Children().ToList())
        {
            outputContainer.Remove(port);
        }

        // 添加输入端口
        var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string));
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        // 添加选项容器
        choicesContainer = new VisualElement();
        choicesContainer.style.flexDirection = FlexDirection.Column;
        choicesContainer.style.marginTop = 10;
        mainContainer.Add(choicesContainer);

        // 初始化选项
        RefreshChoices();

        // 添加按钮
        var addButton = new Button(() => AddChoice("新选项")) { text = "添加选项" };
        mainContainer.Add(addButton);

        RefreshExpandedState();
    }

    public void RefreshChoices()
    {
        // 清空容器
        choicesContainer.Clear();
        choiceFields.Clear();

        // 断开并移除旧端口
        foreach (var port in choicePorts)
        {
            foreach (var edge in port.connections.ToList())
            {
                edge.input?.Disconnect(edge);
                edge.output?.Disconnect(edge);
                edge.parent?.Remove(edge);
            }
            outputContainer.Remove(port);
        }
        choicePorts.Clear();

        // 初始化数据
        if (choiceNode.choices == null)
            choiceNode.choices = new List<string>();
        if (choiceNode.nextNodes == null)
            choiceNode.nextNodes = new List<string>();

        // nextNodes 补齐长度
        while (choiceNode.nextNodes.Count < choiceNode.choices.Count)
            choiceNode.nextNodes.Add("");

        // 逐个生成选项
        for (int i = 0; i < choiceNode.choices.Count; i++)
        {
            int index = i;

            // 容器行
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            row.style.marginBottom = 5;

            // 输入框
            var field = new TextField($"选项 {i + 1}")
            {
                value = choiceNode.choices[i]
            };
            field.style.flexGrow = 1;
            field.RegisterValueChangedCallback(evt =>
            {
                choiceNode.choices[index] = evt.newValue;
                if (index < choicePorts.Count)
                    choicePorts[index].portName = evt.newValue;
            });

            choiceFields.Add(field);

            // 删除按钮
            var deleteButton = new Button(() => RemoveChoice(index)) { text = "删除" };
            deleteButton.style.marginLeft = 5;

            row.Add(field);
            row.Add(deleteButton);
            choicesContainer.Add(row);

            // 创建输出端口
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
            port.portName = choiceNode.choices[i];
            port.userData = index;
            outputContainer.Add(port);
            choicePorts.Add(port);
        }

        RefreshPorts();
    }

    private void AddChoice(string newChoice)
    {
        choiceNode.choices.Add(newChoice);
        choiceNode.nextNodes.Add("");
        RefreshChoices();
        RefreshExpandedState();
    }

    private void RemoveChoice(int index)
    {
        if (index < 0 || index >= choiceNode.choices.Count)
            return;

        // 断开相关连线
        if (index < choicePorts.Count)
        {
            var port = choicePorts[index];
            foreach (var edge in port.connections.ToList())
            {
                edge.input?.Disconnect(edge);
                edge.output?.Disconnect(edge);
                edge.parent?.Remove(edge);
            }
        }

        choiceNode.choices.RemoveAt(index);
        choiceNode.nextNodes.RemoveAt(index);

        RefreshChoices();
        RefreshExpandedState();
    }
}
#endif
