using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanle : BasePanle
{
    [LabelText("背景音乐滑动条"), SerializeField]
    private Slider bkMusicSlider;
    [LabelText("音效滑动条"), SerializeField]
    private Slider soundSlider;
    [LabelText("重置按钮"), SerializeField]
    private Button resetBtn;
    [LabelText("返回按钮"), SerializeField]
    private Button closeBtn;

    private void Start()
    {
        resetBtn.onClick.AddListener(OnResetButtonClick);
        closeBtn.onClick.AddListener(OnCloseButtonClick);
    
    }

    private void OnResetButtonClick()
    {
        
    }
    private void OnCloseButtonClick()
    {
        UIMgr.Instance.Hide<SettingPanle>();
    }
    
}
