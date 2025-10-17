using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于传递 弹窗游戏所需信息
public class PopGameInfo
{
    public BugData bugData;
    public List<RectTransform> popRectTeansList;

    public PopGameInfo(BugData bugData, List<RectTransform> popRectTeansList)
    {
        this.bugData = bugData;
        this.popRectTeansList = popRectTeansList;
    }
}
