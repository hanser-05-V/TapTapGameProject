using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

//弹窗
public class Pup : MonoBehaviour
{
    [LabelText("背景"), SerializeField]
    private Image bkImage;
    [LabelText("关闭按钮"), SerializeField]
    private Button closeButton;
    [LabelText("屏幕随机移动速度"),SerializeField]
    private float randomMoveSpeed = 1f;
    [LabelText("横向移动速度"), SerializeField]
    private float moveSpeed = 10f;
    
    //测试序列化板块
    [LabelText("弹窗数据"), SerializeField]
    private PopupData popupData; // 弹窗数据 (包含的对 )

    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);

        // StartCoroutine(MoveHorizontalCloseButton());
        // StartCoroutine(RandomMoveCloseButton());
    }
    // 初始化弹窗数据
    public void InitPop(PopupData data)
    {
        popupData = data;
        if (popupData == null)
        {
            Debug.LogError("弹窗数据为空！");
            return;
        }

        //根据配置的移动方式开启移动协程
        if (data.fockType == E_FockType.Normal)
            return;
        else if (data.fockType == E_FockType.Random)
        {
            StartCoroutine(RandomMoveCloseButton());
        }
        else if (data.fockType == E_FockType.Move)
        {
            //让叉叉水平移动 （到底反向 速度可控）
            StartCoroutine(MoveHorizontalCloseButton());
        }
    }

    // 关闭按钮点击事件
    private void OnCloseButtonClick()
    {
        //如果当前Debug类型不是点击类 则直接返回
        if(DebugMgr.Instance.currentDebugType != E_DebugType.PopUpGame)
        {
            Debug.Log("当前Debug类型不是点击类 无法关闭弹窗");
            return;
        }
        //TODO:BugType
        if(popupData.bugType == E_BugType.InputBugGame)
            AngerMeterSystem.Instance.ReduceAngerValue(popupData.reduceValue);
        //TODO：优化 缓存池
        Destroy(this.gameObject);
        //减少怒气值 
    }
    // 随机移动关闭按钮
    private IEnumerator RandomMoveCloseButton()
    {

        RectTransform buttonRect = closeButton.GetComponent<RectTransform>();
        RectTransform bkRect = bkImage.GetComponent<RectTransform>();

        //获取边界
        Vector3[] bkCorners = new Vector3[4];
        bkRect.GetWorldCorners(bkCorners);

        Vector3[] buttonCorners = new Vector3[4];
        buttonRect.GetWorldCorners(buttonCorners);

        // 计算按钮的实际尺寸（世界坐标）
        float buttonWidth = buttonCorners[2].x - buttonCorners[0].x;
        float buttonHeight = buttonCorners[2].y - buttonCorners[0].y;

        // 计算背景图的实际边界（世界坐标）
        float minX = bkCorners[0].x + buttonWidth / 2;
        float maxX = bkCorners[2].x - buttonWidth / 2;
        float minY = bkCorners[0].y + buttonHeight / 2;
        float maxY = bkCorners[2].y - buttonHeight / 2;

        while (true)
        {
            
            float duration = 0.25f;
            float elapsedTime = 0f;

            // 获取按钮当前的世界坐标位置
            Vector2 currentWorldPos = buttonRect.position;

            // 生成目标位置（世界坐标）
            Vector2 targetWorldPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            while (elapsedTime < duration)
            {
                // 在世界坐标空间中进行插值
                buttonRect.position = Vector2.Lerp(currentWorldPos, targetWorldPos, elapsedTime / randomMoveSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
    
            // 确保最终位置准确
            buttonRect.position = targetWorldPos;

            // 等待一段时间再移动到下一个位置
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator MoveHorizontalCloseButton()
    {
         // 获取叉叉按钮和背景的 RectTransform
        RectTransform buttonRect = closeButton.GetComponent<RectTransform>();
        RectTransform bkRect = bkImage.GetComponent<RectTransform>();

        // 获取背景的实际边界（世界坐标）
        Vector3[] bkCorners = new Vector3[4];
        bkRect.GetWorldCorners(bkCorners);

        // 计算按钮的宽度（用于边界检测时考虑按钮自身大小）
        float buttonWidth = buttonRect.rect.width * buttonRect.lossyScale.x;

        // 计算背景的左右边界（考虑按钮宽度，确保按钮不会超出背景）
        float minX = bkCorners[0].x + buttonWidth / 2;
        float maxX = bkCorners[2].x - buttonWidth / 2;

        // 初始方向，默认是向左
        float moveDirection = -1f;

        // 移动速度
        float moveSpeed = this.moveSpeed;

        while (true)
        {
            // 获取当前按钮的世界坐标
            Vector3 currentPos = buttonRect.position;

            // 计算下一帧的目标位置
            float newPosX = currentPos.x + moveDirection * moveSpeed * Time.deltaTime;

            // 检查是否超出边界，如果超出则改变方向
            if (newPosX <= minX)
            {
                newPosX = minX; // 确保不超出左边界
                moveDirection = 1f; // 向右移动
            }
            else if (newPosX >= maxX)
            {
                newPosX = maxX; // 确保不超出右边界
                moveDirection = -1f; // 向左移动
            }

            // 更新按钮的世界坐标
            buttonRect.position = new Vector2(newPosX, currentPos.y);

            // 等待下一帧
            yield return null;
        }
    }
}
