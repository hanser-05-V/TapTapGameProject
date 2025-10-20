using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugTool : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [ShowInInspector]
    public E_DebugType DebugType;

    private float defultScale;
    private  void Awake()
    {
        defultScale = transform.localScale.x;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        DebugMgr.Instance.ChangDebugType(DebugType);

        //进入变大
        this.gameObject.transform.DOScale(0.75f, 0.05f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.DOScale(defultScale, 0.001f);
    }
}
