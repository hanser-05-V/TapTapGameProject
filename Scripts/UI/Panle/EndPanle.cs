using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndPanle : BasePanle
{
    [LabelText("关卡失败提示文字"), SerializeField]
    private TextMeshProUGUI tipText;
    [LabelText("返回按钮"), SerializeField]
    private Button butReturn;
    [LabelText("再来一次按钮"), SerializeField]
    private Button butAgain;

    override protected void Awake()
    {
        base.Awake();

    }

    public void Start()
    {
        butAgain.onClick.AddListener(OnAgainClick);
        butReturn.onClick.AddListener(OnReturnClick);
    }

    public void OnReturnClick()
    {
        Debug.Log("返回按钮被点击");
        
        UIMgr.Instance.Hide<EndPanle>();

        // 切换场景开始游戏
        SceneMgr.Instance.LoadSceneAsyn("BeiganScene", () =>
        {
            //显示选择关卡界面
            UIMgr.Instance.Show<ChoosePanle>();
        });
    }
    public void OnAgainClick()
    {
        Debug.Log("再来一次按钮被点击");
        UIMgr.Instance.Hide<EndPanle>();

        //TODO：再次开始游戏
        UIMgr.Instance.Show<GamePanle>();
    }
    
    public void ChangeTipText(string text)
    {
        tipText.text = text;   
    }
}
