using UnityEngine;
using Reflectis.PLG.Graphs;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

namespace Reflectis.PLG.Dialogs
{
    [AddComponentMenu("Dialogs/Interaction/Talkable")]
    public class Talkable : MonoBehaviour
    {
        [Header("Dialog settings")]
        [SerializeField, Tooltip("Reference dialog system to use for this talking entity.")]
        private DialogSystem dialogSystem = null;
        [SerializeField, Tooltip("Dialog part to start from when a dialog is initiated.")]
        private DialogPart dialog;

        ///////////////////////////////////////////////////////////////////////

        public void ActivateDialog()
        {
            if (dialog)
            {
                // Calls the referenced dialog system to start the actual dialog.
                dialogSystem.StartDialog(dialog);
            }
        }

        /// <summary>
        /// Sets the dialog part to start from when a dialog is initiated. Use this when you want to
        /// change the dialog path followed by a character when spoken to.
        /// </summary>
        public void SetDialog(DialogPart newDialog)
        {
            dialog = newDialog;
        }

        // Called when the script is added to a gameobject, or when the component is reset.
        void Reset()
        {
            DialogSystem dialogSystem;
            GraphBehaviour graphBehaviour;
            bool instantiateDialogSystem = true;
            // Looks for a Gameobject called "DialogSystem", with a DialogSystem and a GraphBehaviour
            // component attached to it. If these requirements are not met, it instantiates a new 
            // gameobject with said components.
            Transform child = this.transform.Find("DialogSystem");
            if (child)
            {
                dialogSystem = child.GetComponent<DialogSystem>();
                graphBehaviour = child.GetComponent<GraphBehaviour>();
                if (!dialogSystem && !graphBehaviour)
                {
                    instantiateDialogSystem = false;
                }
            }

            if (instantiateDialogSystem)
            {
                child = new GameObject("DialogSystem").transform;
                child.SetParent(this.transform, false);
                dialogSystem = child.gameObject.AddComponent<DialogSystem>();
                graphBehaviour = child.gameObject.AddComponent<GraphBehaviour>();

                this.dialogSystem = dialogSystem;
                dialogSystem.SetGraphContainer(graphBehaviour);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Talkable))]
    public class SaveDTOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Talkable saveDto = (Talkable)target;

            if (GUILayout.Button("Activate Dialog"))
                saveDto.ActivateDialog();
        }
    }
#endif
}