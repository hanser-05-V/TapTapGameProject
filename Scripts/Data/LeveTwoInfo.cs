using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeveTwoInfo 
{
    public int id; //数据索引
    public string studentTalk; //学生说的话
    public string teacherTalk; //老师说的话

    public float InverTime; //消息间隔事件

    public bool isBugShow; //BUg出现 开始游戏

    public bool isSinggle; //是否是单条爬虫数据
    public bool isGroup;   //是否是组爬虫数据

    public string DataName; //数据名字
}
