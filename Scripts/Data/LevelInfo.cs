using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单条 关卡数据
public class LevelInfo
{
    public int id; //关卡id
    public string avacter;//聊天头像名字
    public string context;//聊天内容

    public float Intervaltime; //间隔时间

    public bool isBugShow; //是否显示bug

    public float AngleValue; //怒气值
}
