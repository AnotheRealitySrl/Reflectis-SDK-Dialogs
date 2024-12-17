using Reflectis.PLG.Graphs;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Reflectis.PLG.Dialogs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A Graph Node used to represent a dialog element for the Dialog System
    /// </summary>
    public class DialogNode : Node
    {

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The possible dialog statuses</summary>
        public enum DialogStatus
        {
            Todo,
            InProgress,
            Completed
        }

        ///////////////////////////////////////////////////////////////////////////
        [Header("Ports")]
        [SerializeField, HideInInspector, PortLabel("")]
        protected MultiInputPort<DialogNode> input = default;

        [SerializeField, HideInInspector, PortLabel("Next / Option 1")]
        protected OutputPort<DialogNode> option1 = default;

        [SerializeField, HideInInspector]
        protected OutputPort<DialogNode> option2 = default;

        [SerializeField, HideInInspector]
        protected OutputPort<DialogNode> option3 = default;

        [SerializeField, HideInInspector]
        protected OutputPort<DialogNode> option4 = default;


        [Header("Data")]
        [SerializeField, TextArea, NodeData, Tooltip("Dialog text (or reference for the database)")]
        protected string dialog;

        [SerializeField, NodeData, Tooltip("Character name (or reference for the database)")]
        protected string character;

        [SerializeField, NodeData, Tooltip("Reference to the picture to use as icon/avatar")]
        protected Sprite avatar;

        [SerializeField, NodeData, Tooltip("Text label to show on dialog choice button")]
        protected string option1Label;

        [SerializeField, NodeData, Tooltip("Text label to show on dialog choice button")]
        protected string option2Label;

        [SerializeField, NodeData, Tooltip("Text label to show on dialog choice button")]
        protected string option3Label;

        [SerializeField, NodeData, Tooltip("Text label to show on dialog choice button")]
        protected string option4Label;

        [SerializeField, NodeData, Tooltip("The current dialog status")]
        protected DialogStatus status = DialogStatus.Todo;


        [Header("Settings")]
        [Tooltip("choose beetween player and NPC UI elements")]
        public bool npcDialogPanel;

        [Header("Events")]
        [SerializeField, Tooltip("Event called when the dialog status changes")]
        public UnityEvent<DialogStatus> onStatusChanged = default;

        [SerializeField, Tooltip("Invoked when the player chooses the first dialog option.")]
        public UnityEvent onOption1 = default;
        [SerializeField, Tooltip("Invoked when the player chooses the second dialog option.")]
        public UnityEvent onOption2 = default;
        [SerializeField, Tooltip("Invoked when the player chooses the third dialog option.")]
        public UnityEvent onOption3 = default;
        [SerializeField, Tooltip("Invoked when the player chooses the fourth dialog option.")]
        public UnityEvent onOption4 = default;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary></summary>The collection of dialogs that precede or depends on
        /// this dialog</summary>
        public IReadOnlyCollection<DialogNode> Inputs => input.LinkedNodes;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the node attached to the first connected port that 
        /// can be found.</summary>
        public DialogNode Next
        {
            get
            {
                if (option1.LinkedNodes.Count > 0)
                {
                    return option1.LinkedNodes.FirstOrDefault();
                }
                else if (option2.LinkedNodes.Count > 0)
                {
                    return option1.LinkedNodes.FirstOrDefault();
                }
                else if (option3.LinkedNodes.Count > 0)
                {
                    return option1.LinkedNodes.FirstOrDefault();
                }
                else if (option4.LinkedNodes.Count > 0)
                {
                    return option1.LinkedNodes.FirstOrDefault();
                }
                else return null;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The previous dialog element(s)</summary>
        public IReadOnlyCollection<DialogNode> Previous => Inputs.ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Number of "option" ports that are currently conntected to another node.
        /// The panel that is gonna show this dialog is supposed to expose the same
        /// amount of dialog choices.
        /// Normal dialog boxes are considered as single-choice dialogs.
        /// </summary>
        public int OptionCount
        {
            get
            {
                //int count = 0;
                //if (option1.LinkedPorts.Count > 0) count++;
                //if (option2.LinkedPorts.Count > 0) count++;
                //if (option3.LinkedPorts.Count > 0) count++;
                //if (option4.LinkedPorts.Count > 0) count++;
                //return count;

                if (option4.LinkedPorts.Count > 0) return 4;
                else if (option3.LinkedPorts.Count > 0) return 3;
                else if (option2.LinkedPorts.Count > 0) return 2;
                else if (option1.LinkedPorts.Count > 0) return 1;
                else return 0;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the dialog text. If the system is using I2 localization, 
        /// it uses the dialog value as id to get the actual dialog text from I2's 
        /// database.</summary>
        public virtual string Dialog
        {
            get => dialog;
            set => dialog = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the character name. If the system is using I2 localization, 
        /// it uses the dialog value as id to get the actual character name from I2's 
        /// database.</summary>
        public virtual string Character
        {
            get => character;
            set => character = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The avatar/icon image to use for this dialog.</summary>
        public Sprite Avatar
        {
            get => avatar;
            set => avatar = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the first option button.</summary>
        public virtual string Option1Label
        {
            get => option1Label;
            set => option1Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the second option button.</summary>
        public virtual string Option2Label
        {
            get => option2Label;
            set => option2Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the third option button.</summary>
        public virtual string Option3Label
        {
            get => option3Label;
            set => option3Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the fourth option button.</summary>
        public virtual string Option4Label
        {
            get => option4Label;
            set => option4Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The current dialog status</summary>
        public DialogStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    var oldStatus = status;
                    status = value;

                    onStatusChanged.Invoke(oldStatus);
                }
            }
        }


        /// <summary>
        /// Returns the dialog node connected to the dialog option specified by choice.
        /// </summary>
        /// <param name="choice">The dialog option to follow.</param>
        /// <returns></returns>
        public DialogNode GetNextAt(int choice)
        {
            DialogNode result;
            switch (choice) {
                case 1:
                    result = option1.LinkedNodes.FirstOrDefault();
                    break;
                case 2:
                    result = option2.LinkedNodes.FirstOrDefault();
                    break;
                case 3:
                    result = option3.LinkedNodes.FirstOrDefault();
                    break;
                case 4:
                    result = option4.LinkedNodes.FirstOrDefault();
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }


        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR

        /// This dictionary maps a status to a color used in the editor's graph
        /// to highlight nodes depending on their statuses
        protected static readonly Dictionary<DialogStatus, Color> nodeColors = new Dictionary<DialogStatus, Color>()
        {
            { DialogStatus.Completed, Color.green },
            { DialogStatus.InProgress, Color.yellow },
            { DialogStatus.Todo, Color.gray },
        };

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Called when a node needs to be visually rendered in the
        /// graph panel</summary>
        /// <param name="nodeElement">The visual element of the node</param>
        public override void OnDrawNodeElementInEditor(VisualElement nodeElement)
        {
            if (Application.isPlaying)
            {
                Color originalColor = nodeElement.style.backgroundColor.value;

                void updateColor()
                {
                    if (!nodeColors.TryGetValue(Status, out Color newNodeColor))
                        newNodeColor = originalColor;
                    nodeElement.style.backgroundColor = newNodeColor;
                }

                updateColor();
                onStatusChanged.AddListener(oldStatus => updateColor());
            }
        }
#endif
    }
}