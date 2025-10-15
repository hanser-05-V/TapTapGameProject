using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanle : BasePanle
{
    [LabelText("第一关"), SerializeField]
    private Button leve_1;


    private void Start()
    {
        leve_1.onClick.AddListener(OnClickLeve1);
    }
    
    private void OnClickLeve1()
    {
        SceneMgr.Instance.LoadScene("Level1");
        UIMgr.Instance.Hide<ChoosePanle>();
    }
}
