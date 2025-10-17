using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爬虫基本状态类
public class CrawBaseState : IState
{
    //状态机管理器
    protected FSM FSM { get; private set; }
    //当前状态机控制的角色
    protected Craw Craw { get; private set; }
   
    public CrawBaseState(Craw craw, FSM fSM)
    {
        Craw = craw;
        FSM = fSM;
    }

    public virtual void OnEnter()
    {

    }

    public virtual  void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }
}
