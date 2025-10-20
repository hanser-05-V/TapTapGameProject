using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    public IState CurrentState { get; private set; } // 当前状态

    /// <summary> 初始化状态
    /// 
    /// </summary>
    /// <param name="beginState"></param>
    public void InitState(IState beginState)
    {
        CurrentState = beginState;
        CurrentState.OnEnter();
    }
    
    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        else
        {
            Debug.LogError("当前状态为空，无法切换");
        }

        CurrentState = newState;
        CurrentState.OnEnter();

    }
}
