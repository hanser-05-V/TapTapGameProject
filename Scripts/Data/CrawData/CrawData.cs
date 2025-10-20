using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CrawData", menuName = "Data/CrawData")]
public class CrawData : BugData
{
    [LabelText("Bug类型:爬虫Bug"), ReadOnly]
    public E_BugType bugType = E_BugType.CrawBugGame;

    [LabelText("爬虫出现位置")]
    public E_CrawPosType posType;

    [LabelText("爬虫运动类型")]
    public E_CrawMoveType moveType;

    [LabelText("爬虫运动等待时间"), ShowIf("@moveType==E_CrawMoveType.Romdam")]
    public float idleTime = 0.25f; //TODO:后面随机等待时间处理 
    [LabelText("爬虫移动时间"), ShowIf("@moveType==E_CrawMoveType.Romdam")]
    public float moveTime = 0.5f;
    [LabelText("爬虫运动角度类型")]
    public E_CrawMoveDir moveDir;
    [LabelText("爬虫运动速度")]
    public float moveSpeed =1 ;

    [LabelText("爬虫生成数量")]
    public int crawNum =1;
}
