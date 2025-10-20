using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeveTwoTalkPanle : BasePanle
{
    [LabelText("对话框"), SerializeField]
    private Image imageTalkBubble;

    private TextMeshProUGUI talkContent;
    override protected void Awake()
    {
        base.Awake();
        //对话组件
        talkContent = this.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ChangeTalk(RectTransform _rectTrans,string _talkContent)
    {
        //设置聊天框位置
        imageTalkBubble.rectTransform.anchoredPosition = _rectTrans.anchoredPosition;
        //更改聊天文本
        talkContent.text = _talkContent;
    }
}
