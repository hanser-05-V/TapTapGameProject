using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Meassage : MonoBehaviour
{

    
    [LabelText("头像"), SerializeField]
    private Image avacter;
    [LabelText("消息内容"), SerializeField]
    private TextMeshProUGUI content;
    [LabelText("聊天气泡"), SerializeField]
    private Image bubble;

    public void SetMassage(string  avacterName, string content)
    {
        //更新显示
        ABResMgr.Instance.LoadResAsync<Sprite>("sprite", avacterName, (sprite) =>
        {
            avacter.sprite = sprite;
        });
        this.content.text = content;
    }
    public void ChangeText(string content)
    {
        this.content.text = content;
    }
}
