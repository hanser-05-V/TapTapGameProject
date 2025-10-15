using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerMeterSystem : BaseManager<AngerMeterSystem>
{
    private  AngerMeterSystem(){}

    //当前怒气值
    public float CurrentAngerValue { get; private set; }


    /// <summary> 增加怒气值
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void AddAngerValue(float value)
    {
        CurrentAngerValue += value;
    }

    /// <summary> 减少怒气值
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void ReduceAngerValue(float value)
    {
        CurrentAngerValue -= value;
        if (CurrentAngerValue < 0)
        {
            CurrentAngerValue = 0;
        }

        
    }
    
    /// <summary>重置怒气值
    /// 
    /// </summary>
    public void ResetAngerValue()
    {
        CurrentAngerValue = 0;
    }
}
