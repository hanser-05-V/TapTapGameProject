using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单条 关卡数据
public class LevelOneInfo
{
    /// <summary> 0-游戏结束  ，1-弹窗游戏 数据文件夹  2-爬充游戏 数据文件夹
    /// 
    /// </summary>
    public int id; //Bug 数据ID （出现Bug时候根据Id 去读取Bug）
    public string avacter;//聊天头像名字
    public string context;//聊天内容
    public bool isRight; //是否是右边的消息 
    public float Intervaltime; //间隔时间

    public float AngleValue; //增加的怒气值
    public bool isBugShow; //是否显示bug
   
    public string DataName; //关卡数据名称

}
