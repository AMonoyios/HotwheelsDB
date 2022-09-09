/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SW.Editor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class DeveloperEditorWindow : BaseDeveloperEditorWindow
{
    //FIXME: Tranfer scene managment to BaseDeveloperEditorWindow

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

    new private void OnGUI()
    {
        base.OnGUI();

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
