using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManage : MonoBehaviour
{
    public static DialogueManage _instance;

    [SerializeField] private Text speakerNameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private RectTransform trans_log;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;
    public float autoPlayDelay = 1.5f;
    public float typewriterSpeed = 0.03f; // 每个字的显示间隔

    private Coroutine typingCoroutine;
    private Coroutine autoCoroutine;
    private Action onDialogueContinue;

    private string fullText = "";
    public bool isTyping = false;

    public bool isAutoPlaying = false;
    public bool isWaitingForContinue = false;
    [SerializeField] private Transform choiceButtonContainer; // 用于放置所有按钮的父物体
    public Image img_BG;
    [Tooltip("NPC加载节点组")] public RectTransform[] npcLoadGroup;
    [SerializeField][Tooltip("SKIP")] private Button btn_Skip;
    [SerializeField][Tooltip("AUDO")] private Button btn_Audo;
    [SerializeField][Tooltip("LOG")] private Button btn_Log;
    private List<GameObject> currentChoiceButtons = new List<GameObject>();
    public bool skipMode = false;
    public bool isOtner = false;
    public bool isActive = false;
    private float normalTypingSpeed = 0.03f;
    private float fastTypingSpeed = 0f;

    private float normalAutoDelay = 1.5f;
    private float fastAutoDelay = 0f;

    private float CurrentTypingSpeed => skipMode ? fastTypingSpeed : normalTypingSpeed;
    private float CurrentAutoDelay => skipMode ? fastAutoDelay : normalAutoDelay;
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        btn_Skip.onClick.AddListener(OnClickSkip);
        btn_Audo.onClick.AddListener(OnClickAudo);
        btn_Log.onClick.AddListener(OnClickLog);

    }

    #region 按钮事件
    void OnClickSkip()
    {
        DialogueContral.instance.StartSkipMode(); // 调用跳过模式
    }
    void OnClickAudo()
    {
        if (isAutoPlaying)
        {   
            
            StopAutoPlay(); // 当前是自动播放，点击后关闭
        }
        else
        {
            StartAutoPlay(); // 当前不是自动播放，点击后开启
        }
    }
    void OnClickLog()
    {
        isOtner = true;
        trans_log.gameObject.SetActive(true);
    }
    #endregion
    public void ShowDialogue(string speaker, string text, Action onContinue)
    {
        string speakerLanage = text;
        speakerNameText.text = speakerLanage;
        fullText = text; //这个地方需要转换多语言
        dialogueText.text = "";
        onDialogueContinue = onContinue;

        isWaitingForContinue = true;
        gameObject.SetActive(true);

        StartTypewriterEffect(fullText);
    }

    private void StartTypewriterEffect(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        // 跳过模式：直接显示文本，无需打字机动画
        if (skipMode)
        {
            dialogueText.text = text;
            yield return new WaitForSeconds(0.05f); // 控制跳过节奏（太快人看不清）
        }
        else
        {
            dialogueText.text = "";

            foreach (char c in text)
            {
                dialogueText.text += c;

                // 播放音效
                if (typeSound != null && audioSource != null && !char.IsWhiteSpace(c))
                {
                    audioSource.PlayOneShot(typeSound);
                }

                yield return new WaitForSeconds(CurrentTypingSpeed);
            }
        }

        isTyping = false;

        if (isAutoPlaying || skipMode)
        {
            if (autoCoroutine != null)
                StopCoroutine(autoCoroutine);
            autoCoroutine = StartCoroutine(AutoContinueAfterDelay());
        }
    }

    private IEnumerator AutoContinueAfterDelay()
    {
        yield return new WaitForSeconds(CurrentAutoDelay);
        ContinueDialogue();
    }

    void Update()
    {
        if (!isWaitingForContinue)
            return;

        if (Input.anyKeyDown && isOtner == false)
        {
            HandlePlayerInput();
        }
    }

    public void OnDialogueClicked()
    {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        if (isTyping)
        {
            // 打字中：立刻显示全部文字
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullText;
            isTyping = false;

            if (isAutoPlaying)
                autoCoroutine = StartCoroutine(AutoContinueAfterDelay());
        }
        else
        {
            // 已经打完：继续
            StopAutoPlay();
            ContinueDialogue();
        }
    }

    private void ContinueDialogue()
    {
        isWaitingForContinue = false;
        onDialogueContinue?.Invoke();
    }

    public void ShowChoices(List<string> choices, Action<int> onChoiceSelected)
    {
        // 遇到选项，必须停下跳过


        ClearChoices();
        speakerNameText.text = "";
        dialogueText.text = "";
        isWaitingForContinue = false;

        for (int i = 0; i < choices.Count; i++)
        {
            int index = i;
            var buttonGO = LoadAndInstantiatePrefab("btn_Choise", choiceButtonContainer);
            currentChoiceButtons.Add(buttonGO);

            var button = buttonGO.GetComponent<Button>();
            var text = buttonGO.GetComponentInChildren<Text>();
            if (text == null)
            {
                Debug.LogError("按钮预制体中没有找到 Text 组件！");
            }
            else
            {
                text.text = choices[i];
            }

            button.onClick.AddListener(() =>
            {
                ClearChoices();
                onChoiceSelected?.Invoke(index);
            });
        }
    }
    private void ClearChoices()
    {
        foreach (var btn in currentChoiceButtons)
        {
            Destroy(btn);
        }
        currentChoiceButtons.Clear();
    }
    public void StartAutoPlay()
    {
        isAutoPlaying = true;
    }

    public void StopAutoPlay()
    {
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }

        isAutoPlaying = false;
    }

    public void EndDialogue()
    {
        isActive = false;
        StopAutoPlay();
        DialogueContral.instance.trans_Panel.gameObject.SetActive(false);
    }

    #region 预制体加载
    public static GameObject LoadAndInstantiatePrefab(string prefabName, Transform parent)
    {
        string path = "prefab/" + prefabName;
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError($"未找到预制体: Resources/{path}");
            return null;
        }

        GameObject instance = Instantiate(prefab, parent);
        instance.name = prefab.name; // 去除 "(Clone)" 后缀（可选）
        return instance;
    }
    #endregion
    #region UI工具方法
    [Tooltip("UI父节点")] public RectTransform trans_Panel;
    
    //UI隐藏
    public void Hide()
    {
        trans_Panel.gameObject.SetActive(false);
    }
    //UI显示
    public void NoHide()
    {
        trans_Panel.gameObject.SetActive(true);
    }
    #endregion
}
