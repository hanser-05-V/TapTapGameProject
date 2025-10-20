using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PopupBugGame : MonoBehaviour
{   
    
    [FoldoutGroup("弹窗预设")]
    [LabelText("小弹窗预设"),BoxGroup("弹窗预设/弹窗预设体相关",false ), SerializeField] //TODO:优化 后续通过缓存池加载
    private GameObject smallPopupPrefab;

    [LabelText("小弹窗预设"),BoxGroup("弹窗预设/弹窗预设体相关",false ),SerializeField] //TODO:优化 后续通过缓存池加载
    private GameObject bigPopupPrefab;

    [LabelText("弹窗生成范围"), BoxGroup("弹窗预设/弹窗预设体相关",false ),SerializeField] //TODO:加载 在场景中动态加载  这里先直接赋值
    private RectTransform imageRange;
    [Space(5)]
    [FoldoutGroup("弹窗数量限制相关")]
    [LabelText("弹窗数量限制"), BoxGroup("弹窗数量限制相关/弹窗数量",false),SerializeField]
    private int maxPopupCount = 5; // 指定最大生成弹窗的数量
    private int currentPopupCount = 0; // 当前生成的弹窗数量

    [LabelText("生长弹窗时间"), BoxGroup("弹窗数量限制相关/弹出时间", false)]
    public float growPopInterval = 0.5f; // 多种类型弹窗之间的间隔 时间 
    [LabelText("弹窗和弹窗之间间隔时间"), BoxGroup("弹窗数量限制相关/弹窗时间", false)]
    public float popInterval = 0.5f; // 多种类型弹窗之间的间隔 时间
    
    [LabelText("弹窗之间的控制间距"),BoxGroup("弹窗数量限制相关/弹窗间隔",false)]
    public Vector2 popupSpacing;  // 控制水平方向和垂直方向的间距

    [Space(5)]
    [FoldoutGroup("弹窗位置相关")]
    [LabelText("Romdom路线"), BoxGroup("弹窗位置相关/Romdom路线", false), SerializeField]
    private List<RectTransform> RandomPosList = new List<RectTransform>();
    [LabelText("路线一"), BoxGroup("弹窗位置相关/路线一", false), SerializeField]
    private List<RectTransform> RouteOne = new List<RectTransform>();
    [LabelText("路线二"), BoxGroup("弹窗位置相关/路线二", false), SerializeField]
    private List<RectTransform> RouteTwo = new List<RectTransform>();
    [LabelText("路线二"), BoxGroup("弹窗位置相关/路线二", false), SerializeField]
    private List<RectTransform> RouteThree = new List<RectTransform>();

    private GameObject mainPupObj; // 主弹窗对象

    [LabelText("小 游戏 数据 ")]
    public PopupData popData; //测试公开

    [LabelText("当前位置列表")] //测试公开
    public List<RectTransform> currentPosList = new List<RectTransform>();

    private int positionIndex = 0; // 位置索引

    private void Awake()
    {
        //获取Canvas坐标  //TODO:寻找优化
        imageRange = GameObject.FindWithTag("Canvas").GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener<BugData>(E_EventType.E_PopupBugGame, StartGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener<BugData>(E_EventType.E_PopupBugGame, StartGame);
    }
    public void StartGame(BugData bugData)
    {

        popData = bugData as PopupData;
        // currentPosList = popInfo.popRectTeansList; 
        //解数据
        InitPupGame(popData);

        // 开始生成弹窗
        StartCoroutine(CreatePopup());
    }

    private void InitPupGame(PopupData popData)
    {
        //确定生成路线
        switch (popData.popTransType)
        {
            case E_PopTransType.Random:
                currentPosList = RandomPosList;
                break;
            case E_PopTransType.RouteOne:
                currentPosList = RouteOne;
                break;
            case E_PopTransType.RouteTwo:
                currentPosList = RouteTwo;
                break;
            case E_PopTransType.RouteThree:
                currentPosList = RouteThree;
                break;
        }

        //设置弹窗数量限制
        maxPopupCount = popData.popNum;
        //设置 弹窗生长时间
        growPopInterval = popData.growInterval;
        //设置 弹窗和弹窗生长间隔
        popInterval = popData.popInterval;
    }

    // //测试使用
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {

    //         StartCoroutine(CreatePopup());
    //     }
    // }

    //生成弹窗
    private IEnumerator CreatePopup()
    {
        //预留缓冲时间
        yield return new WaitForSeconds(0.5f);
        currentPopupCount = 0;
        positionIndex = 0;
        while (currentPopupCount < maxPopupCount)
        {
            //判断弹窗的大小 //TODO: 每次生成的弹窗大小不一样 （不是每次触发bug 都是一样的）
            if (popData.popSize == E_PopSize.Small)
            {
                mainPupObj = Instantiate(smallPopupPrefab, imageRange.transform);
                mainPupObj.transform.SetParent(imageRange.transform, false);

                //设置弹窗数据
                mainPupObj.GetComponent<Pup>().InitPop(popData);
            }
            else if (popData.popSize == E_PopSize.Big)
            {
                mainPupObj = Instantiate(bigPopupPrefab, imageRange.transform);
                mainPupObj.transform.SetParent(imageRange.transform, false);
                //设置弹窗数据
                mainPupObj.GetComponent<Pup>().InitPop(popData);
            }


            // 获取当前弹窗的位置，循环使用位置列表 
            Vector2 position = currentPosList[positionIndex].anchoredPosition;

            // 限制位置不超过边界
            position = ClampPositionToRange(position, mainPupObj.GetComponent<RectTransform>().sizeDelta);

            // 设置弹窗位置
            mainPupObj.GetComponent<RectTransform>().anchoredPosition = position;
            // 更新位置索引
            positionIndex = (positionIndex + 1) % currentPosList.Count;
            // 增加弹窗数量
            currentPopupCount++;
            yield return new WaitForSeconds(popInterval);
            //如果的多个弹窗类型 则进行弹窗生长
            switch (popData.popCount)
            {
                case E_PopCount.One: //单个不做处理
                    break;
                case E_PopCount.Two:
                    yield return new WaitForSeconds(0.1f);
                    yield return GrowPop(popData.growDir, 1, mainPupObj.GetComponent<RectTransform>());

                    break;
                case E_PopCount.Three:
                    yield return new WaitForSeconds(0.1f);
                    yield return GrowPop(popData.growDir, 2, mainPupObj.GetComponent<RectTransform>());

                    break;
                case E_PopCount.Four:
                    yield return new WaitForSeconds(0.1f);
                    yield return GrowPop(popData.growDir, 3, mainPupObj.GetComponent<RectTransform>());

                    break;
            }
        }

    }
  
    #region 弹窗生长
    private IEnumerator GrowPop(E_PopGrowDir growDir, int count,RectTransform mainRect)
    {
           // 确保 mainPupObj 未被销毁
        if (mainPupObj == null)
        {
            yield break; // 如果销毁了，退出协程
        }
        
        // 计算弹窗的大小
        Vector2 sizeDelta = mainRect.sizeDelta;

        // 根据方向生成弹窗
        for (int i = 0; i < count; i++)
        {
            GameObject newPopup = Instantiate(popData.popSize == E_PopSize.Small ? smallPopupPrefab : bigPopupPrefab, imageRange.transform);
            newPopup.GetComponent<Pup>().InitPop(popData);
            RectTransform newRect = newPopup.GetComponent<RectTransform>();

            // 计算新的弹窗位置
            Vector2 offset = GetOffset(growDir, sizeDelta, i);

           // 控制弹窗之间的间距
            Vector2 newPosition = mainRect.anchoredPosition + offset + new Vector2(i * popupSpacing.x, i * popupSpacing.y);

            // 限制弹窗位置在imageRange的范围内
            newPosition = ClampPositionToRange(newPosition, newRect.sizeDelta);

            // 设置弹窗最终位置
            newRect.anchoredPosition = newPosition;
            yield return new WaitForSeconds(growPopInterval);
        }
    }

    // 根据方向计算偏移
    private Vector2 GetOffset(E_PopGrowDir growDir, Vector2 sizeDelta, int index)
    {
        float offsetX = 0, offsetY = 0;

        switch (growDir)
        {
            case E_PopGrowDir.Left:
                offsetX = -sizeDelta.x / 5 * (index + 1); // 左侧偏移
                break;
            case E_PopGrowDir.Right:
                offsetX = sizeDelta.x / 5 * (index + 1); // 右侧偏移
                break;
            case E_PopGrowDir.Top:
                offsetY = sizeDelta.y / 5 * (index + 1); // 上侧偏移
                break;
            case E_PopGrowDir.Down:
                offsetY = -sizeDelta.y / 5 * (index + 1); // 下侧偏移
                break;
            case E_PopGrowDir.LeftTop:
                offsetX = -sizeDelta.x / 5 * (index + 1); // 左上侧偏移
                offsetY = sizeDelta.y / 5 * (index + 1);
                break;
            case E_PopGrowDir.LeftDown:
                offsetX = -sizeDelta.x / 5 * (index + 1); // 左下侧偏移
                offsetY = -sizeDelta.y / 5 * (index + 1);
                break;
            case E_PopGrowDir.RightTop:
                offsetX = sizeDelta.x / 5 * (index + 1); // 右上侧偏移
                offsetY = sizeDelta.y / 5 * (index + 1);
                break;
            case E_PopGrowDir.RightDown:
                offsetX = sizeDelta.x / 5 * (index + 1); // 右下侧偏移
                offsetY = -sizeDelta.y / 5 * (index + 1);
                break;
        }

        return new Vector2(offsetX, offsetY);
    }

    // 限制弹窗位置在规定的范围内
    private Vector2 ClampPositionToRange(Vector2 position, Vector2 popupSize)
    {
        // 获取imageRange的边界
        Rect rangeRect = imageRange.rect;

        // 计算弹窗在imageRange中的位置限制
        float minX = rangeRect.xMin + popupSize.x / 2;
        float maxX = rangeRect.xMax - popupSize.x / 2;
        float minY = rangeRect.yMin + popupSize.y / 2;
        float maxY = rangeRect.yMax - popupSize.y / 2;

        // 限制X和Y位置
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    #endregion



}