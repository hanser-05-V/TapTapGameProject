using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenterTest : MonoBehaviour
{

    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener(E_EventType.OnLeftButtonClick, OnLeftMouseDown);

        EventCenter.Instance.RegisterEventListener<Main>(E_EventType.OnTestFunc, FireFunc);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener(E_EventType.OnLeftButtonClick, OnLeftMouseDown);

        EventCenter.Instance.UnRegisterEventListener<Main>(E_EventType.OnTestFunc, FireFunc);
    }
    private void OnLeftMouseDown()
    {
        Debug.Log("开火");
    }
    private void FireFunc(Main  info)
    {
        Debug.Log("带参数的开火");
    }
}
