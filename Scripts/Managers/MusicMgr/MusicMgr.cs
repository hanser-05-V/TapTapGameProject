using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//音乐管理器
public class MusicMgr : BaseManager<MusicMgr>
{
    private MusicMgr()
    {
        MonoMgr.Instance.AddFixedUpdatListener(Update); //避免每帧频繁调用检测 浪费性能
    }

    //背景音乐播放组件
    private AudioSource bkMusic;
    //背景音乐大小
    private float bkMusicValue = 0.5f;

    //正在播放的音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 0.5f;
    //当前音效是否在播放 （用于播放结束自动删除） 全局控制 是否播放音效
    private bool soundIsPlay = true; 

    #region  背景音乐

    /// <summary> 播放背景音乐 （包括切换音乐）
    /// 
    /// </summary>
    /// <param name="muisicName"> 背景音乐名字 </param>
    public void PlayBKMusic(string muisicName)
    {
        //确保有背景音乐组件
        if (bkMusic == null)
        {
            //创建挂载对象
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            //过场景不删除
            GameObject.DontDestroyOnLoad(obj);
            //添加组件记录
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //AB包中加载资源 （默认为异步加载）
        ABResMgr.Instance.LoadResAsync<AudioClip>("music", muisicName, (clip) =>
        {
          
            //设置切片
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicValue;
            //播放
            bkMusic.Play();

        });
    }

    /// <summary> 停止背景音乐
    /// 
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    /// <summary> 暂停背景音乐
    /// 
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary> 更改背景音乐大小
    /// 
    /// </summary>
    /// <param name="volue"> 更新值 </param>
    public void ChangeBKMusicValue(float volue)
    {
        //更改数据
        bkMusicValue = volue;
        if (bkMusic == null)
            return;
        //更新音量
        bkMusic.volume = bkMusicValue;
    }

    #endregion 

    #region  音效
    /// <summary> 播放音效
    /// </summary>
    /// <param name="soundName"> 音效名字 </param>
    /// <param name="isLoop"> 是否循环 </param>
    /// <param name="isAsync"> 是否为同步加载 默认true 为同步加载 </param>
    /// <param name="callBack"> 加载完成回调 默认为 null</param>
    public void PlaySound(string soundName, bool isLoop = false, bool isAsync = true, UnityAction<AudioSource> callBack = null)
    {
        //加载音效 进行播放
        ABResMgr.Instance.LoadResAsync<AudioClip>("sound", soundName, ((clip) =>
        {
            //创建挂载对象
            AudioSource souce = PoolMgr.Instance.GetObj("Sound/SoundObj").GetComponent<AudioSource>();

            //先暂停 （可能是上次使用的）
            souce.Pause();
            //设置数据
            souce.clip = clip;
            souce.loop = isLoop;
            souce.volume = soundValue;
            //播放 
            souce.Play();
            //加入播放容器
            if (!soundList.Contains(souce))
                soundList.Add(souce);
            //外部回调使用
            callBack?.Invoke(souce);

        }), isAsync);
    }

    /// <summary> 停止音效
    /// 
    /// </summary>
    /// <param name="souce"> 音效对象 </param>
    public void StopSound(AudioSource souce)
    {
        if (soundList.Contains(souce))
        {
            //停止播放
            souce.Stop();
            //移除正常播放列表
            soundList.Remove(souce);
            //清空切片文件
            souce.clip = null;
            //回收到对象池
            PoolMgr.Instance.PushObj(souce.gameObject);
        }
    }

    /// <summary> 继续播放 / 暂停 音效 (控制所有音效)
    /// 
    /// </summary>
    /// <param name="isPlay"> true 为继续播放 false为暂停音效 </param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            soundIsPlay = true;
            //遍历播放音效列表
            foreach (var souce in soundList)
            {
                souce.Play();
            }
        }
        else
        {
            soundIsPlay = false;
            //遍历暂停所有音效
            foreach (var souce in soundList)
            {
                souce.Pause();
            }
        }
    }
    
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        //修改已经创建好的音效
        foreach (var souce in soundList)
        {
            souce.volume = soundValue;
        }
    }

    /// <summary> 非循环音效 播放结束进行销毁
    /// 
    /// </summary>
    private void Update()
    {
        //暂停停止检测
        if (!soundIsPlay)
            return;
        //反向遍历列表 检测播放结束音效
        for(int i =soundList.Count - 1; i >= 0; i--)
        {
            //没有再进行播放
            if (!soundList[i].isPlaying)
            {
                //进行移除
                soundList[i].clip = null;
                //回收到对象池
                PoolMgr.Instance.PushObj(soundList[i].gameObject);
                soundList.RemoveAt(i);
            }
        }
    }
    #endregion


}
