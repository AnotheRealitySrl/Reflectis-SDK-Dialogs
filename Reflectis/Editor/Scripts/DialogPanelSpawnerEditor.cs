using Reflectis.PLG.DialogsDialogsReflectis;
using UnityEditor;
using UnityEngine;

namespace Reflectis.PLG.DialogsReflectisEditor
{
    [CustomEditor(typeof(DialogPanelSpawner))]
    public class DialogPanelSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            var spawner = (DialogPanelSpawner)target;

            EditorGUILayout.LabelField("Player panel settings", EditorStyles.boldLabel);
            SerializedProperty showPlayerNickname = serializedObject.FindProperty("showPlayerNickname");
            EditorGUILayout.PropertyField(showPlayerNickname, new GUIContent("Show player nickname"));
            if (showPlayerNickname.boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useReflectisNickname"), new GUIContent("Use Reflectis nickname"));

            SerializedProperty showPlayerAvatarContainer = serializedObject.FindProperty("showPlayerAvatarContainer");
            EditorGUILayout.PropertyField(showPlayerAvatarContainer, new GUIContent("Show player avatar"));
            if (showPlayerAvatarContainer.boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useReflectisAvatar"), new GUIContent("Use Reflectis avatar"));

            EditorGUILayout.LabelField("Npc panel settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showNpcNickname"), new GUIContent("Show npc nickname"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showNpcAvatarContainer"), new GUIContent("Show npc avatar"));

            EditorGUILayout.LabelField("Typewrite effect settings", EditorStyles.boldLabel);
            spawner.charactersPerSecond = Mathf.Max(0, EditorGUILayout.FloatField("Characters per second", spawner.charactersPerSecond));
            if (spawner.charactersPerSecond > 0 )
                spawner.interpunctuationDelay = Mathf.Max(0, EditorGUILayout.FloatField("Interpunctuation delay", spawner.interpunctuationDelay));

            EditorGUILayout.LabelField("Skip Settings", EditorStyles.boldLabel);
            SerializedProperty enableSkip = serializedObject.FindProperty("enableSkip");
            EditorGUILayout.PropertyField(enableSkip, new GUIContent("Enable Skip"));
            if (enableSkip.boolValue)
            {
                SerializedProperty quickSkip = serializedObject.FindProperty("quickSkip");
                EditorGUILayout.PropertyField(quickSkip, new GUIContent("Quick skip"));
                if (!quickSkip.boolValue)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("skipSpeedup"), new GUIContent("Skip speedup"));
            }

            //Save changes
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(spawner);
                serializedObject.ApplyModifiedProperties();
                PrefabUtility.RecordPrefabInstancePropertyModifications(spawner);
            }
        }
    }
}
