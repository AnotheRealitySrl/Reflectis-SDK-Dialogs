using Reflectis.SDK.Dialogs;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Reflectis.SDK.DialogsEditor
{
    [CustomEditor(typeof(DialogPanelController))]
    public class DialogPanelControllerEditor : Editor
    {
        DialogPanelController controller;
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            controller = (DialogPanelController)target;

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
            {
                dialogPanel.nicknameText = (TMP_Text)EditorGUILayout.ObjectField("Nickname Text", dialogPanel.nicknameText, typeof(TMP_Text), true);
                if (variableName == "playerPanel")
                {
                    controller.NicknameBgPosXLeftPlayer = (float)EditorGUILayout.FloatField("Nickname Pos X Left", controller.NicknameBgPosXLeftPlayer);
                    controller.NicknameBgPosXRightPlayer = (float)EditorGUILayout.FloatField("Nickname Pos X Right", controller.NicknameBgPosXRightPlayer);
                }
                else
                {
                    controller.NicknameBgPosXLeftNpc = (float)EditorGUILayout.FloatField("Nickname Pos X Left", controller.NicknameBgPosXLeftNpc);
                    controller.NicknameBgPosXRightNpc = (float)EditorGUILayout.FloatField("Nickname Pos X Right", controller.NicknameBgPosXRightNpc);
                }
            }

            dialogPanel.dialogText = (TMP_Text)EditorGUILayout.ObjectField("Dialog Text", dialogPanel.dialogText, typeof(TMP_Text), true);

            SerializedProperty showAvatarContainer = serializedObject.FindProperty($"{variableName}.showAvatarContainer");
            EditorGUILayout.PropertyField(showAvatarContainer);
            if (showAvatarContainer.boolValue)
            {
                if (variableName == "playerPanel")
                {
                    controller.AvatarContainerPlayer = (Image)EditorGUILayout.ObjectField("Avatar Container", controller.AvatarContainerPlayer, typeof(Image), true);
                    controller.AvatarContainerPosXLeftPlayer = (float)EditorGUILayout.FloatField("Avatar Container Pos X Left", controller.AvatarContainerPosXLeftPlayer);
                    controller.AvatarContainerPosXRightPlayer = (float)EditorGUILayout.FloatField("Avatar Container Pos X Right", controller.AvatarContainerPosXRightPlayer);
                    controller.DialogBoxPosXLeftPlayer = (float)EditorGUILayout.FloatField("Dialog Box Pos X Left", controller.DialogBoxPosXLeftPlayer);
                    controller.DialogBoxPosXRightPlayer = (float)EditorGUILayout.FloatField("Dialog Box Pos X Right", controller.DialogBoxPosXRightPlayer);
                }
                else
                {
                    controller.AvatarContainerNpc = (Image)EditorGUILayout.ObjectField("Avatar Container", controller.AvatarContainerNpc, typeof(Image), true);
                    controller.AvatarContainerPosXLeftNpc = (float)EditorGUILayout.FloatField("Avatar Container Pos X Left", controller.AvatarContainerPosXLeftNpc);
                    controller.AvatarContainerPosXRightNpc = (float)EditorGUILayout.FloatField("Avatar Container Pos X Right", controller.AvatarContainerPosXRightNpc);
                    controller.DialogBoxPosXLeftNpc = (float)EditorGUILayout.FloatField("Dialog Box Pos X Left", controller.DialogBoxPosXLeftNpc);
                    controller.DialogBoxPosXRightNpc = (float)EditorGUILayout.FloatField("Dialog Box Pos X Right", controller.DialogBoxPosXRightNpc);
                }
            }

            SerializedProperty choiceButtonGroups = serializedObject.FindProperty($"{variableName}.choiceButtonGroups");
            EditorGUILayout.PropertyField(choiceButtonGroups, true);
        }
    }
}
