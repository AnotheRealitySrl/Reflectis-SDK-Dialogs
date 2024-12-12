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

            // Ottieni il target
            DialogPanelController controller = (DialogPanelController)target;

            // Sezioni personalizzate per playerPanel
            EditorGUILayout.LabelField("Player Panel", EditorStyles.boldLabel);
            DrawDialogPanel(controller.PlayerPanel, "playerPanel");

            EditorGUILayout.Space(5);

            // Sezioni personalizzate per npcPanel
            EditorGUILayout.LabelField("NPC Panel", EditorStyles.boldLabel);
            DrawDialogPanel(controller.NpcPanel, "npcPanel");

            // Salva i cambiamenti
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
            EditorGUILayout.Space();

            dialogPanel.nicknameBg = (GameObject)EditorGUILayout.ObjectField("Nickname Bg", dialogPanel.nicknameBg, typeof(GameObject), true);
            SerializedProperty showNicknameText = serializedObject.FindProperty($"{variableName}.showNickname");
            EditorGUILayout.PropertyField(showNicknameText);
            if (showNicknameText.boolValue)
                dialogPanel.nicknameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Nickname Text", dialogPanel.nicknameText, typeof(TextMeshProUGUI), true);
            EditorGUILayout.Space();

            dialogPanel.dialogText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Dialog Text", dialogPanel.dialogText, typeof(TextMeshProUGUI), true);
            EditorGUILayout.Space();

            SerializedProperty showAvatarContainer = serializedObject.FindProperty($"{variableName}.showAvatarContainer");
            EditorGUILayout.PropertyField(showAvatarContainer);
            if (showAvatarContainer.boolValue)
                dialogPanel.avatarContainer = (Image)EditorGUILayout.ObjectField("Avatar Container", dialogPanel.avatarContainer, typeof(Image), true);
            EditorGUILayout.Space();

            SerializedProperty choiceButtonGroups = serializedObject.FindProperty($"{variableName}.choiceButtonGroups");
            EditorGUILayout.PropertyField(choiceButtonGroups, true);
        }
    }
}
