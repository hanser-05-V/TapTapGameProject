using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Data/PopUpData")]
public class PopupData : BugData
{
    [LabelText("Bug类型:弹窗Bug"), ReadOnly]
    public E_BugType bugType = E_BugType.PopupBugGame;
    [LabelText("关闭叉的类型")]
    public E_FockType fockType;
    [LabelText("弹窗数量")]
    public E_PopCount popCount;
    [LabelText("弹窗生长方向")]
    public E_PopGrowDir growDir;
    [LabelText("弹窗大小")]
    public E_PopSize popSize;
    [LabelText("弹窗位置类型")]
    public E_PopTransType popTransType;
    [LabelText("一次生成弹窗数量")]
    public int popNum = 3;
    [LabelText("生长弹窗的间隔时间")]
    public float growInterval = 0.5f;
    [LabelText("弹窗间隔时间")]
    public float popInterval = 0.15f;
    [LabelText("Bug消除减少的怒气值")]
    public float reduceValue = 5;
}
