using Reflectis.PLG.Dialogs;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Reflectis.PLG.DialogsEditor
{
    [CustomEditor(typeof(DialogPanelController))]
    public class DialogPanelControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DialogPanelController controller = (DialogPanelController)target;

            //PlayerPanel
            EditorGUILayout.LabelField("Player Panel", EditorStyles.boldLabel);
            DrawDialogPanel(controller.PlayerPanel, "playerPanel");

            EditorGUILayout.Space(10);

            //NpcPanel
            EditorGUILayout.LabelField("NPC Panel", EditorStyles.boldLabel);
            DrawDialogPanel(controller.NpcPanel, "npcPanel");

            EditorGUILayout.Space(10);

            //Typewrite
            EditorGUILayout.LabelField("Typewrite Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersPerSecond"), new GUIContent("Characters Per Second"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("interpunctuationDelay"), new GUIContent("Interpunctuation Delay"));

            EditorGUILayout.Space();

            //Skip
            EditorGUILayout.LabelField("Skip Settings", EditorStyles.boldLabel);
            SerializedProperty enableSkip = serializedObject.FindProperty("enableSkip");
            EditorGUILayout.PropertyField(enableSkip, new GUIContent("Enable Skip"));
            if (enableSkip.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("quickSkip"), new GUIContent("Quick Skip"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skipSpeedup"), new GUIContent("Quick Speedup"));
            }


            //Save changes
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(controller);
                serializedObject.ApplyModifiedProperties();
                PrefabUtility.RecordPrefabInstancePropertyModifications(controller);
            }
        }

        private void DrawDialogPanel(DialogPanelController.DialogPanel dialogPanel, string variableName)
        {
            if (dialogPanel == null)
                return;

            dialogPanel.panelObject = (GameObject)EditorGUILayout.ObjectField("Panel Object", dialogPanel.panelObject, typeof(GameObject), true);

            dialogPanel.nicknameBg = (GameObject)EditorGUILayout.ObjectField("Nickname Bg", dialogPanel.nicknameBg, typeof(GameObject), true);
            SerializedProperty showNicknameText = serializedObject.FindProperty($"{variableName}.showNickname");
            EditorGUILayout.PropertyField(showNicknameText);
            if (showNicknameText.boolValue)
                dialogPanel.nicknameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Nickname Text", dialogPanel.nicknameText, typeof(TextMeshProUGUI), true);

            dialogPanel.dialogText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Dialog Text", dialogPanel.dialogText, typeof(TextMeshProUGUI), true);

            SerializedProperty showAvatarContainer = serializedObject.FindProperty($"{variableName}.showAvatarContainer");
            EditorGUILayout.PropertyField(showAvatarContainer);
            if (showAvatarContainer.boolValue)
                dialogPanel.avatarContainer = (Image)EditorGUILayout.ObjectField("Avatar Container", dialogPanel.avatarContainer, typeof(Image), true);

            SerializedProperty choiceButtonGroups = serializedObject.FindProperty($"{variableName}.choiceButtonGroups");
            EditorGUILayout.PropertyField(choiceButtonGroups, true);
        }
    }
}
