using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug工具父类
public class CurSorObj : MonoBehaviour
{
    
     protected float currentDebugPoint; //当前debug点数
    protected virtual void Update()
    {
        currentDebugPoint = GameDataMgr.Instance.currentDebugPoint; //获取当前debug点数
    }


}
