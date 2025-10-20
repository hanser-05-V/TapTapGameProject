using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class CrawDieState : CrawBaseState
{
    public CrawDieState(Craw craw, FSM stateMechine, string animatorBoolName) : base(craw, stateMechine, animatorBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Craw.IsDied)
        {
            Debug.Log("放进池子");
            PoolMgr.Instance.PushObj(Craw.gameObject);
        }
    }
}
