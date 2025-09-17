using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

//未继承 Mono 管理者者单例类
//管理者要声明显示 私有构造函数
public class BaseManager<T> where T : class
{

    private static T instance;
    public static T Instance
    {

        get
        {
            if (instance == null)
            {
                //获取类型
                Type type = typeof(T);
                //反射获取构造函数进行创建
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                   null,
                                                                   Type.EmptyTypes,
                                                                   null);

                //创建实例
                instance = constructorInfo.Invoke(null) as T;
            }

            return instance;

        }

    }
}
