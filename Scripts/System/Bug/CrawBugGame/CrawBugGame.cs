using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CrawBugGame : MonoBehaviour
{
    [FoldoutGroup("游戏数据")]
    [LabelText("单出位置爬虫数据"), BoxGroup("游戏数据/数据", false), SerializeField]
    private CrawData crawSingleData;
    [LabelText("多出位置爬虫群体数据"), BoxGroup("游戏数据/数据", false), SerializeField]
    private CrawListData crawGroupData;

    [FoldoutGroup("范围相关")]
    [LabelText("生成位置"), BoxGroup("范围相关/生成位置", false), SerializeField]
    private List<Transform> crawPosList = new List<Transform>();


    [FoldoutGroup("游戏运行数据")]
    [LabelText("爬虫对象列表"), BoxGroup("游戏运行数据/爬虫对象", false), SerializeField]
    private List<Craw> crawList = new List<Craw>();
    [LabelText("爬虫对象列表"), BoxGroup("游戏运行数据/爬虫数据", false), SerializeField]
    private List<CrawData> crawDataList = new List<CrawData>();
    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener<BugData>(E_EventType.E_CrawBugGame, StartGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener<BugData>(E_EventType.E_CrawBugGame, StartGame);
    }
    //测试使用
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            foreach (var craw in crawList)
            {
                PoolMgr.Instance.PushObj(craw.gameObject);
            }

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameDataMgr.Instance.isGameOver = true;
        }
    }
    private void StartGame(BugData bugData)
    {
        //解析 数据类型  赋值Data
        if (bugData is CrawData)
            crawSingleData = bugData as CrawData;
        else if (bugData is CrawListData)
            crawGroupData = bugData as CrawListData;
        Debug.Log("开始爬虫游戏");
        CreatCraw();
    }

    //创建爬虫
    private void CreatCraw()
    {
        if (crawGroupData != null)
            crawDataList.AddRange(crawGroupData.crawList);
        if (crawSingleData != null)
            crawDataList.Add(crawSingleData);
        if (crawDataList.Count != 0)
        {
            //
            // foreach (var crawData in crawDataList)
            // {
            //     for (int i = 0; i < crawData.crawNum; i++)
            //     {
            //         //创建爬虫
            //         GameObject crawObj = PoolMgr.Instance.GetObj("Craw");
            //         Craw craw = crawObj.GetComponent<Craw>();
            //         craw.SetCrawData(crawData);
            //         crawObj.transform.position = SetCrawPos(crawData);

            //         //添加到列表
            //         crawList.Add(craw);
            //     }

            // }
            StartCoroutine(CreatCrawCoroutine());
        }
        else
        {
            Debug.LogError("爬虫数据表为null");
        }

    }
    private IEnumerator CreatCrawCoroutine()
    {
        foreach (var crawData in crawDataList)
        {
            for (int i = 0; i < crawData.crawNum; i++)
            {
                //创建爬虫
                GameObject crawObj = PoolMgr.Instance.GetObj("Craw");
                Craw craw = crawObj.GetComponent<Craw>();
                craw.SetCrawData(crawData);
                crawObj.transform.position = SetCrawPos(crawData);

                //添加到列表
                crawList.Add(craw);
                yield return new WaitForSeconds(0.2f);
            }

        }
    }
    //设置爬虫初始位置
    private Vector3 SetCrawPos(CrawData crawData)
    {
        switch (crawData.posType)
        {
            case E_CrawPosType.TopCenter: return crawPosList[0].position;

            case E_CrawPosType.TopLeft: return crawPosList[1].position;

            case E_CrawPosType.TopRight: return crawPosList[2].position;

            case E_CrawPosType.MiddLeLeft: return crawPosList[3].position;

            case E_CrawPosType.MiddleRight: return crawPosList[4].position;

            case E_CrawPosType.DownLeft: return crawPosList[5].position;

            case E_CrawPosType.DownRight: return crawPosList[6].position;

            case E_CrawPosType.DownCenter: return crawPosList[7].position;

            default: return Vector3.zero;
        }

    }
    
    
}

