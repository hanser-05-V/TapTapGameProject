using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Craw : Enitity
{
    [LabelText("虫子数据"), SerializeField]  //TODO: 测试先进行公开
    public CrawData crawData;

    //状态机字典容器
    private Dictionary<E_CrawStateType, IState> crawStateDic = new Dictionary<E_CrawStateType, IState>();

    //当前是否到达另外一边
    public bool isArriveOtherSide = false;
    //当前是否超出显示范围
    public bool isOutDisplay = true; //创建的时候是在显示范围外边的
    public SpriteRenderer spriteRenderer;

    public bool IsDied { get; private set; } //死亡动画是否播放完毕

    [SerializeField] //测试公开
    public E_CrawMoveDir moveDir;

    protected override void Awake()
    {
        base.Awake();
        moveDir = crawData.moveDir;
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        spriteRenderer.color = Color.white; //初始化颜色
    }
    protected override void Start()
    {
        base.Start();
        if (crawData == null)
            Debug.LogError("CrawData is null");

        //TODO：添加虫子具体状态
        crawStateDic.Add(E_CrawStateType.Idle, new CrawIdleState(this, fSM, "Idle")); //静止状态
        crawStateDic.Add(E_CrawStateType.Move, new CrawMoveState(this, fSM, "Move")); //移动状态
        crawStateDic.Add(E_CrawStateType.Died, new CrawDieState(this, fSM, "Died")); //死亡状态
        //初始化状态机
        fSM.InitState(crawStateDic[E_CrawStateType.Idle]);

    }
    protected override void Update()
    {
        base.Update();
    }

    //外部实例化后 设置数据
    public void SetCrawData(CrawData crawData)
    {
        this.crawData = crawData;
    }

    //外部调用 切换状态
    public void ChangeState(E_CrawStateType stateType)
    {
        if (!crawStateDic.ContainsKey(stateType))
            Debug.LogError("Craw state not exist");
        fSM.ChangeState(crawStateDic[stateType]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string colliderTag = other.gameObject.tag;
        //比较虫子和碰撞对象的Tap是否相同
        if (this.gameObject.tag == colliderTag)
        {

        }
        else if (colliderTag == "Up" || colliderTag == "Down" || colliderTag == "Left" || colliderTag == "Right")
        {

            isArriveOtherSide = true;
            ChangeState(E_CrawStateType.Idle);
            //修改移动方向
            ChangeCrawDir(colliderTag);
            //动态修改Tag
            this.gameObject.tag = other.gameObject.tag;
        }

        // if(colliderTag == "ShowRange") //超出范围
        // {
        //     //上次碰撞是不是在范围内
        //     if (isOutDisplay) //出范围
        //     {   

        //         isOutDisplay = false;
        //         //隐藏起来
        //         spriteRenderer.color = Color.white;
        //     }
        //     else //进范围
        //     {
        //         //显示
        //         spriteRenderer.color = Color.green;
        //         isOutDisplay = true;
        //     }
        // }
    }

    private void ChangeCrawDir(string colliderTag)
    {
        switch (moveDir)
        {
            case E_CrawMoveDir.Right:
                //0-180 碰到 上
                if (colliderTag == "Up")
                {
                    moveDir = E_CrawMoveDir.Down;
                }
                else if (colliderTag == "Down")
                {
                    moveDir = E_CrawMoveDir.Up;
                }
                else
                {
                    //修改为左边
                    moveDir = E_CrawMoveDir.Left;
                }
                break;
            case E_CrawMoveDir.Left:
                //碰到 上
                if (colliderTag == "Up")
                {
                    moveDir = E_CrawMoveDir.Down;
                }
                else if (colliderTag == "Down")
                {
                    moveDir = E_CrawMoveDir.Up;
                }
                else
                {
                    //修改为右边
                    moveDir = E_CrawMoveDir.Right;
                }
                break;
            case E_CrawMoveDir.Up:
                //修改为下边
                if (colliderTag == "Right")
                {
                    moveDir = E_CrawMoveDir.Left;
                }
                else if (colliderTag == "Left")
                {
                    moveDir = E_CrawMoveDir.Right;
                }
                else
                {

                    moveDir = E_CrawMoveDir.Down;
                }
                break;
            case E_CrawMoveDir.Down:
                if (colliderTag == "Right")
                {
                    moveDir = E_CrawMoveDir.Left;
                }
                else if (colliderTag == "Left")
                {
                    moveDir = E_CrawMoveDir.Right;
                }
                else
                {

                    moveDir = E_CrawMoveDir.Up;
                }
                break;
            case E_CrawMoveDir.TopRight:
                moveDir = E_CrawMoveDir.DownLeft;
                break;
            case E_CrawMoveDir.TopLeft:
                moveDir = E_CrawMoveDir.DownRight;
                break;
            case E_CrawMoveDir.DownLeft:
                moveDir = E_CrawMoveDir.TopRight;
                break;
            case E_CrawMoveDir.DownRight:
                moveDir = E_CrawMoveDir.TopLeft;
                break;
        }
    }



    //动画事件 Animation Event
    private void CrawDiedTrigger()
    {
        IsDied = true;
    }



}
