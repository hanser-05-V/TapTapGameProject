using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr : SinggletonMono<GameDataMgr>
{
    //关卡数据列表
    public List<LevelInfo> levelInfos = new List<LevelInfo>();

    //初始化读取json文件
    override protected void Awake()
    {
        base.Awake();
        //读取json文件
        levelInfos = JsonMgr.Instance.LoadData<List<LevelInfo>>("leve1");
    }
    
}
