using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanle : BasePanle
{
    [LabelText("第一关"), SerializeField]
    private Button leve_1;
    [LabelText("第二关"), SerializeField]
    private Button leve_2;
    [LabelText("第三关"), SerializeField]
    private Button leve_3;
    [LabelText("第四关"), SerializeField]
    private Button leve_4;
    [LabelText("第五关"), SerializeField]
    private Button leve_5;
    private void Start()
    {
        leve_1.onClick.AddListener(OnClickLeve1);
        leve_2.onClick.AddListener(OnClickLeve2);
    }

    private void OnClickLeve1()
    {
        SceneMgr.Instance.LoadSceneAsyn("Level1", () =>
        {
            UIMgr.Instance.Show<LeveOneGamePanle>();
        });
        UIMgr.Instance.Hide<ChoosePanle>();
    }
    private void OnClickLeve2()
    {
         SceneMgr.Instance.LoadSceneAsyn("Leve2", () =>
        {
            UIMgr.Instance.Show<LeveTwoGamePanle>();
        });
        UIMgr.Instance.Hide<ChoosePanle>();
    }
}
