using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickColorBugGame : MonoBehaviour
{

    [SerializeField]
    private BugData BugData; //后续依赖传入的bugData

    [LabelText("失去颜色数据列表"), SerializeField]
    private List<Transform> showPosTransList = new List<Transform>();

    [SerializeField]
    private MaskData maskData;
    [SerializeField]
    private MaskListData maskDatas;

    [LabelText("运行时候数据"),SerializeField]
    private List<MaskData> maskDataList; //游戏中运行获取的数据

    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener<BugData>(E_EventType.E_PickColorBugGame, StartPickGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener<BugData>(E_EventType.E_PickColorBugGame, StartPickGame);
    }

    private void Start()
    {
        //测试使用
        StartPickGame(BugData);
    }
    private void StartPickGame(BugData bugData)
    {
        //解析数据
        if (bugData is MaskData)
            maskData = bugData as MaskData;
        else if (bugData is MaskListData)
            maskDatas = bugData as MaskListData;

        CreateMask();
    }
    //创建失色层
    private void CreateMask()
    {
        //加载数据
        if (maskData != null)
            maskDataList.Add(maskData);
        else if (maskDatas != null)
            maskDataList = maskDatas.maskDataList;

        //加载完毕开启创造
        if (maskDataList.Count > 0)
        {
            StartCoroutine(RealCreateMask());
        }
    }
    private IEnumerator RealCreateMask()
    {
        //一个数据生成一个
        foreach (var maskData in maskDataList)
        {
            Debug.Log("开始创造");
            //创建失色层
            GameObject maskObj = GameObject.Instantiate(maskData.maskPrefab);
            //设置数据
            maskObj.GetComponent<Mask>().SetMaskData(maskData);
            //设置位置
            Transform showPos = showPosTransList[maskData.posId-1];
        
            maskObj.transform.position = new Vector3(showPos.position.x, showPos.position.y, 0);
            yield return null; //等待一帧
        }
    }
    
}
