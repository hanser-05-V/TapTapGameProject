#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor;
public class DialogueGraphView : GraphView
{
    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph")); // 加载样式（可选）

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        // 添加右键菜单
        this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));

        // **关键：添加拖拽连线的操作器**
        this.nodeCreationRequest += context =>
        {
            Vector2 mousePos = context.screenMousePosition;
            // 可实现节点创建逻辑
        };

        // 监听图变化事件（创建/删除连线）
        this.graphViewChanged += OnGraphViewChanged;
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        // 先把 ports 转成列表
        var portList = ports.ToList();

        foreach (var port in portList)
        {
            if (port != startPort && port.direction != startPort.direction && port.node != startPort.node)
            {
                compatiblePorts.Add(port);
            }
        }

        return compatiblePorts;
    }
    // 右键菜单，创建各种节点
    private void BuildContextMenu(ContextualMenuPopulateEvent evt)
    {
        Vector2 mousePos = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        evt.menu.AppendAction("创建开始节点", action => CreateStartNode(mousePos));
        evt.menu.AppendAction("创建对话节点", action => CreateTextNode("对话节点", mousePos));
        evt.menu.AppendAction("创建选项节点", action => CreateChoiceNode("选项节点", mousePos));
        evt.menu.AppendAction("创建结束节点", action => CreateEndNode(mousePos));
        evt.menu.AppendAction("创建条件节点", action => CreateConditionNode("条件判断", mousePos));
        evt.menu.AppendAction("创建/委托/暂停", action => CreatePaus(mousePos));
    }

    #region 节点创建方法

    public void CreateTextNode(string nodeName, Vector2 position)
    {
        var nodeData = ScriptableObject.CreateInstance<DialogueTextNode>();
        nodeData.name = nodeName;
        nodeData.nodeGUID = Guid.NewGuid().ToString();
        nodeData.speakerName = "Say";
        nodeData.dialogueText = "";
        nodeData.position = position;

        var nodeView = new DialogueNodeView(nodeData);
        nodeView.SetPosition(new Rect(position, new Vector2(250, 150)));

        AddElement(nodeView);
    }

    public void CreateChoiceNode(string nodeName, Vector2 position)
    {
        var nodeData = ScriptableObject.CreateInstance<DialogueChoiceNode>();
        nodeData.name = nodeName;
        nodeData.nodeGUID = Guid.NewGuid().ToString();
        nodeData.position = position;

        var nodeView = new DialogueChoiceNodeView(nodeData);
        nodeView.SetPosition(new Rect(position, new Vector2(250, 150)));

        AddElement(nodeView);
    }

    public void CreateStartNode(Vector2 position)
    {
        var node = ScriptableObject.CreateInstance<DialogueStartNode>();
        node.name = "开始节点";
        node.position = position;
        node.nodeGUID = Guid.NewGuid().ToString();

        var nodeView = new DialogueStartNodeView(node);
        AddElement(nodeView);
    }

    public void CreateEndNode(Vector2 position)
    {
        var node = ScriptableObject.CreateInstance<DialogueEndNode>();
        node.name = "结束节点";
        node.position = position;
        node.nodeGUID = Guid.NewGuid().ToString();

        var nodeView = new DialogueEndNodeView(node);
        AddElement(nodeView);
    }
    public void CreateConditionNode(string nodeName, Vector2 position)
    {
        var nodeData = ScriptableObject.CreateInstance<DialogueConditionNode>();
        nodeData.name = nodeName;
        nodeData.nodeGUID = Guid.NewGuid().ToString();
        nodeData.position = position;

        var nodeView = new DialogueConditionNodeView(nodeData);
        nodeView.SetPosition(new Rect(position, new Vector2(250, 150)));

        AddElement(nodeView);
    }
    //暂停节点
    public void CreatePaus(Vector2 pos)
    {
        var data = ScriptableObject.CreateInstance<DialoguePauseNode>();
        data.nodeGUID = Guid.NewGuid().ToString();
        data.position = pos;

        var view = new DialoguePauseNodeView(data);
        view.SetPosition(new Rect(pos, new Vector2(250, 160)));
        AddElement(view);
    }
    #endregion

    // 监听GraphView的变化，比如连线创建或断开
    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        if (change.edgesToCreate != null)
        {
            foreach (var edge in change.edgesToCreate)
            {
                var fromView = edge.output.node as BaseNodeView;
                var toView = edge.input.node as BaseNodeView;

                if (fromView == null || toView == null)
                    continue;

                // 选项节点连接处理
                if (fromView is DialogueChoiceNodeView choiceView)
                {
                    if (edge.output.userData is int choiceIndex)
                    {
                        var choiceNode = choiceView.choiceNode;
                        while (choiceNode.nextNodes.Count <= choiceIndex)
                            choiceNode.nextNodes.Add("");
                        choiceNode.nextNodes[choiceIndex] = toView.Data.nodeGUID;
                    }
                }
                // 条件判断节点连接处理
                else if (fromView is DialogueConditionNodeView conditionView)
                {
                    int portIndex = conditionView.OutputPorts.IndexOf(edge.output);
                    if (portIndex >= 0)
                    {
                        var conditionNode = conditionView.ConditionNode;
                        while (conditionNode.nextNodes.Count <= portIndex)
                            conditionNode.nextNodes.Add("");
                        conditionNode.nextNodes[portIndex] = toView.Data.nodeGUID;
                    }
                }
                // 普通节点连接处理（只有一个输出）
                else
                {
                    if (!fromView.Data.nextNodes.Contains(toView.Data.nodeGUID))
                        fromView.Data.nextNodes.Add(toView.Data.nodeGUID);
                }
            }
        }

        if (change.elementsToRemove != null)
        {
            foreach (var el in change.elementsToRemove)
            {
                if (el is Edge edge)
                {
                    var fromView = edge.output.node as BaseNodeView;
                    var toView = edge.input.node as BaseNodeView;

                    if (fromView == null || toView == null)
                        continue;

                    // 选项节点断开处理
                    if (fromView is DialogueChoiceNodeView choiceView)
                    {
                        if (edge.output.userData is int choiceIndex)
                        {
                            var choiceNode = choiceView.choiceNode;
                            if (choiceIndex < choiceNode.nextNodes.Count)
                                choiceNode.nextNodes[choiceIndex] = "";
                        }
                    }
                    // 条件判断节点断开处理
                    else if (fromView is DialogueConditionNodeView conditionView)
                    {
                        int portIndex = conditionView.OutputPorts.IndexOf(edge.output);
                        if (portIndex >= 0 && portIndex < conditionView.ConditionNode.nextNodes.Count)
                        {
                            conditionView.ConditionNode.nextNodes[portIndex] = "";
                        }
                    }
                    // 普通节点断开处理
                    else
                    {
                        fromView.Data.nextNodes.Remove(toView.Data.nodeGUID);
                    }
                }
            }
        }

        return change;
    }
    #region 保存与加载

    public void SaveGraph(DialogueGraphData graphData)
    {
        // 清空旧节点
        foreach (var oldNode in graphData.allNodes)
        {
            if (oldNode != null)
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(oldNode);
        }

        graphData.allNodes.Clear();

        // 添加所有新节点并保存位置
        foreach (var nodeView in this.nodes.ToList())
        {
            if (nodeView is BaseNodeView baseNodeView)
            {
                var node = baseNodeView.Data;

                // 记录位置（GraphView 的 Rect）
                node.position = baseNodeView.GetPosition().position;

                // 添加为子资源
                UnityEditor.AssetDatabase.AddObjectToAsset(node, graphData);

                graphData.allNodes.Add(node);
            }
        }

    #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(graphData);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    #endif
    }

    public void LoadGraph(DialogueGraphData graphData)
    {
        DeleteElements(graphElements.ToList());

        Dictionary<string, BaseNodeView> nodeMap = new Dictionary<string, BaseNodeView>();

        // 创建节点视图
        foreach (var nodeData in graphData.allNodes)
        {
            BaseNodeView view = null;
            switch (nodeData)
            {
                case DialogueStartNode s:
                    view = new DialogueStartNodeView(s);
                    break;
                case DialogueTextNode t:
                    view = new DialogueNodeView(t);
                    break;
                case DialogueChoiceNode c:
                    view = new DialogueChoiceNodeView(c);
                    break;
                case DialogueEndNode e:
                    view = new DialogueEndNodeView(e);
                    break;
                case DialogueConditionNode cond:
                    view = new DialogueConditionNodeView(cond);
                    break;
                case DialoguePauseNode paus:
                    view = new DialoguePauseNodeView(paus);
                    break;
                case DialogueDelegateNode del:
                    view = new DialogueDelegateNodeView(del);
                    break;
            }

            if (view != null)
            {
                // ✅ 恢复保存的坐标
                view.SetPosition(new Rect(nodeData.position, new Vector2(250, 150)));

                AddElement(view);
                nodeMap[nodeData.nodeGUID] = view;
            }
        }

        // 刷新选项节点的端口（确保 choicePorts 初始化）
        foreach (var nodeView in nodeMap.Values)
        {
            if (nodeView is DialogueChoiceNodeView choiceNodeView)
            {
                choiceNodeView.RefreshChoices();
            }
        }

        // 创建连线
        foreach (var nodeData in graphData.allNodes)
        {
            if (!nodeMap.TryGetValue(nodeData.nodeGUID, out var fromView)) continue;
            if (nodeData.nextNodes == null) continue;

            for (int i = 0; i < nodeData.nextNodes.Count; i++)
            {
                string targetGuid = nodeData.nextNodes[i];
                if (string.IsNullOrEmpty(targetGuid)) continue;
                if (!nodeMap.TryGetValue(targetGuid, out var toView)) continue;

                Port outputPort = null;

                // 获取输出端口
                if (fromView is DialogueChoiceNodeView choiceView)
                {
                    if (i < choiceView.choicePorts.Count)
                    {
                        outputPort = choiceView.choicePorts[i];
                    }
                    else
                    {
                        Debug.LogWarning($"加载连线失败：选项节点输出索引{i}超出范围");
                        continue;
                    }
                }
                else if (fromView is DialogueConditionNodeView conditionView)
                {
                    if (i < conditionView.OutputPorts.Count)
                    {
                        outputPort = conditionView.OutputPorts[i]; // 0: True, 1: False
                    }
                    else
                    {
                        Debug.LogWarning($"加载连线失败：条件节点输出索引{i}超出范围");
                        continue;
                    }
                }
                else
                {
                    // 默认节点只有一个输出端口
                    if (fromView.OutputPorts != null && fromView.OutputPorts.Count > 0)
                    {
                        outputPort = fromView.OutputPorts[0];
                    }
                    else
                    {
                        Debug.LogWarning($"加载连线失败：普通节点无有效输出端口");
                        continue;
                    }
                }

                var inputPort = toView.InputPort;

                if (outputPort != null && inputPort != null)
                {
                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
                else
                {
                    Debug.LogWarning("加载连线失败：端口为 null");
                }
            }
        }
    }

    #endregion

    
}

// 辅助类：实现 IEdgeConnectorListener，处理连线拖拽事件
public class MyEdgeConnectorListener : IEdgeConnectorListener
{
    private DialogueGraphView graphView;

    public MyEdgeConnectorListener(DialogueGraphView graphView)
    {
        this.graphView = graphView;
    }

    // 用户拖拽连线放到空白区域时触发
    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        // 可自行处理：比如删除临时连线等
    }

    // 用户拖拽连线成功连接两个端口时触发
    public void OnDrop(GraphView graphView, Edge edge)
    {
        Debug.Log($"连线成功：{edge.output.node.title} -> {edge.input.node.title}");
        // 这里也可以做额外校验或者逻辑处理
    }

    
}
#endif
