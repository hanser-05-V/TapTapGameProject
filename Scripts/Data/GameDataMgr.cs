using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr : SinggletonMono<GameDataMgr>
{
    //关卡数据列表
    public List<LevelOneInfo> levelData_1 = new List<LevelOneInfo>();
    public List<LeveTwoInfo> LeveTwoData { get; private set; } = new List<LeveTwoInfo>();
    //初始化读取json文件

    //游戏开始 //TODO:配置优化
    public bool isGameStart = false;
    public bool isGameOver = false;

    // public float tatalDebugPoint = 100;
    public float currentDebugPoint = 100;
    public bool canDebug =  true;
    override protected void Awake()
    {
        base.Awake();
        //TODO:数据分关卡读取
        //读取json文件
        levelData_1 = JsonMgr.Instance.LoadData<List<LevelOneInfo>>("leve1");
        LeveTwoData = JsonMgr.Instance.LoadData<List<LeveTwoInfo>>("LeveTwo");
    }
    private void Update()
    {
        if(currentDebugPoint <= 0)
        {
            canDebug = false;
        }
    }
    //加载关卡数据
    public void LoadLevelData(int level)
    {
        
    }

    //测试
    public void Load()
    {
        
    } 
}
