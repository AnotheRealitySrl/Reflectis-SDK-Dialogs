﻿using Reflectis.PLG.Graphs;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using static Reflectis.PLG.Dialogs.DialogNode;

namespace Reflectis.PLG.Dialogs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manager class that controls the dialog flow
    /// </summary>
    public class DialogSystem : MonoBehaviour
    {
        #region Inspector Info

        // If this is set to true, the content of dialog, character and option fields 
        // (in dialog nodes) will be used as id value to get the actual text from 
        // I2's database.
        [SerializeField, Tooltip("Uses the content of fields in dialog nodes as id values " +
            "to fetch data from I2 Localization database")]
        private bool useI2Localization = true;

        [Header("References")]
        [SerializeField, Tooltip("The scene component or project asset that contains a valid graph")]
        protected Object graphContainer = default;

        [Header("Events")]
        [SerializeField, Tooltip("Invoked when the dialog system is ready to go")]
        protected UnityEvent dialogSystemReady = default;

        [SerializeField, Tooltip("Invoked when any dialog of the dialog system changes its status")]
        public UnityEvent<DialogNode> dialogChanged = default;

        [SerializeField, Tooltip("Invoked when all dialogs in a dialog path have been completed " +
            "and the last dialog had no output connection.")]
        public UnityEvent dialogPathEnded = default;

        [SerializeField, Tooltip("Invoked when the user cancels a dialog")]
        public UnityEvent dialogCanceled = default;


        #endregion

        protected IGraph graph = null;
        protected bool isPrepared = false;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The graph used to build the dialog system</summary>
        public IGraph Graph
        {
            get
            {
                if (graph == null)
                {
                    if (graphContainer is IContainer<IGraph> container)
                        graph = container.Value;
                }
                return graph;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all dialogs contained in the graph</summary>
        public IReadOnlyCollection<DialogNode> Dialogs => Graph.GetNodes<DialogNode>();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all root dialogs contained in the graph.
        /// A dialog is considered a "root dialog" if no other dialog precedes it or
        /// depends on it</summary>
        public IReadOnlyCollection<DialogNode> RootDialogs => Dialogs
            .Where(t => t.Inputs.Count == 0)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of all dialogs contained in the graph currently
        /// having the status set to ToDo</summary>
        public IReadOnlyCollection<DialogNode> ToDoDialogs => Dialogs
            .Where(t => t.Status == DialogStatus.Todo)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the first dialog node available with its status set to "InProgress".
        /// Returns null if there is none.
        /// </summary>
        public DialogNode CurrentDialog => 
            Dialogs.FirstOrDefault(t => t.Status == DialogStatus.InProgress);


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the first dialog node available with the specified dialogID value.
        /// </summary>
        /// <returns></returns>
        public DialogNode GetDialogById(string dialogId)
        {
            return Dialogs.FirstOrDefault(t => t.Dialog == dialogId);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the dialog node with the specified dialogId to InProgress status, to 
        /// start a dialog path.
        /// </summary>
        /// <param name="dialogId"></param>
        public void StartDialog (string dialogId)
        {
            GetDialogById(dialogId).Status = DialogStatus.InProgress;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts a dialog by setting to InProgress the status of the node related to 
        /// the referenced dialog part.
        /// </summary>
        /// <param name="dialog"></param>
        public void StartDialog(DialogPart dialog)
        {
            dialog.Node.Status = DialogStatus.InProgress;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Does a step forward in the dialog path by setting the current dialog node 
        /// to Completed status, and setting the following dialog node to InProgress.
        /// The next dialog node is reached by following the option port corresponding
        /// to the choice parameter value. If choice value is zero, it means that the 
        /// dialog path ends here, so after setting the current dialog to Completed, the
        /// whole path that led to it shoud be reset to ToDo status.
        /// </summary>
        /// <param name="choice"></param>
        public void ContinueDialog(int choice)
        {
            DialogNode current = CurrentDialog;
            DialogNode next = current.GetNextAt(choice);
            
            //TODO: set the status change and the event invocation locally on the DialogNode
            // script instead of doing it all here.
            
            // Invokes the event related to the option chosen by the player.
            switch (choice)
            {
                case 0:
                case 1:
                    current.onOption1.Invoke();
                    break;
                case 2:
                    current.onOption2.Invoke();
                    break;
                case 3:
                    current.onOption3.Invoke();
                    break;
                case 4:
                    current.onOption4.Invoke();
                    break;
            }

            current.Status = DialogStatus.Completed;
            if (next == null)
            {
                ResetDialogPath(current);
                dialogPathEnded.Invoke();
            }
            else
            {
                next.Status = DialogStatus.InProgress;
            }
        }

        /// <summary>
        /// This can be called to stop a dialog session, even if the last node in a 
        /// dialog path hasn't been reached.
        /// </summary>
        public void CancelDialog()
        {
            ResetDialogPath(CurrentDialog);
            dialogCanceled.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Recursively resets the status of the parameter node and all the nodes 
        /// that led to it.
        /// </summary>
        /// <param name="node"></param>
        public void ResetDialogPath(DialogNode node)
        {
            var previousNodes = node.Inputs;
            foreach(DialogNode previous in previousNodes)
            {
                if (previous.Status != DialogStatus.Todo)
                {
                    ResetDialogPath(previous);
                }
            }
            node.Status = DialogStatus.Todo;
        }


        ///////////////////////////////////////////////////////////////////////////
        protected virtual void Awake()
        {
            Prepare();
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Sets a new graph container</summary>
        public void SetGraphContainer(GraphBehaviour newGraphContainer)
        {
            graphContainer = newGraphContainer;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Call this method to make sure that the dialog system has been
        /// correctly initialized</summary>
        public void Prepare()
        {
            if (isPrepared)
                return;
            isPrepared = true;

            // Register the global callback to the statuses changes events
            IReadOnlyCollection<DialogNode> allNodes = Dialogs;
            foreach (DialogNode node in allNodes)
                node.onStatusChanged.AddListener(oldStatus => OnDialogStatusChanged(node, oldStatus));

            foreach (DialogNode node in allNodes)
            {
                // Sets all root dialogs status to ToDo
                node.Status = DialogStatus.Todo;

                // Sets the I2 option on each node (will be needed for dialog node properties).
                node.useI2Localization = useI2Localization;
            }

            //Skip one frame to adjust execution order in build
            IEnumerator DelayedEvent()
            {
                yield return null;
                // Invoke the "dialog system ready" event
                dialogSystemReady.Invoke();
            }
            StartCoroutine(DelayedEvent());
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Called when a dialog changed its status
        protected void OnDialogStatusChanged(DialogNode dialog, DialogStatus oldStatus)
        {
            // Invoke the dialogChanged event
            dialogChanged.Invoke(dialog);
        }
    }
}