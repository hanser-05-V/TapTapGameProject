using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static GameManage _instance;

    void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        LoadGameData();
    }
    //游戏数据加载和初始化
    public void LoadGameData(){
        //初始播放一些文本
        DialogueContral.instance.LoadAiTree("1");
    }


}
