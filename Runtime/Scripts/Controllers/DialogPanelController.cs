using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Reflectis.PLG.Dialogs
{
    /// <summary>
    /// This script is supposed to be attached to a UI Panel, to be used to display dialog
    /// data received from the graph-based dialog system.
    /// To make this panel reachable by the dialog system, add a listener to the DialogChanged 
    /// event in a DialogSystem, and set it to call the OnDialogChanged method in this class.
    /// </summary>
    public class DialogPanelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private DialogPanel playerPanel;
        [SerializeField]
        private DialogPanel npcPanel;

        [Space]
        [Header("Typewrite Settings")]
        [SerializeField]
        private float charactersPerSecond = 20f;
        [SerializeField]
        private float interpunctuationDelay = 0.5f;
        [SerializeField] //Todo fare una getcomponent?
        private TypewriterEffect typewriterEffect;
        private bool typeWriteActive;
        [Header("Skip options")]
        [SerializeField]
        private bool enableSkip;
        [SerializeField]
        private bool quickSkip;
        [SerializeField][Min(1)]
        private int skipSpeedup = 5;

        private DialogSystem dialogSystemInUse = default;
        private DialogPanel currentDialogPanel;

        [System.Serializable]
        public class DialogPanel
        {
            [Tooltip("Gameobject containing the whole dialog panel")]
            public GameObject panelObject = default;
            [Tooltip("Option to show nickname text")]
            public bool showNickname = false;
            [Tooltip("Nickname bg object")]
            public GameObject nicknameBg = default;
            [Tooltip("Text component that will display the character name")]
            public TextMeshProUGUI nicknameText = default;
            [Tooltip("Text component that will display the dialog text")]
            public TextMeshProUGUI dialogText = default;
            [Tooltip("Option to show avatar image")]
            public bool showAvatarContainer = false;
            [Tooltip("Container that will display the character icon/avatar")]
            public Image avatarContainer = default;
            [Tooltip("List of Gameobjects containing the buttons needed for dialog choices")]
            public GameObject[] choiceButtonGroups = default;

            // This will store a reference to the text component of each choice button, so that 
            // the button labels can be customized for each dialog.
            public List<List<TextMeshProUGUI>> choiceButtonLabels = default;
        }

        public DialogPanel PlayerPanel { get => playerPanel; }
        public DialogPanel NpcPanel { get => npcPanel; }

        /// <summary>
        /// Makes the dialog system in use step on to the next dialog along the dialogue path.
        /// This is supposed to be called by the buttons on the dialog panel.
        /// </summary>
        /// <param name="choice">The dialog option to follow in the path.</param>
        public void ContinueDialog(int choice)
        {
            //Manage skip and all possible cases with next dialog and typewrite effect
            if (enableSkip)
            {
                if (typewriterEffect.CurrentlySkipping)
                    return;

                if (!typewriterEffect.ReadyForNewText)
                    Skip();
                else
                    dialogSystemInUse.ContinueDialog(choice);
            }
            else
            {
                if (typeWriteActive)
                {
                    if (!typewriterEffect.ReadyForNewText)
                        return;
                    else 
                        dialogSystemInUse.ContinueDialog(choice);
                }
                else
                    dialogSystemInUse.ContinueDialog(choice);
            }
        }

        /// <summary>
        /// This can be called to stop a dialog session, even if the last node in a 
        /// dialog path hasn't been reached.
        /// </summary>
        public void CancelDialog()
        {
            dialogSystemInUse.CancelDialog();
            dialogSystemInUse = null;
        }


        public void OnDialogChanged(DialogSystem dialogSystem)
        {
            DialogNode currentDialog = dialogSystem.CurrentDialog;
            // If a dialog is in progress, enables the panel and uses the dialog data 
            // to fill the panel fields.
            if (currentDialog != null)
            {
                if (currentDialogPanel != null)
                    currentDialogPanel.panelObject.SetActive(false);

                currentDialogPanel = playerPanel;
                if (currentDialog.npcDialogPanel)
                    currentDialogPanel = npcPanel;

                dialogSystemInUse = dialogSystem;
                currentDialogPanel.panelObject.SetActive(true);
                SetChoiceButtons(currentDialog, currentDialogPanel);
                SetNicknameText(currentDialog.Character, currentDialogPanel);
                SetAvatar(currentDialog.Avatar, currentDialogPanel);
                SetDialogText(currentDialog.Dialog, currentDialogPanel);
            }
            // if the there is no dialog in progress, disables the dialog panel.
            else
            {
                currentDialogPanel.panelObject.SetActive(false);
                dialogSystemInUse = null;
            }
        }


        private void Start()
        {
            SetUpPanels(playerPanel);
            SetUpPanels(npcPanel);

            typeWriteActive = charactersPerSecond > 0;
            if (typeWriteActive)
            {
                typewriterEffect.Setup(charactersPerSecond, interpunctuationDelay, quickSkip, skipSpeedup);
                playerPanel.dialogText.maxVisibleCharacters = 0;
                npcPanel.dialogText.maxVisibleCharacters = 0;
            }

        }

        private void SetUpPanels(DialogPanel panel)
        {
            if (!panel.panelObject)
                return;

            panel.panelObject.SetActive(false);

            panel.choiceButtonLabels = new List<List<TextMeshProUGUI>>();
            // Fetches a reference to the TextMeshPro Text component of every choice button 
            // label and stores it in choiceButtonLabels. 
            for (int i = 0; i < panel.choiceButtonGroups.Length; i++)
            {
                List<TextMeshProUGUI> componentList =
                    panel.choiceButtonGroups[i].GetComponentsInChildren<TextMeshProUGUI>(true).ToList();

                // Sends a warning if not enough buttons are found as children of each button group.
                if ((i == 0 && componentList.Count < 1) || (i > 0 && componentList.Count < i))
                {
                    Debug.LogWarning("Not enough Button type components found in button group " + i +
                        " (" + panel.choiceButtonGroups[i].name + ").");
                }
                else
                {
                    panel.choiceButtonLabels.Add(componentList);
                }
            }
        }


        /// <summary>
        /// Enables the button group needed by the dialog element, and sets all button labels.
        /// </summary>
        /// <param name="dialog"></param>
        private void SetChoiceButtons(DialogNode dialog, DialogPanel currentPanel)
        {
            foreach (GameObject buttonGroup in currentPanel.choiceButtonGroups)
            {
                buttonGroup.SetActive(false);
            }

            int optionCount = dialog.OptionCount;
            currentPanel.choiceButtonGroups[optionCount].SetActive(true);
            switch (optionCount)
            {
                case 0:
                    currentPanel.choiceButtonLabels[0][0].text = dialog.Option1Label;
                    break;
                case 1:
                    currentPanel.choiceButtonLabels[1][0].text = dialog.Option1Label;
                    break;
                case 2:
                    currentPanel.choiceButtonLabels[2][0].text = dialog.Option1Label;
                    currentPanel.choiceButtonLabels[2][1].text = dialog.Option2Label;
                    break;
                case 3:
                    currentPanel.choiceButtonLabels[3][0].text = dialog.Option1Label;
                    currentPanel.choiceButtonLabels[3][1].text = dialog.Option2Label;
                    currentPanel.choiceButtonLabels[3][2].text = dialog.Option3Label;
                    break;
                case 4:
                    currentPanel.choiceButtonLabels[4][0].text = dialog.Option1Label;
                    currentPanel.choiceButtonLabels[4][1].text = dialog.Option2Label;
                    currentPanel.choiceButtonLabels[4][2].text = dialog.Option3Label;
                    currentPanel.choiceButtonLabels[4][3].text = dialog.Option4Label;
                    break;
            }
        }

        private void SetNicknameText(string titleId, DialogPanel currentPanel)
        {
            if (!currentPanel.showNickname || string.IsNullOrEmpty(titleId))
                currentPanel.nicknameText.gameObject.SetActive(false); //VA SPENTO PADRE!!!
            {
                currentPanel.nicknameText.gameObject.SetActive(true);
                currentPanel.nicknameText.text = titleId;
            }
        }

        private void SetDialogText(string dialogId, DialogPanel currentPanel)
        {
            //if (currentPanel.dialogText.text == dialogId)
            //{
            //    currentPanel.dialogText.text = "";
            //    currentPanel.dialogText.ForceMeshUpdate();
            //}
            currentPanel.dialogText.text = dialogId;
            currentPanel.dialogText.ForceMeshUpdate();

            //Qui va aggiunta logica per come scrivere il testo.
            if (typeWriteActive)
                typewriterEffect.PrepareForNewText(currentPanel.dialogText);
        }

        private void SetAvatar(Sprite texture, DialogPanel currentPanel)
        {
            if (!currentPanel.showAvatarContainer || texture == null)
                currentPanel.avatarContainer.gameObject.SetActive(false);
            else
            {
                currentPanel.avatarContainer.gameObject.SetActive(true);
                currentPanel.avatarContainer.sprite = texture;
            }
        }

        private void Skip()
        {
            //Check se bool è attivo
            //Collegarlo all'evento del bottone dello start
            //Fare lo skip
            typewriterEffect.Skip(currentDialogPanel.dialogText);
        }
    }
}