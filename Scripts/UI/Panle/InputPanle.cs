using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPanle : BasePanle
{
    [LabelText("输入框"), SerializeField]
    private TMP_InputField input;

    private string tipInput; //默认需要输入的文字
    private string avacterName;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        input.onValueChanged.AddListener(OnInput);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (input.text == tipInput)
            {
                //消息控制器发送消息
                UIMgr.Instance.GetPanle<LeveOneGamePanle>().SendMassage(avacterName, input.text, true);

                Debug.Log("游戏通过");
                //TODO:通过面板
            }
            else
            {
                //不一样清空输入内容
                input.text = "";
            }
            
        }
    }
    //输入事件
    private void OnInput(string value)
    {
        input.text = value;
    }
    /// <summary> 设置输入 发送 相关内容
    /// 
    /// </summary>
    /// <param name="avacterName"> 发送文字的头像 </param>
    /// <param name="inputText"> 想要玩家发送的文字 </param>
    public void SetDefultInputData(string avacterName,string inputText )
    {

        tipInput = inputText;
        this.avacterName = avacterName;

        // 获取默认输入文字的 Placeholder Text 组件
        TextMeshProUGUI placeholderText = input.placeholder.GetComponent<TextMeshProUGUI>();
        if (placeholderText != null)
        {
            placeholderText.text = tipInput; // 设置默认提示文字
        }
    }
}
