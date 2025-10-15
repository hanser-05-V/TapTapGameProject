using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Auto_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Tooltip("总节点")] public RectTransform trans_parent;

    void OnEnable()
    {
        //如果当前是自动播放的，就展示下面的
        if (DialogueManage._instance.isAutoPlaying)
        {
            trans_parent.GetChild(0).gameObject.SetActive(false);
            trans_parent.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            trans_parent.GetChild(0).gameObject.SetActive(true);
            trans_parent.GetChild(1).gameObject.SetActive(false);
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //如果当前是自动播放的，让第二个子节点的第二个显示
        if (DialogueManage._instance.isAutoPlaying)
        {
            trans_parent.GetChild(0).gameObject.SetActive(false);
            trans_parent.GetChild(1).gameObject.SetActive(true);

            trans_parent.GetChild(1).GetChild(0).gameObject.SetActive(false);
            trans_parent.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else
        {   
            trans_parent.GetChild(0).gameObject.SetActive(true);
            trans_parent.GetChild(1).gameObject.SetActive(false);

            trans_parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
            trans_parent.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        if (DialogueManage._instance.isAutoPlaying)
        {   
            trans_parent.GetChild(0).gameObject.SetActive(false);
            trans_parent.GetChild(1).gameObject.SetActive(true);
            trans_parent.GetChild(1).GetChild(0).gameObject.SetActive(true);
            trans_parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {   
            trans_parent.GetChild(0).gameObject.SetActive(true);
            trans_parent.GetChild(1).gameObject.SetActive(false);
            trans_parent.GetChild(0).GetChild(0).gameObject.SetActive(true);
            trans_parent.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }

    
}
