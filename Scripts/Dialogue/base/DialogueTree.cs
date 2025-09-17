using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueTree")]
public class DialogueTree : ScriptableObject
{
    public DialogueNode rootNode;
    public List<DialogueNode> allNodes = new List<DialogueNode>();
}
