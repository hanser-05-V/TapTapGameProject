using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueTextNode")]
public class DialogueTextNode : DialogueNode
{
    [TextArea]
    public string dialogueText;

    public override void OnEnter()
    {
        
    }
}
