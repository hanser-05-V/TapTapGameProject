using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PopupBugGame : MonoBehaviour
{
    [LabelText("弹窗预设"), SerializeField] //TODO:优化 后续通过缓存池加载
    private GameObject popupPrefab;

    [LabelText("弹窗生成范围"),SerializeField] //TODO:加载 在场景中动态加载  这里先直接赋值
    private RectTransform imageRange;
 
    [LabelText("弹窗数量限制")]
    [SerializeField]
    private int maxPopupCount = 5; // 指定最大生成弹窗的数量

    private int currentPopupCount = 0; // 当前生成的弹窗数量

    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener(E_EventType.E_PopupBugGame, StartGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener(E_EventType.E_PopupBugGame, StartGame);
    }
    public void StartGame()
    {
        StartCoroutine(CreatePopup());
    }

    private IEnumerator CreatePopup()
    {

        while (currentPopupCount < maxPopupCount)
        {
            // 实例化弹窗
            GameObject popup = Instantiate(popupPrefab, imageRange.transform);
            popup.transform.SetParent(imageRange.transform, false);

            // 设置随机位置（在Canvas范围内）
            RectTransform canvasRect = imageRange.GetComponent<RectTransform>();
            Vector2 randomPosition = new Vector2(
                Random.Range(0, canvasRect.rect.width),
                Random.Range(0, canvasRect.rect.height)
            );

            RectTransform popupRect = popup.GetComponent<RectTransform>();
            popupRect.anchoredPosition = randomPosition - new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2);

            currentPopupCount++;
            
            StartCoroutine(MovePopup(popupRect));

            // 随机等待时间（0.5-2秒）
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }

    }
     private IEnumerator MovePopup(RectTransform rectTransform)
    {
        // 设置轻微移动的范围
        float moveRange = 5f; // 最大移动范围
        float moveSpeed = 1f; // 移动速度

        while (true)
        {
            // 随机生成一个轻微的位移
            Vector2 randomOffset = new Vector2(
                Random.Range(-moveRange, moveRange),
                Random.Range(-moveRange, moveRange)
            );

            // 更新弹窗的位置，轻微移动
            rectTransform.anchoredPosition += randomOffset;

            // 确保弹窗在父对象的范围内
            Vector2 clampedPos = rectTransform.anchoredPosition;
            clampedPos.x = Mathf.Clamp(clampedPos.x, 0, imageRange.rect.width);
            clampedPos.y = Mathf.Clamp(clampedPos.y, 0, imageRange.rect.height);

            // 更新弹窗位置
            rectTransform.anchoredPosition = clampedPos;

            // 每隔一段时间轻微移动
            yield return new WaitForSeconds(moveSpeed);
        }
    }
}
