using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class DebugMgr : SinggletonMono<DebugMgr>
{
    [LabelText("当前Debug类型"), SerializeField]
    public E_DebugType currentDebugType;

    [LabelText("鼠标长按 阈值"),SerializeField]
    private float longPressThreshold = 0.5f;
    private float pressTime = 0f;

    [LabelText("Debug组件面板"), SerializeField]
    private DebugPanle debugPanel;



    [LabelText("追随鼠标Debug控制器"), SerializeField]
    private DebugCursorController cursorController;

    private Vector2 mouseWorldPos;
    private Vector3 mouseScreenPos;

    override protected void Awake()
    {
        base.Awake();
        //TODO:Cursor更改
    }
    private void Start()
    {
        debugPanel.gameObject.SetActive(false);
        cursorController.HideAllCursor();
    }
    private void Update()
    {
        //鼠标右键长按 显示Debug面板
        if (Input.GetMouseButtonDown(1)) //右键按下
        {
            Debug.Log("右键按下");
            pressTime = Time.time;
            // 获取鼠标的屏幕坐标
            Vector2 screenPosition = Input.mousePosition;
            // 将鼠标屏幕坐标转换为世界坐标
            mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPosition);

            //不显示鼠标图片
            cursorController.HideAllCursor();
            
        }
        else if (Input.GetMouseButton(1)) //右键长按
        {
            //超过阈值
            if (Time.time - pressTime >= longPressThreshold)
            {
                Time.timeScale = 0.2f; //闪避时间  
                // 显示Debug面板
                debugPanel.gameObject.SetActive(true);
                debugPanel.transform.position = mouseWorldPos;

                pressTime = -1; //避免重复打开
            }
        }
        else if (Input.GetMouseButtonUp(1)) //右键松开
        {
            debugPanel.RecoverScale();
            debugPanel.gameObject.SetActive(false);
            pressTime = 0f;
            Time.timeScale = 1f; //恢复时间
        }

        //更新位置  
        mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f;
        cursorController.gameObject.transform.position = Camera.main.ScreenToWorldPoint(mouseScreenPos);    
    }


    //切换DebugType
    public void ChangDebugType(E_DebugType newDebugType)
    {
        Debug.Log("切换DebugType:" + newDebugType);
        currentDebugType = newDebugType;

        //切换鼠标工具
        cursorController.SetActiveCursor(newDebugType);
    }
   
}
