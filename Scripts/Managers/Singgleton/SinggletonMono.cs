using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承了Mono类 的单例模式
public class SinggletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;
    protected virtual void AWake()
    {
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}
