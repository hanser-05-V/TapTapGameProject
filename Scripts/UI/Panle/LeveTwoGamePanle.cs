using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeveTwoGamePanle : BasePanle
{
    [FoldoutGroup("关卡面板配置")]
    [LabelText("设置按钮"), BoxGroup("关卡面板配置/按钮配置", false), SerializeField]
    private Button settingBtn;
    [LabelText("签到按钮"), BoxGroup("关卡面板配置/按钮配置", false), SerializeField]
    private Button gameOverBtn;
    //TODO:位置配置优化
    [FoldoutGroup("位置配置"), BoxGroup("位置配置/学生说话位置", false), SerializeField]
    private List<RectTransform> studentTalkList = new List<RectTransform>();
    [FoldoutGroup("位置配置"), BoxGroup("位置配置/老师说话位置", false), SerializeField]
    private List<RectTransform> teacherTalkList = new List<RectTransform>();
    [LabelText("学生说话位置"),]

    [FoldoutGroup("游戏运行加载数据"), BoxGroup("游戏运行加载数据/数据", false), SerializeField]
    //关卡二数据
    private List<LeveTwoInfo> leveTwoDataList = new List<LeveTwoInfo>();
    //测试持有
    public Image imageTalkBubble;
    public TextMeshProUGUI talkContent;

    [LabelText("测试点数文本")]
    public TextMeshProUGUI debupPointText;
    // [LabelText("总测试点数"), ReadOnly]
    // public float totalDebugPoint;
    [LabelText("当前测试点数")]
    public float currentDebugPoint;

    //当前是否已经开始游戏
    // public bool isGameStart = false; //TODO:测试公开
    // //当前Bug是否已经全部消除
    // public bool isGameOver = false;//TODO:测试公开

    private float lastDebugPoint; //上一次测试点数

    private int index = 0; //当前列表数据索引
    protected override void Awake()
    {
        base.Awake();
        imageTalkBubble.gameObject.SetActive(false);
    }
    private void Start()
    {
        //读取数据
        leveTwoDataList = GameDataMgr.Instance.LeveTwoData;
        Debug.Log("关卡二数据" + leveTwoDataList.Count);

        //绑定事件
        settingBtn.onClick.AddListener(OnSetButtonClick);
        gameOverBtn.onClick.AddListener(OnGameOverButtonClick);

        // //Debug点数 相关
        // totalDebugPoint = GameDataMgr.Instance.tatalDebugPoint;
        // currentDebugPoint = totalDebugPoint;
        // lastDebugPoint = currentDebugPoint;
        //更新显示
        debupPointText.text = currentDebugPoint.ToString();
    }
    //TODO：测试使用update 每帧检测变化 ，后续协程进行优化
    protected void Update()
    {
        //TODO:优化性能
        currentDebugPoint = GameDataMgr.Instance.currentDebugPoint;
        //避免每帧都更新文字
        if(currentDebugPoint != lastDebugPoint)
        {
            //更新显示
            debupPointText.text = currentDebugPoint.ToString();
            lastDebugPoint = currentDebugPoint;
        }
    }

    #region  按钮事件相关
    //设置按钮事件
    private void OnSetButtonClick()

    {
        Debug.Log("设置按钮点击");
    }

    //签到按钮事件
    private void OnGameOverButtonClick()
    {
        Debug.Log("签到按钮点击");
        
        if (GameDataMgr.Instance.isGameStart && GameDataMgr.Instance.isGameOver)
        {
            //TODO:游戏通过逻辑
            Debug.Log("游戏通过");
        }
    }
    #endregion
  
    //显示开始游戏 //TODO:测试公开
    public void StartGame()
    {
   
        StartCoroutine(RealStartGame());
    }
    private IEnumerator RealStartGame()
    {
        //读取数据
        LeveTwoInfo leveTwoData = leveTwoDataList[index];

        //是否出现Bug 开始游戏
        if (leveTwoData.isBugShow)
        {
            Debug.Log("出现Bug 开始游戏");
            //加载Bug数据
            BugData bugData = ResMgr.Instance.LoadRes<BugData>("Data/" + leveTwoData.id + "/" + leveTwoData.DataName);
            yield return bugData; //等待确保数据加载完毕
            //触发Bug事件 (根据ID 判断创建哪一个游戏)
            if (leveTwoData.id == 1)
            {
                //触发弹窗游戏
                EventCenter.Instance.EventTrigger(E_EventType.E_PopupBugGame, bugData);

                //游戏开始标志
                GameDataMgr.Instance.isGameStart = true;
            }
            else if (leveTwoData.id == 2)
            {
                //触发爬行游戏
                EventCenter.Instance.EventTrigger(E_EventType.E_CrawBugGame, bugData);
                
                //游戏开始标志
                GameDataMgr.Instance.isGameStart = true;
            }
        }
        //发送消息 
        SendMassage(leveTwoData.studentTalk, leveTwoData.teacherTalk); //TODO:出现位置优化
        yield return new WaitForSeconds(1f); //等待几秒过后 把对话面板隐藏

         //TODO：测试先硬编码加载
        imageTalkBubble.gameObject.SetActive(false);

        // UIMgr.Instance.Hide<LeveTwoTalkPanle>();  //TODO:缓存池子优化
        //发送消息
        yield return new WaitForSeconds(leveTwoData.InverTime);
        index++;
        if (index > leveTwoDataList.Count-1)
        {
            Debug.Log("读取结束");
            yield break;
        }
        yield return RealStartGame();
    }
    /// <summary> 显示对话 
    /// 
    /// </summary>
    /// <param name="studentTalk"> 学生说的话 </param>
    /// <param name="teacherTalk"> 老师的话 </param>
    private void SendMassage(string studentTalk, string teacherTalk)
    {
        imageTalkBubble.gameObject.SetActive(true);
        //判断是学生还是老师话
        if (studentTalk != "")
        {
            //TODO：测试先硬编码加载
            imageTalkBubble.rectTransform.anchoredPosition = studentTalkList[0].anchoredPosition;
            talkContent.text = studentTalk;

            // UIMgr.Instance.Show<LeveTwoTalkPanle>(false).ChangeTalk(studentTalkList[0], studentTalk); //TODO:出现位置优化
        }
        else if (teacherTalk != "")
        {
            imageTalkBubble.rectTransform.anchoredPosition = teacherTalkList[0].anchoredPosition;
            talkContent.text = teacherTalk;


             //TODO：测试先硬编码加载
            // UIMgr.Instance.Show<LeveTwoTalkPanle>(false).ChangeTalk(studentTalkList[0], teacherTalk); //TODO:出现位置优化
        }
    }
}
