using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
