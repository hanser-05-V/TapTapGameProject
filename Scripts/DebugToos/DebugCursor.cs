using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//Debug工具管理者
public class DebugCursorController : MonoBehaviour
{
    public PopCursor popCursor;
    public CrawCursor crawCursor;
    public SelectCursor selectCursor;
    public PickColorCursor pickColorObj;

    public void SetActiveCursor(E_DebugType debugType)
    {
        //先集体失活
        HideAllCursor();
        //再根据类型激活
        switch (debugType)
        {
            case E_DebugType.PopUpGame:
                popCursor.gameObject.SetActive(true);
                break;
            case E_DebugType.CrawBugGame:
                crawCursor.gameObject.SetActive(true);
                break;
            case E_DebugType.SeleBugGame:
                selectCursor.gameObject.SetActive(true);
                break;
            case E_DebugType.PickColorGame:
                pickColorObj.gameObject.SetActive(true);
                break;
        }
    }
    //隐藏所有光标物体
    public void HideAllCursor()
    {
        //全部失活
        popCursor.gameObject.SetActive(false);
        crawCursor.gameObject.SetActive(false);
        selectCursor.gameObject.SetActive(false);
        pickColorObj.gameObject.SetActive(false);
    }
}
