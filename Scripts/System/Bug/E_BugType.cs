using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BugType
{

    Null,

    /// <summary> 弹窗类型的BUG    点击工具 （鼠标）
    /// 
    /// </summary>
    PopupBugGame,

    /// <summary> 爬行虫类型的BUG  -长按工具（电蚊拍）
    CrawBugGame,
    /// <summary> 框选类Bug   - 框选工具
    SelectBugGame,

    /// <summary> 输入类游戏  - 输入工具
    /// 
    /// </summary>
    InputBugGame,
}

