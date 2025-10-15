using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum JsonType 
{
    JsonUtlity,
    LitJson,
}


public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }


    public void SavaData(object _data,string _fileName,JsonType _type = JsonType.LitJson) 
    {
        string path = Application.persistentDataPath + "/" + _fileName +".json";

        string jsonStr = "";
        switch (_type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(_data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(_data); 
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    public T LoadData<T>(string _fileName,JsonType _type = JsonType.LitJson) where T : new()
    {
        //默认路径为StreamingAssets 
        string path = Application.streamingAssetsPath + "/" + _fileName + ".json";
        if (!File.Exists(path))
        {
            //如果不存在 则再可读写路径创造
            path = Application.persistentDataPath + "/" + _fileName + ".json";
        }
        if(!File.Exists(path))
        {
    
            return new T();
        }
        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (_type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        return data;
    }
}
