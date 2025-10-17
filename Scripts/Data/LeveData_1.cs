using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Data/PopUpData")]
public class PopupData : BugData
{
 
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
}
