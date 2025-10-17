using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanle : BasePanle
{
    [LabelText("开始游戏按钮"), SerializeField]
    private Button startButton;
    [LabelText("设置按钮"), SerializeField]
    private Button settingButton;
    [LabelText("退出按钮"), SerializeField]
    private Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);

      
    }

    private void OnStartButtonClick()
    {
        UIMgr.Instance.Show<ChoosePanle>();

        UIMgr.Instance.Hide<BeginPanle>();
    }
    private void OnSettingButtonClick()
    {
        UIMgr.Instance.Show<SettingPanle>();

    }
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
