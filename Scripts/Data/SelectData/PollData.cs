using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PollData", menuName = "Data/PollData")]
public class PollData : BugData
{   
    
    public int pollNUmber = 5; //出现数量
    // public E_PollTransposType routeType; //路线
    public E_PollType pollType; //污染类型
    public float pollInterval = 0.5f; //间隔时间
}
