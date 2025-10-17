using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawBugGame : MonoBehaviour
{
    private void OnEnable()
    {
        EventCenter.Instance.RegisterEventListener(E_EventType.E_CrawBugGame, StartGame);
    }
    private void OnDisable()
    {
        EventCenter.Instance.UnRegisterEventListener(E_EventType.E_CrawBugGame, StartGame);
    }
    private void StartGame()
    {
        Debug.Log("开始爬虫游戏");
    }

    //创建爬虫
}

