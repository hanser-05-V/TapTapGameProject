using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class UIMgr : BaseManager<UIMgr>
{
    private Canvas uiCanVas;
    private GameObject eventSystem;
    private Camera uiCamera;
    private UIMgr()
    {
        //动态创建Canvas 、摄像机 、EventSystem 
        uiCanVas = GameObject.Instantiate(ResMgr.Instance.LoadRes<GameObject>("UI/Canvas")).GetComponent<Canvas>();
        GameObject.DontDestroyOnLoad(uiCanVas.gameObject);
        //
        uiCamera = GameObject.Instantiate(ResMgr.Instance.LoadRes<GameObject>("UI/UICamera")).GetComponent<Camera>();
        GameObject.DontDestroyOnLoad(uiCamera.gameObject);
        // 
        eventSystem = GameObject.Instantiate(ResMgr.Instance.LoadRes<GameObject>("UI/EventSystem"));
        GameObject.DontDestroyOnLoad(eventSystem);



    }

    //面板容器
    private Dictionary<string, BasePanle> panleDic = new Dictionary<string, BasePanle>();


    /// <summary> 显示面板
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public T Show<T>() where T : BasePanle
    {
        //面板名 面板名和挂载在身上脚本名 一致
        string panleName = typeof(T).Name;

        //如果存在面板 之间取出来激活
        if (panleDic.ContainsKey(panleName))
        {
            //取出面板
            BasePanle panle = panleDic[panleName];
            //设置父对象
            panle.transform.SetParent(uiCanVas.transform, false);
            //TODO:激活
            panle.gameObject.SetActive(true);
            //显示面板
            panle.Showme();

            return panle as T;
        }
        else //否则进行 加载显示 
        {
            //TODO:优化加载

            BasePanle panle = null;
            ABResMgr.Instance.LoadResAsync<GameObject>("ui", panleName, (res) =>
            {

                GameObject panleObj = GameObject.Instantiate(res);
                panleObj.transform.SetParent(uiCanVas.transform, false);
                panle = panleObj.GetComponent<BasePanle>();
                panle.Showme();

                panleDic.Add(panleName, panle);
            });
            return panle as T;


            // GameObject panleObj = GameObject.Instantiate(EditorResMgr.Instance.LoadEditorRes<GameObject>("ui/" + panleName));
            //  panleObj.transform.SetParent(uiCanVas.transform, false);
            //     BasePanle panle = panleObj.GetComponent<BasePanle>();
            //     panle.Showme();

            //     panleDic.Add(panleName, panle);

        }
    }

    /// <summary> 隐藏面板
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade"></param>
    public void Hide<T>(bool isFade = true) where T : BasePanle
    {
        //TODO:进一步优化
        string panleName = typeof(T).Name;
        if (panleDic.ContainsKey(panleName))
        {
            BasePanle panle = panleDic[panleName];
            panle.Hideme(isFade, () =>
            {
                //TODO:优化销毁

                //失活
                panle.gameObject.SetActive(false);
            });

        }
    }

    
    public T GetPanle<T>() where T : BasePanle //得到面板
    {
        //面板名 面板名和挂载在身上脚本名 一致
        string panleName = typeof(T).Name;
        if (panleDic.ContainsKey(panleName) && panleDic[panleName].gameObject.activeSelf)  //有记录而且处于激活状态
        {
            return panleDic[panleName] as T;
        }
        else
        {
            Debug.LogError(panleName + "未记录 或 面板被隐藏了");
            return null;
        }
    }
}
