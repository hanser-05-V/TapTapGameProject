using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 统合 Editor加载和 AB包加载， 
public class ABResMgr : BaseManager<ABResMgr>
{
    private ABResMgr() { }
    /// <summary> true 表示 使用Editor 加载 (开发使用)   false  把对应资源打入对应AB包后 (发布使用)
    /// 
    /// </summary>
    private static bool isDebug = true; 

    /// <summary> 协程异步加载资源
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <param name="isAsync"></param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack, bool isAsync = false) where T : Object
    {
#if UNITY_EDITOR
        //仅在编辑器模式使用
        if (isDebug)
        {
            T res = EditorResMgr.Instance.LoadEditorRes<T>(abName + "/" + resName); //对应文件夹就是包名 / 内部成员就是 资源名
            //回调返回资源
            callBack?.Invoke(res as T);
        }
        else
        {
            //异步加载AB包
            ABMgr.Instance.LoadResAsync<T>(abName, resName, callBack, isAsync);
        }
#else
        //异步加载AB包
        ABMgr.Instance.LoadResAsync<T>(abName, resName, callBack, isAsync);
#endif
    }
}
