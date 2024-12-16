using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Reflectis.PLG.Dialogs
{
    /// <summary>
    /// This script is supposed to be attached to a UI Panel made with Unity components,
    /// to be used to display dialog data received from the graph-based dialog system.
    /// To make this panel reachable by the dialog system, add a listener to the DialogChanged 
    /// event in a DialogSystem, and set it to call the OnDialogChanged method in this class.
    /// </summary>
    public class DialogPanelController : DialogPanelControllerGeneric
    {
        [SerializeField, Tooltip("Container that will display the player icon/avatar")]
        private Image avatarContainerPlayer = default;
        [SerializeField, Tooltip("Container that will display the npc icon/avatar")]
        private Image avatarContainerNpc = default;

        public Image AvatarContainerPlayer { get => avatarContainerPlayer; set => avatarContainerPlayer = value; }
        public Image AvatarContainerNpc { get => avatarContainerNpc; set => avatarContainerNpc = value; }


        protected override void SetAvatar(Sprite texture, DialogPanel currentPanel)
        {
            var avatarContainer = currentPanel.isPlayer ? avatarContainerPlayer : avatarContainerNpc;
            if (!currentPanel.showAvatarContainer || texture == null)
                avatarContainer.gameObject.SetActive(false);
            else
            {
                avatarContainer.gameObject.SetActive(true);
                avatarContainer.sprite = texture;
            }
        }
    }
}