using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//事件类型抽象父类
public abstract class EventInfoBase { }

//没有参数事件类型
public class EventInfo : EventInfoBase
{
    public UnityAction action;
    public EventInfo(UnityAction action)
    {
        this.action = action;
    }
}

//一个参数事件类型
public class EventInfo<T> : EventInfoBase
{
    public UnityAction<T> action;
    public EventInfo(UnityAction<T> action)
    {
        this.action = action;
    }
}

public class EventCenter : BaseManager<EventCenter>
{
    //防止外部进行实例化
    private EventCenter() { }
    //事件存储容器
    private Dictionary<E_EventType, EventInfoBase> eventDic = new Dictionary<E_EventType, EventInfoBase>();

    //触发事件 （没有参数）
    public void EventTrigger(E_EventType eventType)
    {
        if (eventDic.ContainsKey(eventType))
        {
            (eventDic[eventType] as EventInfo).action?.Invoke();
        }
    }

    //触发事件 （一个参数）
    public void EventTrigger<T>(E_EventType eventType,T info)
    {
        if (eventDic.ContainsKey(eventType))
        {
            //触发事件
            (eventDic[eventType] as EventInfo<T>).action?.Invoke(info);
        }
    }

    //注册事件监听 (没有参数)
    public void RegisterEventListener(E_EventType eventType, UnityAction action)
    {
        //有事件记录
        if (eventDic.ContainsKey(eventType))
        {
            //加入监听
            (eventDic[eventType] as EventInfo).action += action;
        }
        else
        {
            //没有就创建事件 添加

            eventDic.Add(eventType, new EventInfo(action));
        }

    }

    //注册事件监听 (一个参数)
    public void RegisterEventListener<T>(E_EventType eventType, UnityAction<T> action)
    {
        //有事件记录
        if (eventDic.ContainsKey(eventType))
        {
            //加入监听
            (eventDic[eventType] as EventInfo<T>).action += action;
        }
        else
        {
            //没有就创建事件 添加
            eventDic.Add(eventType, new EventInfo<T>(action));
        }
    }
    //卸载事件监听 (没有参数)
    public void UnRegisterEventListener(E_EventType eventType, UnityAction action)
    {
        //移除事件监听
        if (eventDic.ContainsKey(eventType))
        {
            (eventDic[eventType] as EventInfo).action -= action;
        }
        else
        {
            Debug.LogError("改类型事件监听不存在");
        }
    }

    //卸载事件监听 (一个参数)
    public void UnRegisterEventListener<T>(E_EventType eventType, UnityAction<T> action)
    {
        //移除事件监听
        if (eventDic.ContainsKey(eventType))
        {
            (eventDic[eventType] as EventInfo<T>).action -= action;
        }
        else
        {
            Debug.LogError("改类型事件监听不存在");
        }
    }

    //清空事件 过场景使用
    public void ClearEventCenter()
    {
        eventDic.Clear();
    }

}
