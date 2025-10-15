using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//提供给 未继承Mono 的脚本依旧能够使用生命周期函数
public class MonoMgr : SingletonAutoMono<MonoMgr>
{
    //start事件
    private event UnityAction startEvent;
    //fixedUpdate事件
    private event UnityAction fixedEvent;
    //帧更新事件
    private event UnityAction updateEvent;
    //LateUpdate事件
    private event UnityAction lateEvent;

    //Start
    public void AddStartListener(UnityAction action)
    {
        startEvent += action;
    }
    public void RemoveStartListener(UnityAction action)
    {
        startEvent -= action;
    }

    private void Start()
    {
        startEvent?.Invoke();
    }
   
   
    //物理帧更新
    public void AddFixedUpdatListener(UnityAction action)
    {
        fixedEvent += action;
    }
    public void RemoveFixedUpdatListener(UnityAction action)
    {
        fixedEvent -= action;
    }

    private void FixedUpdate()
    {
        fixedEvent?.Invoke();
    }
  
    
    //帧更新
    public void AddUpdatListener(UnityAction action)
    {
        updateEvent += action;
    }
    public void RemoveUpdatListener(UnityAction action)
    {
        updateEvent -= action;
    }
    private void Update()
    {
        updateEvent?.Invoke();
    }
  
    
    //LateUpdate
    public void AddLateUpdateListener(UnityAction action)
    {
        lateEvent += action;
    }
    public void RemoveLateUpdateListener(UnityAction action)
    {
        lateEvent -= action;
    }
    private void LateUpdate()
    {
        lateEvent?.Invoke();
    }

}
