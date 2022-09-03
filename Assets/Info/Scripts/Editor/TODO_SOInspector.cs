/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEditor;

using PD.Editor;
using PD.Utils.ResourcesHandler;

[CustomEditor(typeof(TODO_ScriptableObject)), CanEditMultipleObjects]
[InitializeOnLoad]
public sealed class TODO_SOInspector : Editor
{
    private TODO_ScriptableObject root;

    #region Select Automatically
    static TODO_SOInspector()
    {
        EditorApplication.delayCall += SelectTODOAutomatically;
    }
    static void SelectTODOAutomatically()
    {
        var ids = AssetDatabase.FindAssets("TODO t:TODO_ScriptableObject");
        if  (ids.Length == 1)
        {
            var todoObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

            Selection.objects = new Object[]
            {
                todoObject
            };
        }
        else
        {
            Debug.LogWarning("Couldn't find ToDo list.");
        }
    }
    #endregion

    #region Styles
    private GUIStyle InsetFoldoutStyle => new(EditorStyles.foldout)
    {
        margin = new RectOffset(10, 0, 0, 0)
    };
    private GUIStyle StatusStyle => new(EditorStyles.whiteLabel);
    #endregion

    #region StateColors
    private static Color baseColor = new();
    private static Color PendingColor => new(0.75f, 0.75f, 0.75f, 1.0f); //grey
    private static Color DevelopementColor => Color.yellow;
    private static Color TestingColor => new(1.0f, 0.5f, 0.0f, 1.0f); //orange
    private static Color BugFixingColor => Color.red;
    private static Color DoneColor => new(0.0f, 1.0f, 0.5f, 1.0f); //mint - cyan
    private static Color LiveColor => Color.green;
    #endregion

    private const float buttonWidth = 32.0f;
    private const float distanceBetweenBoxes = 15.0f;

    private void OnEnable()
    {
        root = (TODO_ScriptableObject)target;

        baseColor = GUI.color;

        root.resources.gitHubIcon = (Texture2D)CoreResources.FetchObject("GitHub_Icon");
    }

    public override void OnInspectorGUI()
    {
        // Header with Resources
        CoreEditorGUILayout.BeginVertical(() =>
        {
            // Todo title
            GUILayout.Label(root.header.GetToDoTitle, CoreGUIStyles.GetHeader1Style);

            CoreEditorGUILayout.Line();

            // Icon with links
            CoreEditorGUILayout.BeginHorizontal(() =>
            {
                // Icon
                GUILayout.Label(root.resources.gitHubIcon, GUILayout.Width(40.0f), GUILayout.Height(40.0f));

                // Links
                CoreEditorGUILayout.BeginVertical(() =>
                {
                    if (EditorGUILayout.LinkButton(root.resources.GetGitHubOwnerName + " GitHub page"))
                    {
                        Application.OpenURL(root.resources.GetGitHubOwnerLink);
                    }
                    if (EditorGUILayout.LinkButton(root.resources.GetGitHubProjectName + " GitHub page"))
                    {
                        Application.OpenURL(root.resources.GetGitHubProjectLink);
                    }
                });
            });
        }, EditorStyles.helpBox);

        EditorGUILayout.Space(distanceBetweenBoxes);

        // Current status of latest builds
        CoreEditorGUILayout.BeginVertical(() =>
        {
            EditorGUILayout.LabelField("Latest Dev build: " + root.GetLatestVersionName(TODO_ScriptableObject.BuildState.Dev, TODO_ScriptableObject.FeatureState.Pending, true), StatusStyle);
            EditorGUILayout.LabelField("Latest Test build: " + root.GetLatestVersionName(TODO_ScriptableObject.BuildState.Test, TODO_ScriptableObject.FeatureState.Pending, true), StatusStyle);
            EditorGUILayout.LabelField("Latest Live build: " + root.GetLatestVersionName(TODO_ScriptableObject.BuildState.Live, TODO_ScriptableObject.FeatureState.Live), StatusStyle);
        }, EditorStyles.helpBox);

        EditorGUILayout.Space(distanceBetweenBoxes);

        // Builds count controls
        CoreEditorGUILayout.BeginHorizontal(() =>
        {
            EditorGUILayout.LabelField(new GUIContent("Builds"));
            CoreEditorGUILayout.Button(root, "+", "Added new build to list", () => root.builds.Add(new TODO_ScriptableObject.Build()), buttonOptions: GUILayout.Width(buttonWidth));
            CoreEditorGUILayout.Button(root, "-", "Removed last build from list", () => root.builds.RemoveAt(root.builds.Count - 1), root.builds.Count > 0, GUILayout.Width(buttonWidth));
        }, EditorStyles.helpBox);

        CoreEditorGUILayout.Line();

        // Go through all build indexes
        for (int b = 0; b < root.builds.Count; b++)
        {
            // Build label
            EditorGUI.BeginChangeCheck();
            bool showBuild = EditorGUILayout.BeginFoldoutHeaderGroup(root.builds[b].isVisibleInInspector, new GUIContent(root.builds[b].name));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(root, "Toggle visibility for build " + root.GetVersionName(b));
                root.builds[b].isVisibleInInspector = showBuild;
                EditorUtility.SetDirty(root);
            }

            if (root.builds[b].isVisibleInInspector)
            {
                // Build data
                CoreEditorGUILayout.BeginVertical(() =>
                {
                    // Build name and remove button
                    CoreEditorGUILayout.BeginHorizontal(() =>
                    {
                        CoreEditorGUILayout.TextField(root, "Build name", root.builds[b].name, "Renamed " + root.GetVersionName(b), (string newTextField) => root.builds[b].name = newTextField);
                        CoreEditorGUILayout.Button(root, "X", "Remove build " + root.GetVersionName(b), () => root.builds.RemoveAt(b), buttonOptions: GUILayout.Width(buttonWidth));
                    });

                    // Bug fix: when removing the last index of the list and on it points to an out of range index
                    if (b >= root.builds.Count)
                        return;

                    // Features count controls
                    CoreEditorGUILayout.BeginHorizontal(() =>
                    {
                        EditorGUILayout.LabelField(new GUIContent("Features"));
                        CoreEditorGUILayout.Button(root, "+", "Added new feature to list", () => root.builds[b].changeLog.Add(new TODO_ScriptableObject.FeatureProperties()), buttonOptions: GUILayout.Width(buttonWidth));
                        CoreEditorGUILayout.Button(root, "-", "Removed last feature from list", () => root.builds[b].changeLog.RemoveAt(root.builds[b].changeLog.Count - 1), root.builds[b].changeLog.Count > 0, GUILayout.Width(buttonWidth));
                    });

                    CoreEditorGUILayout.Line();

                    // Go through all features of current build index
                    for (int f = 0; f < root.builds[b].changeLog.Count; f++)
                    {
                        ChangeStateColor(b, f);

                        // Feature box
                        CoreEditorGUILayout.BeginVertical(() =>
                        {
                            // Feature label
                            EditorGUI.BeginChangeCheck();
                            bool showFeature = EditorGUILayout.Foldout(root.builds[b].changeLog[f].isVisibleInInspector, new GUIContent(root.builds[b].changeLog[f].name), true, InsetFoldoutStyle);
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(root, "Toggle visibility for feature " + root.GetVersionName(b, f));
                                root.builds[b].changeLog[f].isVisibleInInspector = showFeature;
                                EditorUtility.SetDirty(root);
                            }

                            // Feature data
                            if (root.builds[b].changeLog[f].isVisibleInInspector)
                            {
                                // Feature name and remove button
                                CoreEditorGUILayout.BeginHorizontal(() =>
                                {
                                    CoreEditorGUILayout.TextField(root, "Feature name", root.builds[b].changeLog[f].name, "Renamed " + root.GetVersionName(b, f), (string newTextField) => root.builds[b].changeLog[f].name = newTextField);
                                    CoreEditorGUILayout.Button(root, "X", "Removed feature " + root.GetVersionName(b, f), () => root.builds[b].changeLog.RemoveAt(f), buttonOptions: GUILayout.Width(buttonWidth));
                                });

                                // Bug fix: when removing the last index of the list and on it points to an out of range index
                                if (f >= root.builds[b].changeLog.Count)
                                    return;

                                // Feature description
                                CoreEditorGUILayout.TextArea(root, "Feature description", root.builds[b].changeLog[f].desc, "Changed description for " + root.GetVersionName(b, f), (string newTextField) => root.builds[b].changeLog[f].desc = newTextField, textAreaOptions: GUILayout.Height(90.0f) );

                                // Feature state control
                                EditorGUI.BeginChangeCheck();
                                TODO_ScriptableObject.FeatureState newFeatureState = (TODO_ScriptableObject.FeatureState)EditorGUILayout.EnumPopup("Feature state", root.builds[b].changeLog[f].state);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(root, "Changed state of feature " + root.GetVersionName(b, f));
                                    root.builds[b].changeLog[f].state = newFeatureState;
                                    EditorUtility.SetDirty(root);
                                }

                                CoreEditorGUILayout.Line();
                            }
                        }, EditorStyles.helpBox);
                        GUI.color = baseColor;
                    }

                    // Build state control
                    EditorGUI.BeginChangeCheck();
                    TODO_ScriptableObject.BuildState newBuildState = (TODO_ScriptableObject.BuildState)EditorGUILayout.EnumPopup("Build state", root.builds[b].state);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(root, "Changed state of build " + root.GetVersionName(b));
                        root.builds[b].state = newBuildState;
                        EditorUtility.SetDirty(root);
                    }
                }, EditorStyles.helpBox);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            CoreEditorGUILayout.Line();
        }
    }

    private void ChangeStateColor(int b, int f)
    {
        switch (root.builds[b].changeLog[f].state)
                    {
                        case TODO_ScriptableObject.FeatureState.Pending:
                            GUI.color = PendingColor;
                            break;
                        case TODO_ScriptableObject.FeatureState.InDevelopment:
                            GUI.color = DevelopementColor;
                            break;
                        case TODO_ScriptableObject.FeatureState.Testing:
                            GUI.color = TestingColor;
                            break;
                        case TODO_ScriptableObject.FeatureState.BugFixing:
                            GUI.color = BugFixingColor;
                            break;
                        case TODO_ScriptableObject.FeatureState.Done:
                            GUI.color = DoneColor;
                            break;
                        case TODO_ScriptableObject.FeatureState.Live:
                            GUI.color = LiveColor;
                            break;
                    }
    }
}
