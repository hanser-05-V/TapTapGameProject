using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContral : MonoBehaviour
{
    public static DialogueContral instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dialogueUI = GetComponent<DialogueManage>();
    }

    public DialogueGraphData graphData;  // 当前对话树
    private Dictionary<string, DialogueNode> nodeLookup;
    public DialogueNode currentNode;
    public RectTransform trans_Panel;
    public DialogueManage dialogueUI; // 你的UI脚本
    [Tooltip("暂停标记")] public bool isPaused;
    private DialogueNode pausedNode;
    private bool isSkipping = false;
    private void Start()
    {
        if (graphData != null)
        {
            InitializeGraphData(graphData);
        }

    }
    // 2. 传入资源名，动态加载 Resources 里的对话树资源并开始对话
    public Queue<string> dialogueQueue = new Queue<string>();
    private bool isDialoguePlaying = false;
    public void LoadAiTree(string graphName)
    {
        
        // 如果当前有对话在播放，则入队
        if (isDialoguePlaying)
        {
            dialogueQueue.Enqueue(graphName);
            return;
        }

        PlayDialogueTree(graphName);
    }
    private void PlayDialogueTree(string graphName)
    {

        
        isDialoguePlaying = true;
        trans_Panel.gameObject.SetActive(true);

        string path = $"dialogueaitree/{graphName}";
        var originalGraph = Resources.Load<DialogueGraphData>(path);

        if (originalGraph == null)
        {
            Debug.LogError($"未能从Resources加载对话树: {path}");
            isDialoguePlaying = false; // 防止卡死
            TryPlayNextFromQueue(); // 尝试播放下一个
            return;
        }
        
        // ✅ 克隆 ScriptableObject 本体
        graphData = Instantiate(originalGraph);

        // ✅ 再克隆节点内容
        CloneNodesIntoGraph(graphData);

        // ✅ 初始化
        InitializeGraphData(graphData);

        StartDialogue();
    }
    public void OnDialogueEnd()
    {
        isDialoguePlaying = false;
        TryPlayNextFromQueue();
    }
    private void TryPlayNextFromQueue()
    {
        if (dialogueQueue.Count > 0)
        {
            string nextTree = dialogueQueue.Dequeue();
            PlayDialogueTree(nextTree);
        }
    }
    private void CloneNodesIntoGraph(DialogueGraphData dialogueGraph)
    {
        var clonedNodes = new List<DialogueNode>();

        foreach (var node in dialogueGraph.allNodes)
        {
            if (node == null)
            {
                Debug.LogWarning("检测到已销毁节点，跳过");
                continue;
            }

            var clone = Instantiate(node); // 克隆 ScriptableObject 节点
            clonedNodes.Add(clone);
        }

        // ✅ 注意：这里只改 runtime 用的 graphData，不会污染资源文件
        dialogueGraph.allNodes = clonedNodes;
    }
    // 初始化节点字典
    private void InitializeGraphData(DialogueGraphData dialogueGraph)
    {
        nodeLookup = new Dictionary<string, DialogueNode>();
        var clonedNodes = new List<DialogueNode>();

        foreach (var node in dialogueGraph.allNodes)
        {
            if (node == null)
            {
                Debug.LogWarning("检测到已销毁节点，跳过");
                continue;
            }
            // 复制节点，避免原资源被卸载导致引用丢失
            var clone = ScriptableObject.Instantiate(node);
            clonedNodes.Add(clone);
            nodeLookup[clone.nodeGUID] = clone;
        }

        // 把graphData.allNodes换成克隆节点列表，避免后续访问原始资源
        dialogueGraph.allNodes = clonedNodes;
    }

    public void StartDialogue()
    {
        currentNode = null;


        foreach (var node in graphData.allNodes)
        {

            if (node is DialogueStartNode)
            {
                currentNode = node;

                break;
            }
        }

        if (currentNode == null)
        {
            Debug.LogError("找不到开始节点");
            return;
        }

        EnterNode(currentNode);
    }

    public void EnterNode(DialogueNode node)
    {
        if (node == null)
        {
            Debug.LogError("EnterNode 时节点为空！");
            return;
        }
        DialogueManage._instance.isActive = true;
        currentNode = node;
        
        switch (node)
        {   
            case DialogueTextNode textNode:
                if (isSkipping)
                {
                    MoveToNextNode(); // 跳过当前文本
                }
                else
                {
                    dialogueUI.ShowDialogue(textNode.speakerName, textNode.dialogueText, () =>
                    {
                        MoveToNextNode();
                    });
                }
                break;

            case DialogueChoiceNode choiceNode:
                StopSkip();
                dialogueUI.ShowChoices(choiceNode.choices, choiceIndex =>
                {
                    if (choiceIndex < choiceNode.nextNodes.Count)
                    {
                        string nextNodeGUID = choiceNode.nextNodes[choiceIndex];
                        if (nodeLookup.TryGetValue(nextNodeGUID, out var nextNode))
                        {
                            EnterNode(nextNode);
                        }
                        else
                        {
                            Debug.LogError($"选择节点指向的下一个节点不存在，GUID: {nextNodeGUID}");
                        }
                    }
                    else
                    {
                        Debug.LogError("选择索引超出范围");
                    }
                });
                break;

            case DialogueEndNode _:
                dialogueUI.EndDialogue();
                OnDialogueEnd();
                break;

            case DialoguePauseNode pauseNode:
                
                isPaused = true;
                pausedNode = pauseNode;
                dialogueUI.Hide(); // 隐藏对话 UI
                break;

            case DialogueDelegateNode delegateNode:
                delegateNode.OnEnter(); // 执行委托逻辑
                break;

            case DialogueConditionNode conditionNode:
            {
                StopSkip();
                DialogueNode nextNode = conditionNode.GetNextNode(nodeLookup);
                if (nextNode != null)
                {
                    EnterNode(nextNode);
                }
                else
                {
                    Debug.LogWarning("条件节点未找到下一个节点或判断失败");
                    dialogueUI.EndDialogue();
                }
                break;
            }

            default:
                MoveToNextNode();
                break;
        }
    }

    public void ResumeDialogue()
    {
        if (isPaused && pausedNode != null)
        {
            isPaused = false;
            DialogueManage._instance.NoHide(); // 恢复 UI 显示
            pausedNode = null;
            MoveToNextNode();
        }
        else
        {
            Debug.LogWarning("没有暂停节点可恢复");
        }
    }
    public void MoveToNextNode()
    {
        if (isPaused) return; // 暂停时不继续

        if (currentNode.nextNodes != null && currentNode.nextNodes.Count > 0)
        {
            string nextGuid = currentNode.nextNodes[0];
            if (nodeLookup.TryGetValue(nextGuid, out var nextNode))
            {
                EnterNode(nextNode);
            }
            else
            {
                Debug.LogError("下一个节点不存在");
            }
        }
        else
        {
            dialogueUI.EndDialogue();
        }
    }
    public void StartSkip()
    {
        if (isPaused) return; // 暂停时不能跳过

        isSkipping = true;
        if (currentNode is DialogueTextNode)
        {
            MoveToNextNode(); // 当前是文本节点，立刻进入下一个
        }
    }
    public void StopSkip()
    {
        isSkipping = false;
    }

    public void StartSkipMode()
    {
        StartCoroutine(SkipThroughDialogue());
    }

    private IEnumerator SkipThroughDialogue()
    {
        dialogueUI.skipMode = true;

        while (true)
        {
            if (isPaused)
            {
                yield return new WaitUntil(() => !isPaused); // 等待恢复
            }

            if (currentNode is DialogueChoiceNode || currentNode is DialogueEndNode)
            {
                dialogueUI.skipMode = false;
                yield break;
            }

            if (currentNode.nextNodes == null || currentNode.nextNodes.Count == 0)
            {
                dialogueUI.skipMode = false;
                yield break;
            }

            string nextGuid = currentNode.nextNodes[0];

            if (nodeLookup.TryGetValue(nextGuid, out var nextNode))
            {
                EnterNode(nextNode);

                yield return new WaitUntil(() => !dialogueUI.isTyping && !dialogueUI.isWaitingForContinue);

                yield return new WaitForSeconds(CurrentDialogueDelay());
            }
            else
            {
                Debug.LogError($"跳过失败，下个节点不存在: {nextGuid}");
                break;
            }
        }

        dialogueUI.skipMode = false;
    }

    private float CurrentDialogueDelay()
    {
        return dialogueUI.skipMode ? 0f : 0.01f;
    }
}
