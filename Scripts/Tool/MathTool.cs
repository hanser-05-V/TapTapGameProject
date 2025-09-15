using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//数学工具类
public class MathTool : MonoBehaviour
{
    //float向int转化
    public static int FloatToInt(float _f)
    {
        return Mathf.RoundToInt(_f);
    }

    /// 保留小数点后 n 位
    public static float KeepDecimal(float value, int decimals)
    {
        float factor = Mathf.Pow(10, decimals);
        return Mathf.Round(value * factor) / factor;
    }
    //权重抽取
    public static int WeightToRandom(int[] _item, int[] _weight)
    {
        if (_item == null || _weight == null || _item.Length == 0 || _item.Length != _weight.Length)
        {
            Debug.LogError("WeightToRandom 参数错误：数组为空或长度不一致");
            return -1;
        }

        // 计算总权重
        int totalWeight = 0;
        for (int i = 0; i < _weight.Length; i++)
        {
            if (_weight[i] < 0)
            {
                Debug.LogError("WeightToRandom 参数错误：权重不能为负数");
                return -1;
            }
            totalWeight += _weight[i];
        }

        if (totalWeight == 0)
        {
            Debug.LogError("WeightToRandom 参数错误：总权重为 0");
            return -1;
        }

        
        int randomValue = Random.Range(0, totalWeight);

        
        int cumulative = 0;
        for (int i = 0; i < _item.Length; i++)
        {
            cumulative += _weight[i];
            if (randomValue < cumulative)
            {
                return _item[i];
            }
        }
        return _item[_item.Length - 1];
    }

}
