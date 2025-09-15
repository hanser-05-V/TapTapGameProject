using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//池子数据（抽屉对象）
public class PoolData
{
    private Stack<GameObject> poolObjs = new Stack<GameObject>();
    private GameObject poolObjRoot; //柜子父节点
   
    public float Count => poolObjs.Count;
    public PoolData(GameObject poolRoot,string name)
    {
        poolObjRoot = new GameObject(name); //柜子节点命名为 柜子内部名字
        if (PoolManager.isLayoutGroup)
            //设置柜子节点位置
            poolObjRoot.transform.SetParent(poolRoot.transform);
    }
    public GameObject Pop()
    {
        //弹出栈对象
        GameObject obj = poolObjs.Pop();
        //激活对象
        obj.SetActive(true);
        if (PoolManager.isLayoutGroup)
            //取消父子关系
            obj.transform.SetParent(null);
        return obj;
    }
    public void Push(GameObject obj)
    {
        //失活
        obj.SetActive(false);
        if (PoolManager.isLayoutGroup)
            //设置柜子内部对象 父子关系
            obj.transform.SetParent(poolObjRoot.transform,false);
        poolObjs.Push(obj);
    }
}

public class PoolManager : BaseManagers<PoolManager>
{
    private PoolManager() { }
    //池子（柜子）容器
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject poolRoot; //池子 隐藏对象根节点 （失活过后隐藏在下面 避免窗口出现太多）
    public static bool isLayoutGroup = true; //是否使用布局组 （测试开始使用，把隐藏的对象统一放一起）

    /// <summary>
    /// 取出对象
    /// </summary>
    /// <param name="name"> 取出对象名字 </param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        GameObject obj; //返回对象
        //池子有对应 抽屉 并且 有东西则返回
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            //弹出栈对象给外部
            obj = poolDic[name].Pop();
        }
        else
        {
            //没有则创建
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name; //修改新创建对象名字
        }

        return obj;
    }
    /// <summary>
    /// 放入对象  （隐藏进入根对象）
    /// </summary>
    /// <param name="obj"> 要放入的对象 </param>
    public void PushObj(GameObject obj)
    {
        //创建隐藏根节点
        if (poolRoot == null)
            poolRoot = new("Root");

        //如果木有抽屉 则进行创造
        if (!poolDic.ContainsKey(obj.name))
        {
            poolDic.Add(obj.name, new PoolData(poolRoot,obj.name));
        }
        //放入抽屉
        poolDic[obj.name].Push(obj);
    }
    /// <summary>
    /// 清空池子  切换场景调用，避免内存泄漏
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolRoot = null;
    }
}
