using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
//用于UI动画表现的工具
public class UITool : MonoBehaviour
{
    #region 强制重置布局
    public static void RefreshAllLayouts(Transform t)
    {
        while (t != null)
        {
            var layoutGroup = t.GetComponent<LayoutGroup>();
            var fitter = t.GetComponent<ContentSizeFitter>();
            var rect = t.GetComponent<RectTransform>();

            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
                layoutGroup.enabled = true;
            }

            if (fitter != null)
            {
                fitter.enabled = false;
                fitter.enabled = true;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            t = t.parent;
        }
    }
    #endregion

    #region 动画展示区域
    //动态平滑移动到某个位置
    /// <summary>
    /// 平滑移动物体到目标位置，完成后执行委托
    /// </summary>
    /// <param name="_obj">要移动的物体</param>
    /// <param name="_TargetPos">目标位置的 Transform</param>
    /// <param name="_time">移动耗时</param>
    /// <param name="_act">完成后的回调委托，可为空</param>
    public static void Do_MoveToTargetPos(GameObject _obj, Transform _TargetPos, float _time, Action _act = null)
    {
        if (_obj == null || _TargetPos == null)
        {
            Debug.LogError("Do_MoveToTargetPos: _obj 或 _TargetPos 为空！");
            return;
        }

        RectTransform rtObj = _obj.GetComponent<RectTransform>();
        RectTransform rtTarget = _TargetPos.GetComponent<RectTransform>();

        if (rtObj != null && rtTarget != null)
        {
            // UI 元素，使用锚点坐标
            rtObj.DOAnchorPos(rtTarget.anchoredPosition, _time)
                .SetEase(Ease.Linear)
                .OnComplete(() => _act?.Invoke());
        }
        else
        {
            // 3D 物体，使用世界坐标
            _obj.transform.DOMove(_TargetPos.position, _time)
                .SetEase(Ease.Linear)
                .OnComplete(() => _act?.Invoke());
        }
    }

    //物体缩放
    /// <summary>
    /// 平滑缩放物体到指定大小
    /// </summary>
    /// <param name="_obj">要缩放的物体</param>
    /// <param name="_time">缩放耗时</param>
    /// <param name="_size">目标缩放值（Uniform 缩放）</param>
    /// <param name="_act">完成后的回调委托，可为空</param>
    public static void Do_ScaleToTarget(GameObject _obj, float _time, float _size, Action _act = null)
    {
        if (_obj == null)
        {
            Debug.LogError("Do_ScaleToTarget: _obj 为空！");
            return;
        }

        RectTransform rt = _obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            // UI 元素，缩放局部缩放
            rt.DOScale(Vector3.one * _size, _time)
              .SetEase(Ease.Linear)
              .OnComplete(() => _act?.Invoke());
        }
        else
        {
            // 3D 物体
            _obj.transform.DOScale(Vector3.one * _size, _time)
                .SetEase(Ease.Linear)
                .OnComplete(() => _act?.Invoke());
        }
    }


    //DOTween动画队列
    // 队列存储每个动作的委托
    private static Queue<Action<Action>> actionQueue = new Queue<Action<Action>>();
    private static bool isPlaying = false;

    /// <summary>
    /// 添加动画到队列
    /// </summary>
    /// <param name="tweenAction">动画委托，参数是完成回调 done()</param>
    public static void Enqueue(Action<Action> tweenAction)
    {
        actionQueue.Enqueue(tweenAction);
        if (!isPlaying)
        {
            PlayNext();
        }
    }

    /// <summary>
    /// 播放队列中的下一个动画
    /// </summary>
    private static void PlayNext()
    {
        if (actionQueue.Count == 0)
        {
            isPlaying = false;
            return;
        }

        isPlaying = true;
        Action<Action> currentAction = actionQueue.Dequeue();
        currentAction.Invoke(() =>
        {
            PlayNext();
        });
    }

    /// <summary>
    /// 清空队列
    /// </summary>
    public static void ClearQueue()
    {
        actionQueue.Clear();
        isPlaying = false;
    }
    #endregion

    #region UI对象池
    //创建对象池
    private static Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// 获取对象池中的对象，或实例化新对象
    /// </summary>
    /// <param name="prefab">要生成的预制体</param>
    /// <param name="parent">父物体</param>
    /// <param name="orderInLayer">SortingOrder 值</param>
    /// <returns>生成或获取的 GameObject</returns>
    public static GameObject GetObject(GameObject prefab, Transform parent = null, int orderInLayer = 0)
    {
        string key = prefab.name;
        GameObject obj;

        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            obj = UnityEngine.Object.Instantiate(prefab);
            obj.name = key;
        }

        obj.SetActive(true);
        obj.transform.SetParent(parent, false); // 保持 UI 层级关系


        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        RectTransform objRect = obj.GetComponent<RectTransform>();
        if (prefabRect != null && objRect != null)
        {
            objRect.anchorMin = prefabRect.anchorMin;
            objRect.anchorMax = prefabRect.anchorMax;
            objRect.pivot = prefabRect.pivot;
            objRect.sizeDelta = prefabRect.sizeDelta;
            objRect.localScale = Vector3.one;
            objRect.anchoredPosition = prefabRect.anchoredPosition;
        }

        SetOrderInLayer(obj, orderInLayer);
        return obj;
    }

    /// <summary>
    /// 将对象归还到对象池
    /// </summary>
    /// <param name="obj">需要回收的对象</param>
    public static void ReturnObject(GameObject obj, bool forceDestroy = false)
    {
        string key = obj.name;

        if (forceDestroy)
        {
            // 强制销毁对象并卸载资源
            GameObject.Destroy(obj);
            Resources.UnloadUnusedAssets();
            return;
        }

        // 正常回收到对象池
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }
        obj.SetActive(false);
        obj.transform.SetParent(null); // 设置父物体为 null 或对象池管理器
        poolDictionary[key].Enqueue(obj);
    }
    //UI对象池
    public static void CreateItemPrefabs<T, TComponent>(
    string prefabName,
    Transform parent,
    List<T> itemList,
    Action<TComponent, T> onInit)
    where TComponent : Component
    {
        if (string.IsNullOrEmpty(prefabName) || parent == null || itemList == null)
        {
            Debug.LogWarning("创建预制体参数不合法");
            return;
        }

        GameObject prefab = Tool.LoadPrefab(prefabName);
        if (prefab == null)
        {
            Debug.LogError($"未能加载预制体：{prefabName}");
            return;
        }

        // 回收旧的子物体
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        foreach (Transform child in children)
        {
            ReturnObject(child.gameObject);
        }

        // 实例化并初始化
        foreach (var item in itemList)
        {
            GameObject obj = GetObject(prefab, parent);
            TComponent comp = obj.GetComponent<TComponent>();
            if (comp != null)
            {
                onInit?.Invoke(comp, item);
            }
        }
    }
    /// <summary>
    /// 设置 UI 元素的 Order in Layer
    /// </summary>
    /// <param name="obj">需要设置的对象</param>
    /// <param name="orderInLayer">SortingOrder 值</param>
    private static void SetOrderInLayer(GameObject obj, int orderInLayer)
    {
        // 获取 Canvas 组件
        Canvas canvas = obj.GetComponent<Canvas>();
        if (canvas != null)
        {
            // 获取 CanvasRenderer 组件并设置 SortingOrder
            canvas.sortingOrder = orderInLayer; // 设置 Canvas 的 Sorting Order

        }

    }
    #endregion
}
