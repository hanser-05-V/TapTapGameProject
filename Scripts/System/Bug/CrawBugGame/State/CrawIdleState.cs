using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawIdleState : CrawBaseState
{

    //等待时间
    private float idleTime;

    public CrawIdleState(Craw craw, FSM stateMechine, string animatorBoolName) : base(craw, stateMechine, animatorBoolName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();

        idleTime = CrawData.idleTime;

        //重制状态
        Craw.isArriveOtherSide = false;

        //直线型无需等待，直接进入移动状态
        if(CrawData.moveType == E_CrawMoveType.Straight)
           idleTime = 0;
    }
    public override void OnExit()
    {
        base.OnExit();
        //重置idleTime
        idleTime = 0;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //逐帧减少时间
        idleTime -= Time.deltaTime;
        //等待时间结束，切换到移动状态
        if(idleTime <= 0)
        {
            Craw.ChangeState(E_CrawStateType.Move);
        }
    }
}
