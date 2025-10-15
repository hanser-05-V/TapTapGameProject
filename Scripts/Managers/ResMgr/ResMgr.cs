using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//资源抽象基类
public abstract class ResInfoBase
{
    //引用计数
    public int refCount;
}
//具体资源类
public class ResInfo<T> : ResInfoBase
{
    //资源文件
    public T asset;
    //加载完毕回调函数
    public UnityAction<T> callBack;
    //加载协程
    public Coroutine loadCoroutine;
    //是否立即删除标记
    public bool isDelete;

    //增加引用计数
    public int AddRefCount()
    {
        ++refCount;
        return refCount;
    }
    //减少引用计数
    public int ReduceRefCount()
    {
        --refCount;
        if (refCount <= 0)
        {
            Debug.LogError("引用计数小于0 无法继续卸载资源");
        }
        return refCount;
    }
}


//资源加载器
public class ResMgr : BaseManager<ResMgr>
{
    private ResMgr() { }

    //资源加载记录容器
    private Dictionary<string, ResInfoBase> resDic = new Dictionary<string, ResInfoBase>();

    #region  加载资源

    /// <summary> 同步加载资源
    /// 
    /// </summary>
    /// <typeparam name="T">  资源类型 </typeparam>
    /// <param name="resName"> 资源路径 </param>
    /// <returns></returns>
    public T LoadRes<T>(string path) where T : Object
    {
        string resName = path + "_" + typeof(T).Name;
        //没有记载过这个资源
        if (!resDic.ContainsKey(resName))
        {
            //直接同步加载资源
            T asset = Resources.Load<T>(path);
           
            //加入资源记录
            ResInfo<T> resInfo = new ResInfo<T>();
            resInfo.asset = asset;
            resInfo.AddRefCount();

            // //如果当前引用计数为0 说明资源已经不再被使用 立即卸载
            // if (resInfo.refCount == 0)
            // {
            //     UnloadRes<T>(path, false);
            // }

            //加入记录字典
            resDic.Add(resName, resInfo);
            //返回资源
            return asset;
        }
        else
        {
            //取出资源
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            resInfo.AddRefCount(); //增加引用计数

            //判断资源是否 已经加载完毕了 （可能之前是异步加载）
            if (resInfo.asset == null) //还没有加载完
            {
                //直接停止协程
                MonoMgr.Instance.StopCoroutine(resInfo.loadCoroutine);
                //直接同步加载
                T asset = Resources.Load<T>(path);
                //更新资源
                resInfo.asset = asset;
                //调用回调函数（异步地方可以有加载完毕的回调）
                resInfo.callBack?.Invoke(asset);

                //清空引用 避免内存泄漏
                resInfo.callBack = null;
                resInfo.loadCoroutine = null;

                //返回资源
                return asset;
            }
            else //资源已经加载完毕了 
            {
                //直接返回资源
                return resInfo.asset;

            }
        }
    }

    /// <summary> 异步加载资源
    /// 
    /// </summary>
    /// <typeparam name="T"> 资源类型 </typeparam>
    /// <param name="path"> 资源路径 </param>
    /// <param name="callBack"> 记载完毕回调函数 </param>
    public void LoadResAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        string resName = path + "_" + typeof(T).Name;
        //没有记载过这个资源
        if (!resDic.ContainsKey(resName))
        {
            //资源记录
            ResInfo<T> resInfo = new ResInfo<T>();
            //添加记录 (资源还没有加载完成)
            resInfo.AddRefCount();
            resDic.Add(resName, resInfo);
            //添加回调
            resInfo.callBack += callBack;
            //开启记录协程
            resInfo.loadCoroutine = MonoMgr.Instance.StartCoroutine(LoadResAsyncCoroutine(path, callBack));

        }
        else //已经记载过这个资源
        {
            //取出资源
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            //增加引用计数
            resInfo.AddRefCount();
            //判断此时资源是否已经加载完成了
            if (resInfo.asset == null) //还没有加载完
                resInfo.callBack += callBack; //添加回调
            else //资源已经加载完毕了
            {
                //直接调用回调函数
                callBack?.Invoke(resInfo.asset);
            }
        }
    }
    //异步加载协程
    private IEnumerator LoadResAsyncCoroutine<T>(string path, UnityAction<T> callBack) where T : Object
    {
        //异步加载
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
        yield return resourceRequest; //等待加载完毕
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            //取出资源
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            //更新资源
            resInfo.asset = resourceRequest.asset as T;
            //判断是否是待删除状态
            if (resInfo.isDelete && resInfo.refCount == 0)
            {
                //卸载资源
                UnloadRes<T>(path, resInfo.isDelete, null, false);
            }
            //执行回调函数
            resInfo.callBack?.Invoke(resInfo.asset);
            //清空引用
            resInfo.callBack = null;
            resInfo.loadCoroutine = null;
        }

    }

    #endregion

    #region 卸载资源 (手动释放资源,避免内存泄漏)

    /// <summary> 卸载资源（同步）
    /// 
    /// </summary>
    /// <typeparam name="T"> 资源类型</typeparam>
    /// <param name="path"> 资源路径 </param>
    /// <param name="isDel"> 是否立即删除 </param>
    /// <param name="callBack"> 卸载完毕回调函数  默认为同步加载卸载资源 异步加载卸载</param>
    public void UnloadRes<T>(string path, bool isDel = false, UnityAction<T> callBack = null, bool isSub = true) where T : Object
    {
        string resName = path + "_" + typeof(T).Name;
        //没有记载过这个资源
        if (!resDic.ContainsKey(resName))
            Debug.LogError("没有加载过这个资源");
        else
        {
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;

            //是否减少引用计数 （常用于确保资源在不再被使用时被卸载 防止异步还没有加载完毕）
            if (isSub)
                resInfo.ReduceRefCount(); //减少引用计数
            else
            {
                if (resInfo.asset != null && resInfo.refCount == 0 && resInfo.isDelete)
                {
                    //卸载
                    Resources.UnloadAsset(resInfo.asset as UnityEngine.Object);
                    //字典记录中移除
                    resDic.Remove(resName);
                }
                else if (resInfo.asset == null)// 还没有加载完毕
                {
                    //进入代删除状态 等待资源加载完毕过后再删除
                    //resInfo.isDel = true;
                    //处理异步回调，某一个异步不使用了，应该处理异步回调函数 只有当计数为0 才真正删除
                    if (callBack != null)
                        resInfo.callBack -= callBack;
                }
            }
        }
    }
    /// <summary> 异步卸载
    /// 
    /// </summary>
    /// <param name="callBack"> 卸载完毕回调函数 </param>
    public void UnloadUnusedAssets(UnityAction callBack)
    {
        MonoMgr.Instance.StartCoroutine(UnloadUnusedAsset(callBack));
    }
    private IEnumerator UnloadUnusedAsset(UnityAction callBack)
    {
        //先把引用计数为0，并且没有被移除记录 的资源移除
        List<string> list = new List<string>();
        foreach (string path in list)
        {
            if (resDic[path].refCount == 0)
                list.Add(path);
        }
        foreach (string path in list)
        {
            resDic.Remove(path);
        }
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        callBack?.Invoke();
    }

    #endregion

    //得到当前资源的引用计数
    public int GetRefCount<T>(string path)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            return (resDic[resName] as ResInfo<T>).refCount;
        }
        return 0;
    }

}
