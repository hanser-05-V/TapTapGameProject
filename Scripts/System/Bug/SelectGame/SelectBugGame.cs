using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SelectBugGame : MonoBehaviour
{

    [LabelText("右"), SerializeField]
    private GameObject pollRight;
    
    [LabelText("左"), SerializeField]
    private GameObject pollTop;
    [LabelText("上"), SerializeField]
    private GameObject pollLeft;
    [LabelText("下"), SerializeField]
    private GameObject pollDown;
    // [LabelText("污染物数量")]
    // private int pollutionNum = 6;
    [LabelText("Right路线"), SerializeField]
    private List<Transform> rightPosList;

    [LabelText("Top路线"), SerializeField]
    private List<Transform> topPoslist;

    [LabelText("Left路线"), SerializeField]
    private List<Transform> LeftPoslist;
    [LabelText("Down路线"), SerializeField]
    private List<Transform> DownPoslist;

    private List<PollData> pollDataList = new List<PollData>();

    [SerializeField] //测试公开
    private PollListData pollListData;
    [SerializeField] //测试公开
    private PollData pollData;

    public BugData bugData1; //后续传入 //测试使用

    private int index = 0;
    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener<BugData>(E_EventType.E_SelectBugGame, StartGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener<BugData>(E_EventType.E_SelectBugGame, StartGame);
    }
    private void Start()
    {   
        //测试直接开始
        StartGame(bugData1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame(pollListData);
        }
    }
    public void StartGame(BugData bugData)
    {
        if (bugData is PollData)
            pollListData = bugData as PollListData;
        else if (bugData is PollListData)
            pollData = bugData as PollData;
   
        CreatPoll();
    }
    private void CreatPoll()
    {
        //加入Data数据列表
        if (pollListData != null)
            pollDataList.AddRange(pollListData.pollList);
        if (pollData != null)
            pollDataList.Add(pollData);
        if (pollDataList.Count != 0) //有数据才开始
        {
            StartCoroutine(CreatePollutionCoroutine());
        }
    }
    private IEnumerator CreatePollutionCoroutine()
    {
        //int index = 0; //TODO：固定路线生成
        foreach( var data in pollDataList)
        {
            for(int i=0; i<data.pollNUmber; i++)
            {
                //创建污染物
                CreatePollution(data);
                yield return new WaitForSeconds(data.pollInterval);
            }
        }
    }

    private void CreatePollution(PollData data)
    {
       
        switch (data.pollType)
        {
            case E_PollType.Top:
                // pollutionPrefab = pollTop;
                //实例化
                GameObject pollObj = PoolMgr.Instance.GetObj("TopPoll");
                //设置位置
                Transform pos = topPoslist[Random.Range(0, topPoslist.Count - 1)];
                pollObj.transform.position = new Vector3(pos.position.x, pos.position.y, 0);
                break;
            case E_PollType.Right:
                // pollutionPrefab = pollRight;

                GameObject pollObj1 =PoolMgr.Instance.GetObj("RightPoll");
                //设置位置
                Transform pos1 = rightPosList[Random.Range(0, rightPosList.Count - 1)];
                pollObj1.transform.position = new Vector3(pos1.position.x, pos1.position.y, 0);

                break;
            case E_PollType.Left:
                // pollutionPrefab = pollLeft;

                 GameObject pollObj2 = PoolMgr.Instance.GetObj("LeftPoll");
                //设置位置
                Transform pos2 = LeftPoslist[Random.Range(0, LeftPoslist.Count - 1)];
                pollObj2.transform.position = new Vector3(pos2.position.x, pos2.position.y, 0);


                break;
            case E_PollType.Down:
                // pollutionPrefab = pollDown;

                GameObject pollObj3 = PoolMgr.Instance.GetObj("DownPoll");
                //设置位置
                Transform pos3 = DownPoslist[Random.Range(0, DownPoslist.Count - 1)];
                pollObj3.transform.position = new Vector3(pos3.position.x, pos3.position.y, 0);

                break;
        }
       
        //区分路线


    }
}
