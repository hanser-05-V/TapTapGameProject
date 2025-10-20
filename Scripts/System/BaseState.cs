using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爬虫基本状态类
public class CrawBaseState : IState
{
    //状态机管理器
    protected FSM stateMechine { get; private set; }
    //当前状态机控制的角色
    protected Craw Craw { get; private set; }
    //虫子数据
    protected CrawData CrawData { get; private set; }
    //状态机bool 条件
    protected string AnimatorBoolName { get; private set; }
    public CrawBaseState(Craw craw, FSM stateMechine, string animatorBoolName)
    {
        Craw = craw;
        this.stateMechine = stateMechine;
        AnimatorBoolName = animatorBoolName;
    }

    public virtual void OnEnter()
    {
        //切换状态
        Craw.Animator.SetBool(AnimatorBoolName, true);
        CrawData = Craw.crawData;
    }

    public virtual  void OnExit()
    {
        //切换状态
        Craw.Animator.SetBool(AnimatorBoolName, false);
    }

    public virtual void OnUpdate()
    {

    }
}
