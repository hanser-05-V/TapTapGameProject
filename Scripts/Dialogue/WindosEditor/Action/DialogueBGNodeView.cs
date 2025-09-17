using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.UIElements;

public class DialogueBGNodeView : DialogueDelegateNodeView
{
    private TextField bgNameField;

    public DialogueBGNodeView(DialogueBGNode node) : base(node)
    {
        title = "切换背景";

        bgNameField = new TextField("背景图名称")
        {
            value = node.bgName
        };
        bgNameField.RegisterValueChangedCallback(evt =>
        {
            node.bgName = evt.newValue;
        });
        mainContainer.Add(bgNameField);

    }
}
#endif
