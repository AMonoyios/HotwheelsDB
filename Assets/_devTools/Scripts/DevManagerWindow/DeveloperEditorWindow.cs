using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SW.Editor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class DeveloperEditorWindow : EditorWindow
{
    private int selectedTab = 0;
    private class SceneInfo
    {
        public SceneInfo(int index, string name, SceneAsset scene)
        {
            GetIndex = index;
            GetName = name;
            GetScene = scene;
        }

        public string GetName { get; }
        public int GetIndex { get; }
        public SceneAsset GetScene { get; }
    }
    private readonly Dictionary<int, SceneInfo> scenes = new();

    [MenuItem("Window/SW Tools/Developer Window")]
    public static void ShowWindow()
    {
        GetWindow<DeveloperEditorWindow>();
    }

    private void OnEnable()
    {
        Debug.Log("Initializing developer editor window...");

        scenes.Clear();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            scenes.Add(i, new(i, SceneManager.GetSceneByBuildIndex(i).name, AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneManager.GetSceneByBuildIndex(i).path)));
            Debug.Log($"Added scene {i} to dictionary.");
        }
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, new string[] {"General", "Scenes"});

        switch (selectedTab)
        {
            case 0:
            {
                CoreEditorGUILayout.Label("This is the Generals tab");
            }
            break;
            case 1:
            {
                ShowScenesTab();
            }
            break;
            default:
            {
                Debug.LogError($"Developer editor window has selected tab index out of bounds. Index:{selectedTab}");
            }
            break;
        }
    }

    private void ShowScenesTab()
    {
        EditorGUILayout.Space(10.0f);
        CoreEditorGUILayout.Label("Scene selection");
        CoreEditorGUILayout.Line();

        for (int i = 0; i < scenes.Count; i++)
        {
            CoreEditorGUILayout.BeginHorizontal(() =>
            {
                CoreEditorGUILayout.Label(scenes[i].GetName);
                CoreEditorGUILayout.Button("Open",
                                        () =>
                                        {
                                            if (SceneManager.GetActiveScene().isDirty)
                                            {
                                                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                                                EditorSceneManager.LoadScene(i, LoadSceneMode.Single);
                                                EditorSceneManager.OpenScene(scenes[i].GetName, OpenSceneMode.Single);
                                            }
                                        });
                CoreEditorGUILayout.Button("Open & Run",
                                        () =>
                                        {
                                            if (SceneManager.GetActiveScene().isDirty)
                                            {
                                                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                                                SceneManager.LoadScene(i, LoadSceneMode.Single);
                                                EditorApplication.EnterPlaymode();
                                            }
                                        });
            });
        }

        CoreEditorGUILayout.Button("Repaint", () => { Repaint(); OnEnable(); });
    }
}
