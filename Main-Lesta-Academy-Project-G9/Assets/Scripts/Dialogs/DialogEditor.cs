#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DialogAsset))]
public class DialogEditor : Editor
{
    class DAEF
    {
        public DialogAsset.ArrayGroup group;
        public bool show = false;
    }

    class CGEF
    {
        public DialogAsset.ArrayCharacters group;
        public bool show = false;
    }
    List<DAEF> groups = new List<DAEF>();
    private List<CGEF> chgroups = new List<CGEF>();
    GUIStyle buttonStyle;
    DialogAsset asset
    {
        get { return (DialogAsset)target; }
    }

    void OnEnable()
    {
        var treeViewState = new TreeViewState();
        var jsonState = SessionState.GetString("DEA" + asset.GetInstanceID(), "");
        if (!string.IsNullOrEmpty(jsonState))
            JsonUtility.FromJsonOverwrite(jsonState, treeViewState);
        groups = new List<DAEF>();
        chgroups = new List<CGEF>();
        foreach (DialogAsset.ArrayGroup da in asset.arrayGroups)
            groups.Add(new DAEF() { group = da });
        foreach (DialogAsset.ArrayCharacters da in asset.arrayCharacters)
            chgroups.Add(new CGEF() { group = da });
        buttonStyle = new GUIStyle();
        buttonStyle.alignment = TextAnchor.MiddleLeft;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(5f);

        ToolBar();
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        for (int i = 0; i < chgroups.Count; i++)
        {
            CGEF gr = chgroups[i];
            if (gr.show)
            {

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button($"#CHARACTER_{i} {gr.group.characterName}"))
                        gr.show = false;
                    if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                        chgroups.Remove(chgroups[i]);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Character");
                    gr.group.characterName = EditorGUILayout.TextField(gr.group.characterName);
                }
                gr.group.position = (DialogAsset.ArrayCharacters.CharacterPosition)EditorGUILayout.EnumPopup("Position", gr.group.position);
                gr.group.sprite = (Sprite)EditorGUILayout.ObjectField("Image", gr.group.sprite, typeof(Sprite), false);
            }
            else
            {
                string txt = "";
                txt = $"#CHARACTER_{i} {gr.group.characterName}";
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(txt))
                        gr.show = true;
                    if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                        chgroups.Remove(chgroups[i]);
                }
            }
        }
        GUILayout.Space(20f);
        for (int i = 0; i < groups.Count; i++)
        {
            DAEF gr = groups[i];
            if (gr.show)
            {

                switch (gr.group.groupType)
                {
                    case DialogAsset.ArrayGroup.GroupType.DIALOG:
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button($"#{i} {gr.group.characterName}"))
                                gr.show = false;
                            if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                                groups.Remove(groups[i]);
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Character"); gr.group.characterName = EditorGUILayout.TextField(gr.group.characterName);
                            }
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Event"); gr.group.eventName = EditorGUILayout.TextField(gr.group.eventName);

                            }

                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Min Time"); gr.group.minDialogTime = float.Parse(EditorGUILayout.TextField(gr.group.minDialogTime.ToString()));

                            }
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Force Dialog"); gr.group.force = int.Parse(EditorGUILayout.TextField(gr.group.force.ToString()));
                            }
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            gr.group.hideName = EditorGUILayout.Toggle("Hide Name", gr.group.hideName);

                        }

                        gr.group.clip = (AudioClip)EditorGUILayout.ObjectField("Dialog Clip", gr.group.clip, typeof(AudioClip));

                        EditorGUILayout.LabelField("Dialog"); gr.group.dialog = EditorGUILayout.TextArea(gr.group.dialog, GUILayout.Height(100), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                        break;
                    case DialogAsset.ArrayGroup.GroupType.EVENT:
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button($"#{i} {gr.group.eventName}"))
                                gr.show = false;
                            if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                                groups.Remove(groups[i]);
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Event"); gr.group.eventName = EditorGUILayout.TextField(gr.group.eventName);
                            }
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField("Min Time"); gr.group.minDialogTime = float.Parse(EditorGUILayout.TextField(gr.group.minDialogTime.ToString()));

                            }
                        }
                        break;
                    case DialogAsset.ArrayGroup.GroupType.ANSWER:
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button($"#{i} ANSWERS"))
                                gr.show = false;
                            if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                                groups.Remove(groups[i]);
                        }
                        if (GUILayout.Button("Add", "miniButton", GUILayout.Width(60)))
                        {
                            DialogAsset.ArrayGroup.AnswerGroup[] answers = gr.group.answers;
                            gr.group.answers = new DialogAsset.ArrayGroup.AnswerGroup[answers.Length + 1];
                            for (int j = 0; j < answers.Length; j++)
                                gr.group.answers[j] = answers[j];
                        }
                        for (int j = 0; j < gr.group.answers.Length; j++)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                using (new EditorGUILayout.VerticalScope())
                                {
                                    EditorGUILayout.LabelField("Message"); gr.group.answers[j].msg = EditorGUILayout.TextField(gr.group.answers[j].msg);

                                }
                                using (new EditorGUILayout.VerticalScope())
                                {
                                    EditorGUILayout.LabelField("Force Dialog"); gr.group.answers[j].force = int.Parse(EditorGUILayout.TextField(gr.group.answers[j].force.ToString()));
                                }
                            }
                            if (GUILayout.Button("Remove"))
                            {
                                DialogAsset.ArrayGroup.AnswerGroup[] answers = gr.group.answers;
                                gr.group.answers = new DialogAsset.ArrayGroup.AnswerGroup[answers.Length - 1];
                                for (int k = 0; k < answers.Length - 1; k++)
                                {
                                    gr.group.answers[k] = answers[k < j ? k : k + 1];
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                string txt = "";
                switch (gr.group.groupType)
                {

                    case DialogAsset.ArrayGroup.GroupType.DIALOG:
                        txt = $"#{i} {gr.group.characterName}";
                        break;
                    case DialogAsset.ArrayGroup.GroupType.EVENT:
                        txt = $"#{i} {gr.group.eventName}";
                        break;
                    case DialogAsset.ArrayGroup.GroupType.ANSWER:
                        txt = $"#{i} ANSWERS";
                        break;

                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(txt))
                        gr.show = true;
                    if (GUILayout.Button("Remove", "miniButton", GUILayout.Width(60)))
                        groups.Remove(groups[i]);
                }
            }
        }
        GUILayout.Space(3f);

        /*const float topToolbarHeight = 20f;
        const float spacing = 2f;
        float totalHeight = 600f;
        Rect rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
        Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
        Rect multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);*/
    }



    void ToolBar()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            var style = "miniButton";

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save", style))
            {
                DialogAsset.ArrayGroup[] ag = new DialogAsset.ArrayGroup[groups.Count];
                DialogAsset.ArrayCharacters[] ac = new DialogAsset.ArrayCharacters[chgroups.Count];
                for (int i = 0; i < ag.Length; i++)
                {
                    ag[i] = groups[i].group;
                }
                for (int i = 0; i < ac.Length; i++)
                {
                    ac[i] = chgroups[i].group;
                }
                asset.arrayGroups = ag;
                asset.arrayCharacters = ac;
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssetIfDirty(asset);
            }
            if (GUILayout.Button("Add Character", style))
            {
                chgroups.Add(new CGEF() { group = new DialogAsset.ArrayCharacters() });
            }
            if (GUILayout.Button("Add Dialogue", style))
            {
                groups.Add(new DAEF() { group = new DialogAsset.ArrayGroup() });
            }

            if (GUILayout.Button("Add Event", style))
            {
                groups.Add(new DAEF() { group = new DialogAsset.ArrayGroup() { groupType = DialogAsset.ArrayGroup.GroupType.EVENT } });
            }
            if (GUILayout.Button("Add Answers", style))
            {
                groups.Add(new DAEF() { group = new DialogAsset.ArrayGroup() { groupType = DialogAsset.ArrayGroup.GroupType.ANSWER } });
            }
        }
    }
}

#endif