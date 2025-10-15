using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GamePanle : BasePanle
{

    [LabelText("消息Content位置 "), SerializeField]
    private RectTransform contentTrans;

    [LabelText("消息列表控制器"), SerializeField]
    private BubbleSlider bubbleSlider;
    [LabelText("怒气值滑动条"), SerializeField]
    private Slider angerSlider;

    //测试使用
    public bool isRight;


    //本关的 数据列表
    private List<LevelInfo> levelInfos = new List<LevelInfo>();
    //当前是消息的索引
    private int index = 0;

    private Coroutine currentCoroutine; //当前协程

    [SerializeField]
    private float currentAngerValue;

    //初始读取数据
    override protected void Awake()
    {
        base.Awake();
        index = 0;
    }
    void Start()
    {
        levelInfos = GameDataMgr.Instance.levelInfos;
        Debug.Log("当前关卡数据：" + levelInfos.Count);

        currentAngerValue = AngerMeterSystem.Instance.CurrentAngerValue;
    }
    private void Update()
    {
        //TODO:优化
        currentAngerValue = AngerMeterSystem.Instance.CurrentAngerValue;
        //更新怒气条的值
        angerSlider.DOValue(currentAngerValue * 0.01f, 0.1f);

    }
    private void FixedUpdate()
    {

    }

    //发送 消息
    public void SendMassage()//默认是左侧消息
    {
        currentCoroutine = StartCoroutine(RealSendMassage());
    }
    private IEnumerator RealSendMassage()
    {
        //TODO：数据加载优化
        if(index >= levelInfos.Count)
        {
            Debug.Log("数据还没有加载完毕");
               yield break;
        }
        //取出当前关卡数据
        LevelInfo levelInfo = levelInfos[index];

        if (levelInfo.isBugShow)
        {

            Debug.Log("显示bug");
            //TODO：开始游戏
            EventCenter.Instance.EventTrigger(E_EventType.E_PopupBugGame);

            //等待1帧再进行发消息 //TODO：这里先给几秒方便测试
            yield return new WaitForSeconds(2);
            
        }
        
        //创建消息
        if (isRight)
        {
            ABResMgr.Instance.LoadResAsync<GameObject>("ui", "RightContext", (res) =>
            {
                GameObject obj = GameObject.Instantiate(res);
                //设置消息内容
                Meassage meassage = obj.GetComponent<Meassage>();
                meassage.SetMassage(levelInfo.avacter, levelInfo.context);
                //设置位置
                bubbleSlider.AddMessage(obj, isRight);
            });
        }
        else
        {
            ABResMgr.Instance.LoadResAsync<GameObject>("ui", "LeftContext", (res) =>
            {
                GameObject obj = GameObject.Instantiate(res);
                //设置消息内容
                Meassage meassage = obj.GetComponent<Meassage>();
                meassage.SetMassage(levelInfo.avacter, levelInfo.context);
                //设置位置
                bubbleSlider.AddMessage(obj, isRight);
            });
        }

        //怒气值增加
        AngerMeterSystem.Instance.AddAngerValue(levelInfo.AngleValue);
        
        //当值大于1
        if (currentAngerValue > 100)
        {
            Debug.Log("游戏失败");
            //隐藏面板
            UIMgr.Instance.Hide<GamePanle>();
            //清空数据记录
            ClearData();
            //跳转到失败界面
            UIMgr.Instance.Show<EndPanle>().ChangeTipText("YOU LOSE");

            yield break;
        }

        yield return new WaitForSeconds(levelInfo.Intervaltime);
        index++;
        yield return RealSendMassage();
    }
    /// <summary> 清楚数据
    /// 
    /// </summary>
    private void ClearData()
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
        currentAngerValue = 0;
        index = 0;
        bubbleSlider.ClearMessages();
        AngerMeterSystem.Instance.ResetAngerValue();
    }

    public override void Showme(bool isFade = true)
    {
        base.Showme(isFade);
        SendMassage();
    }

}
