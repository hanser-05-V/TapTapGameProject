#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueConditionNodeView : BaseNodeView
{
    public DialogueConditionNode ConditionNode => Data as DialogueConditionNode;

    private TextField conditionField;
    private IntegerField compareValueField;
    private IntegerField targetIDField;
    private TextField tagArrayField;

    public DialogueConditionNodeView(DialogueConditionNode node) : base(node)
    {
        title = "条件分支";
        viewDataKey = node.nodeGUID;
        SetPosition(new Rect(node.position, new Vector2(300, 200)));

        // 条件类型字段（选择后控制显示哪些字段）
        EnumField conditionTypeField = new EnumField("条件类型", node.branchType);
        conditionTypeField.RegisterValueChangedCallback(evt =>
        {
            node.branchType = (ConditionType)evt.newValue;
        });
        mainContainer.Add(conditionTypeField);

        // 比较类型字段
        EnumField comparisonField = new EnumField("比较方式", node.comparisonOperator);
        comparisonField.RegisterValueChangedCallback(evt =>
        {
            node.comparisonOperator = (ComparisonType)evt.newValue;
        });
        mainContainer.Add(comparisonField);

        // 条件名字段
        conditionField = new TextField("条件名")
        {
            value = node.conditionKey
        };
        conditionField.RegisterValueChangedCallback(evt =>
        {
            node.conditionKey = evt.newValue;
        });
        mainContainer.Add(conditionField);

        // 比较值字段（用于做过次数、金币数量）
        compareValueField = new IntegerField("比较值");
        compareValueField.value = int.TryParse(node.conditionKey, out var val) ? val : 0;
        compareValueField.RegisterValueChangedCallback(evt =>
        {
            node.conditionKey = evt.newValue.ToString(); // 如果你单独做了 int 字段也可以改这里
        });
        mainContainer.Add(compareValueField);

        // 目标武器ID字段
        targetIDField = new IntegerField("目标武器ID")
        {
            value = node.haveTargetID
        };
        targetIDField.RegisterValueChangedCallback(evt =>
        {
            node.haveTargetID = evt.newValue;
        });
        mainContainer.Add(targetIDField);

        // 标签数组字段（用 , 分隔）
        tagArrayField = new TextField("标签ID数组 (逗号分隔)")
        {
            value = string.Join(",", node.HaveTargetTag ?? new int[0])
        };
        tagArrayField.RegisterValueChangedCallback(evt =>
        {
            string[] parts = evt.newValue.Split(',');
            node.HaveTargetTag = parts
                .Where(p => int.TryParse(p.Trim(), out _))
                .Select(p => int.Parse(p.Trim()))
                .ToArray();
        });
        mainContainer.Add(tagArrayField);

        // 输出端口
        outputContainer.Clear();
        OutputPorts.Clear();

        var truePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
        truePort.portName = "True";
        OutputPorts.Add(truePort);
        outputContainer.Add(truePort);

        var falsePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
        falsePort.portName = "False";
        OutputPorts.Add(falsePort);
        outputContainer.Add(falsePort);

        RefreshPorts();
        RefreshExpandedState();

        
    }
}
#endif
