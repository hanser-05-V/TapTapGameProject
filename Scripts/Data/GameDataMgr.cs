using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr : SinggletonMono<GameDataMgr>
{
    //关卡数据列表
    public List<LevelInfo> levelData_1 = new List<LevelInfo>();

    //初始化读取json文件
    override protected void Awake()
    {
        base.Awake();
        //读取json文件
        levelData_1 = JsonMgr.Instance.LoadData<List<LevelInfo>>("leve1");
    }
    

    //测试
    public void Load()
    {
        Debug.Log("levelData_1 count = " + levelData_1.Count);
    } 
}
