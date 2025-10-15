using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件类型  每个事件类型对应一个枚举值
public enum E_EventType
{

    /// <summary> 场景切换  - float 参数 表示加载进度
    /// 
    /// </summary>
    E_SceneLoadChange,

    /// <summary> 弹窗Bug游戏
    /// 
    /// </summary>
    E_PopupBugGame,

}
