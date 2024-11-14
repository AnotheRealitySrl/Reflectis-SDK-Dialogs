using UnityEngine;
using UnityEngine.Events;

using Reflectis.PLG.Graphs;
using static Reflectis.PLG.Dialogs.DialogNode;

namespace Reflectis.PLG.Dialogs
{

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Scene component that contains a dialog element and handles its events in 
    /// the Graph System.
    /// </summary>
    public class DialogPart : NodeBehaviour<DialogNode>
    {
        [Header("Events")]
        [SerializeField, Tooltip("Invoked when the dialog is reset (status ToDo)")]
        protected UnityEvent onDialogReset = default;

        [SerializeField, Tooltip("Invoked when the dialog is in progress (status InProgress)")]
        protected UnityEvent onDialogInProgress = default;

        [SerializeField, Tooltip("Invoked when the dialog is completed (status Completed)")]
        protected UnityEvent onDialogCompleted = default;


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Invoked when the dialog is reset (status ToDo)</summary>
        public UnityEvent OnDialogReset => onDialogReset;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Invoked when the dialog is in progress (status InProgress)</summary>
        public UnityEvent OnDialogInProgress => onDialogInProgress;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Invoked when the dialog is completed (status Completed)</summary>
        public UnityEvent OnDialogCompleted => onDialogCompleted;


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Turns the dialog status to ToDo</summary>
        public void UnlockDialog() => Node.Status = DialogStatus.Todo;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Turns the dialog status to Completed</summary>
        public void CompleteDialog()
        {
            if (Node.Status == DialogStatus.Todo)
                Node.Status = DialogStatus.Completed;
        }


        ///////////////////////////////////////////////////////////////////////////
        protected virtual void Awake()
        {
            //OnStatusChanged(oldStatus: DialogStatus.Locked);
            Node.onStatusChanged.AddListener(OnStatusChanged);
        }

        ///////////////////////////////////////////////////////////////////////////
        protected virtual void OnStatusChanged(DialogStatus oldStatus)
        {
            if (Node.Status == DialogStatus.Todo)
                onDialogReset.Invoke();
            else if (Node.Status == DialogStatus.InProgress)
                onDialogInProgress.Invoke();
            else if (Node.Status == DialogStatus.Completed)
                onDialogCompleted.Invoke();
        }
    }
}