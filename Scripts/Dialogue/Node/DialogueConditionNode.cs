using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ConditionType
{
    是否交易成功,
    交易成功次数,
    金币数量对比,
    仓库是否有指定ID武器,
    检查仓库是否为空,
    仓库中是否有指定标签武器
}
public enum ComparisonType{
    大于等于,
    大于,
    等于,
    小于等于,
    小于,
    不等于
}

[System.Serializable]
[CreateAssetMenu(fileName = "NewConditionNode", menuName = "Dialogue/DialogueConditionNode")]
public class DialogueConditionNode : DialogueNode
{
    public string conditionKey; // 条件名（比如“HasKey”）
    public int haveTargetID; //查找仓库是否有指定的武器名
    public int[] HaveTargetTag; //检查仓库是否有指定的标签的武器
    public ConditionType branchType = ConditionType.是否交易成功;
    public ComparisonType comparisonOperator = ComparisonType.大于等于;
    public override void OnEnter()
    {
        // 这里不用做什么，控制权交给 DialogueContral 的流程管理器
    }

    public override DialogueNode GetNextNode(Dictionary<string, DialogueNode> allNodes)
    {
        bool conditionResult = CheckCondition();
        string nextID = null;
        if (conditionResult)
        {
            if (nextNodes.Count > 0)
                nextID = nextNodes[0]; // True 分支
        }
        else
        {
            if (nextNodes.Count > 1)
                nextID = nextNodes[1]; // False 分支
        }

        

        if (!string.IsNullOrEmpty(nextID) && allNodes.TryGetValue(nextID, out var nextNode))
        {
            
            return nextNode;
        }

        
        return null;
    }

    private bool CheckCondition()
    {
        return true;
    }
}
