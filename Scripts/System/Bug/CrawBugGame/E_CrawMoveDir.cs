using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CrawMoveDir
{
    /// <summary> 右边 （270 -90）
    ///  
    /// </summary>
    Right,

    /// <summary> 左边 （90 -270）
    /// 
    /// </summary>
    Left,

    /// <summary> 上边 （0 -180）
    /// 
    /// </summary>
    Up,

    /// <summary> 下边 （180 -360）
    /// 
    /// </summary>
    Down,
    /// <summary> 右上（0-90）
    /// 
    /// </summary>
    TopRight,
    /// <summary> 左上 (90 -180)
    /// 
    /// </summary>
    TopLeft,
    /// <summary> 下左（180-270）
    /// 
    /// </summary>
    DownLeft,
    /// <summary> 下右（270-360 ）
    /// 
    /// </summary>
    DownRight,
}
