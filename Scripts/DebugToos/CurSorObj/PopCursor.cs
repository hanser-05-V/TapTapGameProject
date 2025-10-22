using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCursor : CurSorObj
{
    public int cost = 1;
    protected override void Update()
    {
        base.Update();
        //点击就消耗值
        if (Input.GetMouseButtonDown(0))
        {
            GameDataMgr.Instance.currentDebugPoint -= cost;
        }
    }
}
