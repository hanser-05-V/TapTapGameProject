using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private List<GameObject> poolObjs = new List<GameObject>();
    private int index =0;
    void Start()
    {
        #region  资源加载
        // ABMgr.Instance.LoadResAsync<GameObject>("ui", "BeginPanle", (res) =>
        // {
        //     GameObject gameObject = Instantiate(res);
        // });
        // Instantiate(EditorResMgr.Instance.LoadEditorRes<GameObject>("ui/BeginPanle"));

        ABResMgr.Instance.LoadResAsync<GameObject>("ui", "BeginPanle", (res) =>
        {
            GameObject gameObject = Instantiate(res);
        });
        #endregion

        #region 背景音乐

        MusicMgr.Instance.PlayBKMusic("Begin");
        MusicMgr.Instance.ChangeBKMusicValue(0.1f);
        #endregion
    }

    void Update()
    {
        #region Pool 缓存池测试
        // if (Input.GetMouseButtonDown(0))
        // {
        //     GameObject obj = PoolManager.Instance.GetObj("PoolTest/BulletTest1");
        //     obj.transform.position = Vector3.zero;

        //     poolObjs.Add(obj);
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     index++;
        //     PoolManager.Instance.PushObj(poolObjs[index]);
        // }
        #endregion
        #region  事件中心 测试
        if (Input.GetMouseButtonDown(0))
        {

            // EventCenter.Instance.EventTrigger(E_EventType.OnLeftButtonClick);
            // EventCenter.Instance.EventTrigger<Main>(E_EventType.OnTestFunc,this);
            // ResMgr.Instance.LoadResAsync<GameObject>(GameConstData.ResLoadPrefabPath + "PoolTest/BulletTest1", (obj) =>
            // {
            //     GameObject bullet = GameObject.Instantiate(obj);
            // });


        }
        #endregion
        #region 音效
        if (Input.GetKeyDown(KeyCode.P))
        {
            MusicMgr.Instance.PlayOrPauseSound(false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            MusicMgr.Instance.PlayOrPauseSound(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            MusicMgr.Instance.PlaySound("hit_4");
            MusicMgr.Instance.PlaySound("hit_4", true);
        }
        #endregion


    }
    public void TestFun(GameObject obj)
    {
        // GameObject bullet = GameObject.Instantiate(obj);
    }
}
