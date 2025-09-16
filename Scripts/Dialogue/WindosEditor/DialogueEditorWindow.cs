#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    private DialogueGraphView graphView;

    [MenuItem("Tools/自定义编辑器")]
    public static void Open()
    {
        var window = GetWindow<DialogueEditorWindow>();
        window.titleContent = new GUIContent("对话树");
        window.Show();
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        if (graphView != null && rootVisualElement.Contains(graphView))
        {
            rootVisualElement.Remove(graphView);
        }
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }
    private string fileName = "NewDialogueData";
    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var createButton = new Button(() => graphView.CreateTextNode("New Say Node", new Vector2(100, 100)))
        {
            text = "Create Say Node"
        };
        toolbar.Add(createButton);

        // 保存按钮
        var saveButton = new Button(() =>
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "保存对话图",
                "DialogueGraphData.asset",
                "asset",
                "选择一个位置保存对话图数据"
            );

            if (!string.IsNullOrEmpty(path))
            {
                var graphData = AssetDatabase.LoadAssetAtPath<DialogueGraphData>(path);
                if (graphData == null)
                {
                    graphData = ScriptableObject.CreateInstance<DialogueGraphData>();
                    AssetDatabase.CreateAsset(graphData, path);
                }

                graphView.SaveGraph(graphData);
            }
        })
        {
            text = "保存对话树"
        };
        toolbar.Add(saveButton);

        // 加载按钮
        var loadButton = new Button(() =>
        {
            string path = EditorUtility.OpenFilePanel("加载对话图", "Assets", "asset");

            if (!string.IsNullOrEmpty(path))
            {
                // 转换为相对路径
                string relativePath = "Assets" + path.Substring(Application.dataPath.Length);

                var graphData = AssetDatabase.LoadAssetAtPath<DialogueGraphData>(relativePath);
                if (graphData != null)
                {
                    graphView.LoadGraph(graphData);
                }
                else
                {
                    Debug.LogError("选中的文件不是有效的 DialogueGraphData 资源！");
                }
            }
        })
        {
            text = "加载对话树"
        };
        toolbar.Add(loadButton);

        rootVisualElement.Add(toolbar);
    }
    

    
}
#endif
