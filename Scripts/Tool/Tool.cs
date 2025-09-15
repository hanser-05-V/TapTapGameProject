using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//游戏工具类
public class Tool : MonoBehaviour
{
    //按钮事件响应
    public static void Button(Button _btn, Action _act)
    {

        if (_btn == null) return;
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() =>
        {
            _act?.Invoke();
        });
    }

    #region 动态加载区域
    //动态加载预制体
    /// <summary>
    /// 动态加载预制体
    /// </summary>
    /// <param name="prefabName">预制体名称（不带扩展名）</param>
    /// <returns>加载的预制体实例</returns>
    public static GameObject LoadPrefab(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            Debug.LogError("Prefab path cannot be null or empty.");
            return null;
        }

        // 使用 Resources.Load 来加载预制体
        GameObject prefab = Resources.Load<GameObject>(GameConstData.ResLoadPrefabPath + relativePath);

        if (prefab == null)
        {
            Debug.LogError("Failed to load prefab: " + relativePath);
            return null;
        }

        return prefab;
    }

    // 3D场景内动态加载，并设置父节点
    public static GameObject LoadPrefab(string relativePath, Transform parent)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            Debug.LogError("Prefab path cannot be null or empty.");
            return null;
        }

        // 加载预制体
        GameObject prefab = Resources.Load<GameObject>(GameConstData.ResLoadPrefabPath + relativePath);

        if (prefab == null)
        {
            Debug.LogError("Failed to load prefab: " + relativePath);
            return null;
        }

        // 实例化预制体并设置父节点
        GameObject obj = GameObject.Instantiate(prefab, parent);

        // 设置局部位置、旋转和缩放为默认
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        return obj;
    }

    //动态加载图片资源
    public static Sprite LoadImage(string imageName, float pixelsPerUnit = 100f)
    {
        if (string.IsNullOrEmpty(imageName))
        {
            Debug.LogError("图片名称不能为空！");
            return null;
        }

        string fullPath = $"{GameConstData.ResLoadSpritPath}/{imageName}";
        Texture2D texture = Resources.Load<Texture2D>(fullPath);

        if (texture == null)
        {
            Debug.LogError($"图片加载失败！路径：{fullPath}");
            return null;
        }

        return Sprite.Create(texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f),
                            pixelsPerUnit);
    }
    #endregion

    #region 类相关工具
    //类的深拷贝
    public static T CopyClass<T>(T _class)
    {
        if (_class == null) return default;

        string json = JsonUtility.ToJson(_class);
        return JsonUtility.FromJson<T>(json);

    }
    #endregion
}
