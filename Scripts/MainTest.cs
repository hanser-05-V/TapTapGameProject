using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour
{
    // //TODO:GamePanle 面板  现在测试直接进行引用
    // [SerializeField] private Camera uiCamera;
    [SerializeField] private LeveOneGamePanle gamePanle;
    // [SerializeField] private LayerMask whatIsBug;
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        //打开初始面板
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIMgr.Instance.Show<BeginPanle>();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UIMgr.Instance.Hide<BeginPanle>();
        }

        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     gamePanle.SendMassage(false);

        // }
        // if (Input.GetKeyDown(KeyCode.R))
        // {

        //     gamePanle.SendMassage(true);
        // }
        
        #region  以下是测试代码

        //鼠标射线检测
        if (Input.GetMouseButtonDown(1))
        {
            UIMgr.Instance.Show<LeveOneGamePanle>();
            // gamePanle.SendMassage();
        }
       


        #endregion
    }
}