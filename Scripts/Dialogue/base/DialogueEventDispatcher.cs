using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEventDispatcher
{
    private static Dictionary<string, Action> eventMap = new Dictionary<string, Action>();

    public static void Register(string key, Action action)
    {
        if (!eventMap.ContainsKey(key))
            eventMap[key] = action;
        else
            eventMap[key] += action;
    }

    public static void Unregister(string key, Action action)
    {
        if (eventMap.ContainsKey(key))
            eventMap[key] -= action;
    }

    public static void Invoke(string key)
    {
        if (eventMap.TryGetValue(key, out var action))
        {
            action?.Invoke();
        }
        else
        {
            Debug.LogWarning($"未找到委托事件：{key}");
        }
    }
}