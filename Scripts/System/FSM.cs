using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    private IState currentState; // 当前状态

    /// <summary> 初始化状态
    /// 
    /// </summary>
    /// <param name="beginState"></param>
    public void InitState(IState beginState)
    {
        currentState = beginState;
        currentState.OnEnter();
    }
    
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        else
        {
            Debug.LogError("当前状态为空，无法切换");
        }

        currentState = newState;
        currentState.OnEnter();

    }
}
