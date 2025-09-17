using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private List<GameObject> poolObjs = new List<GameObject>();
    private int index =0;
    void Start()
    {
        
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
           
            EventCenter.Instance.EventTrigger(E_EventType.OnLeftButtonClick);
            EventCenter.Instance.EventTrigger<Main>(E_EventType.OnTestFunc,this);
        }
        #endregion
    }
}
