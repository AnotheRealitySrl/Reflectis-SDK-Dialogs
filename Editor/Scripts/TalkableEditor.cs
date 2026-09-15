using SPACS.Dialogs;
using UnityEditor;
using UnityEngine;

namespace SPACS.DialogsEditor
{
    [CustomEditor(typeof(Talkable))]
    public class TalkableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Talkable talkable = (Talkable)target;

            if (GUILayout.Button("Activate Dialog"))
                talkable.ActivateDialog();
        }
    }
}
