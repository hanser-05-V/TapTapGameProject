using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BasePanle : MonoBehaviour
{
    //Canvas组件 用于淡入淡出
    private CanvasGroup canvasGroup;
    //淡入时间
    public float showTime = 1f;
    //淡出时间
    public float hideTime = 1f;

    protected virtual void Awake()
    {
        //确保组件存在
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    /// <summary> 显示面板
    /// 
    /// </summary>
    /// <param name="isFade"></param>
    public virtual void Showme(bool isFade = true)
    {
        canvasGroup.DOFade(1, showTime);
    }
    /// <summary> 隐藏面板
    /// 
    /// </summary>
    /// <param name="isFade"> 是否为 淡出 默认为是 </param>
    /// <param name="callBack"> 隐藏后 处理回调 </param>
    public virtual void Hideme(bool isFade = true,DG.Tweening.TweenCallback callBack = null) 
    {
        canvasGroup.DOFade(0, hideTime).OnComplete(callBack);
    }
    
}
