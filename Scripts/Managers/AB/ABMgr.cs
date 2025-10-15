// using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//AB包资源管理器
public class ABMgr : SingletonAutoMono<ABMgr>
{
    //主包
    private AssetBundle mainAB = null;
    //主包依赖配置文件
    private AssetBundleManifest mainManifest = null;
    //AB包储存容器 避免重复加载
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();   

    /// <summary> AB 包加载路径
    /// </summary>
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary> 主包名称 不同平台名字不同 ISO / Android / PC
    /// 
    /// </summary>
    private string MainName
    {
        get
        {
            //TODO:后续编辑器拓展平台
            return "PC";
        }
    }

    //加载主包及其配置文件
    private void LoadMainAB()
    {
        if (mainAB == null)
        {
            //加载主包
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainName);
            //加载配置文件
            mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    #region  异步加载AB 包

    /// <summary> 异步加载 AB 包 （泛型加载） 
    /// 
    /// </summary>
    /// <typeparam name="T">  加载类型 </typeparam>
    /// <param name="abName"> 加载ab包名字 </param>
    /// <param name="resName"> 加载的资源名字</param>
    /// <param name="callBack"> 资源加载完毕回调 （携带资源参数） </param>
    /// <param name="isAsync"> 资源是否为同步加载 默认为 fasle 异步加载 </param>
    public void LoadResAsync<T>(string abName , string resName,UnityAction<T> callBack,bool isAsync = false) where T : Object
    {
        StartCoroutine(RealLoadResAsync<T>(abName, resName, callBack,isAsync));
    }
    private IEnumerator RealLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack, bool isAsync) where T : Object
    {
        //加载 主包
        LoadMainAB();
        //获取AB 包相关的资源包
        string[] dependencies = mainManifest.GetAllDependencies(abName);
        //加载依赖包
        foreach (string dependency in dependencies)
        {
            if (!abDic.ContainsKey(dependency))
            {
                //同步加载
                if (isAsync)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + dependency);
                    //添加记录
                    abDic.Add(dependency, ab);
                }
                else //异步加载
                {
                    //一来先添加记录 避免同帧调用重复加载 null先站位
                    abDic.Add(dependency, null);
                    //加载依赖包
                    AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(PathUrl + dependency);
                    //等待依赖包 加载完成
                    yield return abcr;
                    //添加记录
                    abDic[dependency] = abcr.assetBundle;
                }
            }
            else
            {
                //有可能正在加载中 不管同步还是异步加载 都需要等待加载完成 再进行
                while (abDic[dependency] == null)
                {
                    yield return 0; //等待一针 直到资源加载完成
                }
            }
        }

        //加载目标包 没有就进行加载记录
        if (!abDic.ContainsKey(abName))
        {
            if (isAsync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                //添加记录
                abDic.Add(abName, ab);
            }
            else
            {
                //添加占位记录
                abDic.Add(abName, null);
                //异步加载
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return abcr;
                //加载结束 填充资源
                abDic[abName] = abcr.assetBundle;

            }

        }
        else
        {
            //直到AB 包 加载结束 再进行处理资源
            while (abDic[abName] == null)
            {
                yield return 0; //等待一针 直到资源加载完成
            }
        }

        //同步加载资源
        if (isAsync)
        {
            //加载资源
            T obj = abDic[abName].LoadAsset<T>(resName);
            //回调返回资源
            callBack?.Invoke(obj);
        }
        else //异步加载
        {
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abr;
            //回调返回加载资源
            callBack?.Invoke(abr.asset as T);
        }

    }
  
    /// <summary> 异步加载 AB 包 （Type 加载 ） ——> Lua 热更新使用
    /// 
    /// </summary>
    /// <param name="abName"> ab包名字 </param>
    /// <param name="resName"> 加载的资源名字 </param>
    /// <param name="type"> 加载类型 </param>
    /// <param name="callBack"> 资源加载完毕回调 （携带资源参数） </param>
    /// <param name="isAsync"> 是否为 同步加载  默认为异步加载 </param>
    public void LoadResAsync(string abName,string resName, System.Type type,UnityAction<Object> callBack, bool isAsync = false)
    {
        StartCoroutine(RealLoadResAsyn(abName, resName, type, callBack,isAsync));
    }
     private IEnumerator RealLoadResAsyn(string abName, string resName, System.Type type,UnityAction<Object> callBack ,bool isAsync) 
    {
        //加载 主包
        LoadMainAB();
        //获取AB 包相关的资源包
        string[] dependencies = mainManifest.GetAllDependencies(abName);
        //加载依赖包
        foreach (string dependency in dependencies)
        {
            if (!abDic.ContainsKey(dependency))
            {
                //同步加载
                if (isAsync)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + dependency);
                    //添加记录
                    abDic.Add(dependency, ab);
                }
                else //异步加载
                {
                    //一来先添加记录 避免同帧调用重复加载 null先站位
                    abDic.Add(dependency, null);
                    //加载依赖包
                    AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(PathUrl + dependency);
                    //等待依赖包 加载完成
                    yield return abcr;
                    //添加记录
                    abDic[dependency] = abcr.assetBundle;
                }
            }
            else
            {
                //有可能正在加载中 不管同步还是异步加载 都需要等待加载完成 再进行
                while (abDic[dependency] == null)
                {
                    yield return 0; //等待一针 直到资源加载完成
                }
            }
        }
        //加载目标包 没有就进行加载记录
        if (!abDic.ContainsKey(abName))
        {
            if (isAsync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                //添加记录
                abDic.Add(abName, ab);
            }
            else
            {
                //添加占位记录
                abDic.Add(abName, null);
                //异步加载
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return abcr;
                //加载结束 填充资源
                abDic[abName] = abcr.assetBundle;

            }

        }
        else
        {
            //直到AB 包 加载结束 再进行处理资源
            while (abDic[abName] == null)
            {
                yield return 0; //等待一针 直到资源加载完成
            }
        }
        //同步加载资源
        if (isAsync)
        {
            //加载资源
            Object obj = abDic[abName].LoadAsset(resName,type);
            //回调返回资源
            callBack?.Invoke(obj);
        }
        else
        {
             //异步加载包中资源
            AssetBundleRequest abq = abDic[abName].LoadAssetAsync(resName, type);
            yield return abq;

            callBack(abq.asset);
        }
       
    }


    #endregion

    //卸载指定AB包
    public void UnLoadAB(string abName,UnityAction<bool> callBack ,bool isAsync = false)
    {
        StartCoroutine(RealUnLoadAB(abName, callBack, isAsync));
    }
    private IEnumerator RealUnLoadAB(string abName,UnityAction<bool> callBack ,bool isAsync)
    {
        if (abDic.ContainsKey(abName))
        {
            //判断资源是否还在加载
            if (abDic[abName] == null) //资源还没有加载完成
            {
                //等待加载完成卸载
                while (abDic[abName] == null)
                {
                    yield return 0; //等待一针 直到资源加载完成
                }
            }
            else //卸载资源
            {
                Debug.Log("资源加载完成 准备卸载");
                if (isAsync)
                {
                    abDic[abName].Unload(false);
                    //移除字典记录
                    abDic.Remove(abName);
                    callBack?.Invoke(true);
                }
                else
                {
                    AssetBundleUnloadOperation abUnlod = abDic[abName].UnloadAsync(false);
                    yield return abUnlod;
                    //移除字典记录
                    abDic.Remove(abName);
                    callBack?.Invoke(true);
                }
            }
        }
        else
        {
            Debug.LogError("资源并没有加载记录 卸载失败");
            callBack?.Invoke(false);
        }
    }
    //卸载所有AB包
    public void ClearAB()
    {
        //暂停之前所有协程
        StopAllCoroutines(); //TODO:暂停优化

        //卸载所有AB包 不清楚已经使用的缓存
        AssetBundle.UnloadAllAssetBundles(false);
        //清空主包
        mainAB = null;
        //清空配置文件
        mainManifest = null;
        //清空AB包字典
        abDic.Clear();
    }
}
