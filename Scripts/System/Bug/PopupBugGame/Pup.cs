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
    [LabelText("移动速度"), SerializeField]
    private float moveSpeed = 10f;
    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);


        StartCoroutine(MoveCloseButton());
    }

    // 关闭按钮点击事件
    private void OnCloseButtonClick()
    {
        //减少怒气值 
        AngerMeterSystem.Instance.ReduceAngerValue(10f);
        Debug.Log("当前怒气值：" + AngerMeterSystem.Instance.CurrentAngerValue);
        //TODO：优化 缓存池
        Destroy(this.gameObject);
    }

    //移动关闭按钮
    // private IEnumerator MoveCloseButton()
    // {
    //      // 获取关闭按钮的 RectTransform
    //     RectTransform buttonRect = closeButton.GetComponent<RectTransform>();
    //     // 获取背景图的 RectTransform
    //     RectTransform bkRect = bkImage.GetComponent<RectTransform>();

    //     // 获取背景图的大小和位置
    //     Vector2 bkSize = bkRect.sizeDelta;
    //     Vector2 bkPosition = bkRect.anchoredPosition;

    //     // 获取按钮的尺寸
    //     Vector2 buttonSize = buttonRect.sizeDelta;

    //     // 计算按钮可以在背景图内移动的范围
    //     float minX = bkPosition.x - bkSize.x / 2 + buttonSize.x / 2;  // 左边界
    //     float maxX = bkPosition.x + bkSize.x / 2 - buttonSize.x / 2;  // 右边界
    //     float minY = bkPosition.y - bkSize.y / 2 + buttonSize.y / 2;  // 下边界
    //     float maxY = bkPosition.y + bkSize.y / 2 - buttonSize.y / 2;  // 上边界

    //     while (true)
    //     {
    //         float duration = 0.5f;
    //         float elapsedTime = 0f;
    //         Vector2 startPosition = buttonRect.anchoredPosition;
    //         Vector2 targetPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    //         while (elapsedTime < duration)
    //         {
    //             buttonRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
    //             elapsedTime += Time.deltaTime;
    //             yield return null;
    //         }

    //         // 确保最终位置准确
    //         buttonRect.anchoredPosition = targetPosition;

    //         // 等待一段时间再移动到下一个位置
    //         yield return new WaitForSeconds(0.5f);
    //     }
    // }

    private IEnumerator MoveCloseButton()
    {
        RectTransform buttonRect = closeButton.GetComponent<RectTransform>();
        RectTransform bkRect = bkImage.GetComponent<RectTransform>();

        // 使用更可靠的方法获取边界
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
            float duration = 0.5f;
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
                buttonRect.position = Vector2.Lerp(currentWorldPos, targetWorldPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 确保最终位置准确
            buttonRect.position = targetWorldPos;

            // 等待一段时间再移动到下一个位置
            yield return new WaitForSeconds(0.5f);
        }
    }

}
