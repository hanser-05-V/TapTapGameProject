using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//自动挂载Mono 的 单例模式
public class SingletonAutoMono<T> : MonoBehaviour  where T : MonoBehaviour
{
    private static T instance;


    public static T Instance 
    {
        
        get
        {
            if (instance == null)
            {
                //创建对象
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();

                //自动挂载脚本
                instance = obj.AddComponent<T>();
                //过场景不删除
                DontDestroyOnLoad(obj);
            }
            return instance;
        }


    }
}
