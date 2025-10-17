using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 游戏实体类 包含创建对象的公共属性/行为
/// 
/// </summary>
public class Enitity : MonoBehaviour
{

    // 状态机
    private FSM fSM;
    protected virtual  void Awake()
    {
        fSM = new FSM();
    }


}
