using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class LeveOneGamePanle : BasePanle
{
    [FoldoutGroup("面板相关")]
    [LabelText("消息Content位置 "), BoxGroup("面板相关/组件相关",false),SerializeField]
    private RectTransform contentTrans;

    [LabelText("消息列表控制器"),BoxGroup("面板相关/组件相关",false), SerializeField]
    private BubbleSlider bubbleSlider;
    [LabelText("怒气值滑动条"), BoxGroup("面板相关/组件相关", false), SerializeField]
    private Slider angerSlider;
    [LabelText("设置按钮"), BoxGroup("面板相关/组件相关", false), SerializeField]
    private Button settingBtn;


    //测试使用
    public bool isRight;
    //本关的 数据列表
    private List<LevelOneInfo> leveOneInfos = new List<LevelOneInfo>();
    //当前是消息的索引
    private int index = 0;

    private Coroutine currentCoroutine; //当前协程
    private float currentAngerValue;
    private List<RectTransform> currentPosList; //当前刷新位置列表

    //初始读取数据
    override protected void Awake()
    {
        base.Awake();
       
        index = 0;
    }
    void Start()
    {
        leveOneInfos = GameDataMgr.Instance.levelData_1;
        Debug.Log("当前关卡数据：" + leveOneInfos.Count);
        currentAngerValue = AngerMeterSystem.Instance.CurrentAngerValue;

        settingBtn.onClick.AddListener(OnSetButtonClick);
    }
    private void OnSetButtonClick()
    {
        //打开 游戏内设置面板
    }
    private void Update()
    {
        //TODO:优化
        currentAngerValue = AngerMeterSystem.Instance.CurrentAngerValue;
        //更新怒气条的值
        angerSlider.DOValue(currentAngerValue * 0.01f, 0.1f);

    }

    //发送 消息
    public void SendMassage()//默认是左侧消息
    {
        StartCoroutine(RealSendMassage());
    }
    private IEnumerator RealSendMassage()
    {
        yield return new WaitForSeconds(1f); //先等待2 秒 确保数据加载完毕
        //取出当前关卡数据
        LevelOneInfo levelInfo = leveOneInfos[index];

        //是否为游戏结束标准
        if (levelInfo.id == 0)
        {
            //显示输入框 让玩家输入 (此时的 头像名字就为 提示输入信息)
            UIMgr.Instance.Show<InputPanle>().SetDefultInputData(levelInfo.avacter,levelInfo.context);
            yield break;//结束协程
        }

        //创建 发送 消息 (这里默认都是左侧发)
        SendMassage(levelInfo.avacter,levelInfo.context,levelInfo.isRight);

        //判断是否显示bug
        if (levelInfo.isBugShow)
        {
            Debug.Log("显示bug");
            //避免数据填写错误
            if (levelInfo.DataName == null)
                Debug.LogError("数据名称为空");

            //TODO：加载bug数据
            BugData bugData = ResMgr.Instance.LoadRes<BugData>("Data/" + levelInfo.id + "/" + levelInfo.DataName);

            yield return bugData;
            //确定进去游戏
            if (levelInfo.id == 1)
            {
                //TODO:优化 确定路线
                // E_PopTransType transType = (bugData as PopupData).popTransType;
                // switch (transType)
                // {
                //     case E_PopTransType.Random:
                //         currentPosList = RandomPosList;
                //         break;
                //     case E_PopTransType.RouteOne:
                //         currentPosList = RouteOne;
                //         break;
                //     case E_PopTransType.RouteTwo:
                //         currentPosList = RouteTwo;
                //         break;
                //     case E_PopTransType.RouteThree:
                //         currentPosList = RouteThree;
                //         break;
                // }
                //创建关卡一数据传入
                // PopGameInfo popGameInfo = new PopGameInfo(bugData, currentPosList);
              
                Debug.Log("触发弹窗游戏事件");
                EventCenter.Instance.EventTrigger(E_EventType.E_PopupBugGame, bugData);
            }
            else if(levelInfo.id == 2)
            {
                Debug.Log("触发爬虫游戏事件");
                //TODO：开始 爬虫 游戏
                EventCenter.Instance.EventTrigger(E_EventType.E_CrawBugGame, bugData);
            }

            //等待1帧再进行发消息 //TODO：这里先给几秒方便测试
            yield return null;
        }

        //怒气值增加
        AngerMeterSystem.Instance.AddAngerValue(levelInfo.AngleValue);

        //当值大于1
        if (currentAngerValue > 100)
        {
            Debug.Log("游戏失败");
            //隐藏面板
            UIMgr.Instance.Hide<LeveOneGamePanle>();
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

    /// <summary> 发送单条消息
    /// 
    /// </summary>
    /// <param name="avacterName"> 消息头像名字 </param>
    /// <param name="context"> 消息内容 </param>
    /// <param name="isRight"> 是否是右侧消息 默认不是 </param>
    public void SendMassage(string avacterName,string context,bool isRight = false)
    {
        if (isRight)
        {
            ABResMgr.Instance.LoadResAsync<GameObject>("ui", "RightContext", (res) =>
            {
                GameObject obj = GameObject.Instantiate(res);
                //设置消息内容
                Meassage meassage = obj.GetComponent<Meassage>();
                meassage.SetMassage(avacterName, context);
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
                meassage.SetMassage(avacterName,context);
                //设置位置
                bubbleSlider.AddMessage(obj, isRight);
            });
        }
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
