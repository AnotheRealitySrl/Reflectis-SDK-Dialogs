using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Reflectis.SDK.Dialogs
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
        [SerializeField, Tooltip("X coord for the Player avatar if Left-sided")]
        private float avatarContainerPlayerPosXLeft = default;
        [SerializeField, Tooltip("X coord for the Player avatar if Right-sided")]
        private float avatarContainerPlayerPosXRight = default;

        [SerializeField, Tooltip("Container that will display the npc icon/avatar")]
        private Image avatarContainerNpc = default;
        [SerializeField, Tooltip("X coord for the NPC avatar if Left-sided")]
        private float avatarContainerNpcPosXLeft = default;
        [SerializeField, Tooltip("X coord for the NPC avatar if Right-sided")]
        private float avatarContainerNpcPosXRight = default;

        public Image AvatarContainerPlayer { get => avatarContainerPlayer; set => avatarContainerPlayer = value; }
        public float AvatarContainerPlayerPosXLeft { get => avatarContainerPlayerPosXLeft; set => avatarContainerPlayerPosXLeft = value; }
        public float AvatarContainerPlayerPosXRight { get => avatarContainerPlayerPosXRight; set => avatarContainerPlayerPosXRight = value; }

        public Image AvatarContainerNpc { get => avatarContainerNpc; set => avatarContainerNpc = value; }
        public float AvatarContainerNpcPosXLeft { get => avatarContainerNpcPosXLeft; set => avatarContainerNpcPosXLeft = value; }
        public float AvatarContainerNpcPosXRight { get => avatarContainerNpcPosXRight; set => avatarContainerNpcPosXRight = value; }


        protected override void SetAvatar(Sprite texture, DialogNode.LayoutElementSide characterAvatarSide, DialogPanel currentPanel)
        {
            var avatarContainer = currentPanel.isPlayer ? avatarContainerPlayer : avatarContainerNpc;
            var avatarXPos = currentPanel.isPlayer
                ? (characterAvatarSide == DialogNode.LayoutElementSide.Left ? avatarContainerPlayerPosXLeft : avatarContainerPlayerPosXRight)
                : (characterAvatarSide == DialogNode.LayoutElementSide.Left ? avatarContainerNpcPosXLeft : avatarContainerNpcPosXRight);
            if (!currentPanel.showAvatarContainer || texture == null)
                avatarContainer.gameObject.SetActive(false);
            else
            {
                RectTransform avatarContainerRT = currentPanel.nicknameBg.GetComponent<RectTransform>();
                avatarContainerRT.anchoredPosition = new Vector2(avatarXPos, avatarContainerRT.anchoredPosition.y);
                avatarContainer.gameObject.SetActive(true);
                avatarContainer.sprite = texture;
            }
        }
    }
}