using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//挂载在 Scrow view上
public class BubbleSlider : MonoBehaviour
{
    [LabelText("消息列表"), SerializeField]
    private List<GameObject> messagrList = new List<GameObject>();
    [LabelText("消息之间的间隔距离"), SerializeField]
    private float spacing = 30f;

    [LabelText("左右消息距离 倍数")]
    public float index;

    public RectTransform context; // content容器改为RectTransform
    public RectTransform defultTransform; // 默认位置改为RectTransform

    private RectTransform lastRectTransform; // 最后一条消息的RectTransform

    public void AddMessage(GameObject message, bool isRight = false)
    {
        // 设置父对象
        message.transform.SetParent(context, false);

        // 获取RectTransform组件
        RectTransform messageRect = message.GetComponent<RectTransform>();

        // 设置锚点，根据左右消息调整
        if (isRight)
        {
            // 右侧消息：锚点在右上角
            messageRect.anchorMin = new Vector2(1, 1);
            messageRect.anchorMax = new Vector2(1, 1);
            messageRect.pivot = new Vector2(1, 1); // 轴心点在右上角
        }
        else
        {
            // 左侧消息：锚点在左上角
            messageRect.anchorMin = new Vector2(0, 1);
            messageRect.anchorMax = new Vector2(0, 1);
            messageRect.pivot = new Vector2(0, 1); // 轴心点在左上角
        }

        Vector2 anchoredPos = Vector2.zero;

        // 计算新消息位置
        if (messagrList.Count == 0)
        {
            // 第一条消息，使用默认位置
            anchoredPos = defultTransform.anchoredPosition;
            // X位置根据左右对齐
            anchoredPos.x = isRight ? index * defultTransform.anchoredPosition.x : defultTransform.anchoredPosition.x;

            lastRectTransform = defultTransform;
        }
        else
        {
            // 获取上一条消息的RectTransform
            lastRectTransform = messagrList[messagrList.Count - 1].GetComponent<RectTransform>();

            // 计算新消息的Y位置：上条消息的Y - 上条消息高度 - 间距
            float lastMessageBottom = lastRectTransform.anchoredPosition.y - lastRectTransform.rect.height;
            anchoredPos.y = lastMessageBottom - spacing;

            // X位置根据左右对齐
            anchoredPos.x = isRight ? index * defultTransform.anchoredPosition.x : defultTransform.anchoredPosition.x;
        }

        // 设置位置
        messageRect.anchoredPosition = anchoredPos;

        // 添加到列表
        messagrList.Add(message);

        // 调整Content高度以适应所有消息
        AdjustContentHeight();

        // 自动滚动到底部
        ScrollToBottom();
        
    }
    // 滚动视图自动滚动到底部
    private void ScrollToBottom()
    {
        //TODO:滚动视图更新
        //  // 获取滚动视图的总高度
        // float contentHeight = context.rect.height;

        // // 获取Content的当前高度
        // float totalHeight = 0f;
        // foreach (GameObject msg in messagrList)
        // {
        //     RectTransform rect = msg.GetComponent<RectTransform>();
        //     totalHeight += rect.rect.height + spacing;
        // }

        // // 计算要滚动到的位置
        // Vector2 newAnchoredPosition = new Vector2(0, totalHeight - contentHeight);

        // // 设置新的滚动位置，确保滚动到底部
        // context.anchoredPosition = newAnchoredPosition;
    }
    // 调整Content高度以包含所有消息
    private void AdjustContentHeight()
    {
        if (messagrList.Count == 0) return;

        float totalHeight = 0f;
        foreach (GameObject msg in messagrList)
        {
            RectTransform rect = msg.GetComponent<RectTransform>();
            totalHeight += rect.rect.height + spacing;
        }

        // 加上第一条消息的初始偏移
        totalHeight += Mathf.Abs(defultTransform.anchoredPosition.y);

        // 设置Content高度
        context.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    // 清空消息列表
    public void ClearMessages()
    {
        foreach (GameObject msg in messagrList)
        {
            if (msg != null)
                Destroy(msg);
        }
        messagrList.Clear();
        lastRectTransform = defultTransform;
    }
}