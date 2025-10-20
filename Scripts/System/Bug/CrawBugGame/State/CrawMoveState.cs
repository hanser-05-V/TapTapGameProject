using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CrawMoveState : CrawBaseState
{
    //移动策略
    private ICrawMoveStrategy moveStrategy;
    //移动时间
    private float moveTime;

    public CrawMoveState(Craw craw, FSM stateMechine, string animatorBoolName) : base(craw, stateMechine, animatorBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        moveTime = CrawData.moveTime;
        //根据配置选择移动策略 //TODO:后续拓展内容获取不同策略即可 现在都是通用的策略
        // moveStrategy = CrawStrategyFactory.GetMoveStrategy(CrawData.moveDir,Craw);
        moveStrategy = CrawStrategyFactory.GetGeneralMoveStrategy(Craw.moveDir, CrawData.moveType, Craw);
    }
    public override void OnExit()
    {
        base.OnExit();
        moveTime = 0;
        moveStrategy = null;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //逐帧减少时间
        moveTime -= Time.deltaTime;

        if (moveTime <= 0 && CrawData.moveType == E_CrawMoveType.Romdam) //只有Romdam 才会停止
        {
            //时间到 开始停住
            Craw.ChangeState(E_CrawStateType.Idle);
        }

        if (Craw.isArriveOtherSide)
        {
            Debug.Log("到达另一边");
            Craw.ChangeState(E_CrawStateType.Idle);
        }

        //根据移动策略移动虫子
        if(moveStrategy!= null) //避免退出状态过后 还在调用
            moveStrategy.Move(Craw, CrawData.moveType);

    }
}
