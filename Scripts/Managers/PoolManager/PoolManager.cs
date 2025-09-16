using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



//池子数据（抽屉对象）
public class PoolData
{
    //柜子容器 （还没有使用过的对象）
    private Stack<GameObject> poolObjs = new Stack<GameObject>();
    //正在使用中 的 对象列表
    private List<GameObject> usedList = new List<GameObject>();
    //最大容量
    private int maxNumber; 
    private GameObject poolObjRoot; //柜子父节点
    public int UsedObjCount => usedList.Count;  
    public bool CanCreat => UsedObjCount< maxNumber; //使用对象还没有超出上线
    public float Count => poolObjs.Count;
    public PoolData(GameObject poolRoot, string name, GameObject usedobj)
    {
        //开启布局
        if (PoolManager.isLayoutGroup)
        {
            //柜子节点命名为 柜子内部名字
            poolObjRoot = new GameObject(name);
            //设置柜子节点位置
            poolObjRoot.transform.SetParent(poolRoot.transform);

        }
        //加入到 正在使用中 的列表
        AddUsedList(usedobj);
        //获得最大容量
        PoolObject obj = usedobj.GetComponent<PoolObject>();
        if (obj == null)
        {
            Debug.LogError("缓存对象没有挂载 PoolObject组件 ");
        }
        maxNumber = obj.MaxNumber;
       
    }
     //添加记录使用对象
    public void AddUsedList(GameObject obj)
    {
        usedList.Add(obj);
    }
    //出栈方法方法
    public GameObject Pop()
    {

        GameObject obj;
        //有还没有使用的对象
        if (poolObjs.Count > 0)
        {
            //弹出
            obj = poolObjs.Pop();
            usedList.Add(obj); //添加使用记录
        }
        else //已经到达使用上线
        {
            //以使用时间最长的对象 作为出栈对象
            obj = usedList[0];
            //更新使用记录
            usedList.RemoveAt(0);
            usedList.Add(obj); //添加到队伍尾
        }
        //激活
        obj.SetActive(true);
        //设置父子关系
        if (PoolManager.isLayoutGroup)
            obj.transform.SetParent(null, false);
        return obj;
    }
    //压栈方法
    public void Push(GameObject obj)
    {
        //失活
        obj.SetActive(false);
        if (PoolManager.isLayoutGroup)
            //设置柜子内部对象 父子关系
            obj.transform.SetParent(poolObjRoot.transform, false);

        //放入到空闲池子
        poolObjs.Push(obj);
        //移除使用记录
        usedList.Remove(obj);
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
    ///   取出对象 
    /// </summary>
    /// <param name="name"> 取出对象名字 </param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        
        GameObject obj = null;; //返回对象
        //创建隐藏根节点
        if (poolRoot == null)
            poolRoot = new("Root");
        //使用对象没有超上线时候 创建
        if (!poolDic.ContainsKey(name) || (poolDic[name].Count == 0 && poolDic[name].CanCreat))
        {
            //创建物体 用于记录使用着 的对象
            obj = GameObject.Instantiate(Resources.Load<GameObject>(GameConstData.ResLoadPrefabPath + name));
            obj.name = name;

            //如果没有柜子
            if (!poolDic.ContainsKey(name))
            {
                //创建抽屉
                poolDic.Add(name, new PoolData(poolRoot, name, obj));
            }
            else //柜子已经有了 直接记录
            {
                //添加到 正在使用中 的列表
                poolDic[name].AddUsedList(obj);
            }
        }
        else//有对象 使用超上限 
        {
            //直接取出来使用 （使用 最早出现的对象）
            obj = poolDic[name].Pop();
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
