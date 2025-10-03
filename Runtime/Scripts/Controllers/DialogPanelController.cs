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
        [Header("Player")]
        [SerializeField, Tooltip("X coord for the player name if Left-sided")]
        private float nicknameBgPosXLeftPlayer = default;
        [SerializeField, Tooltip("X coord for the player name if Right-sided")]
        private float nicknameBgPosXRightPlayer = default;
        [Space]
        [SerializeField, Tooltip("Container that will display the player icon/avatar")]
        private Image avatarContainerPlayer = default;
        [Space]
        [SerializeField, Tooltip("X coord for the player avatar if Left-sided")]
        private float avatarContainerPosXLeftPlayer = default;
        [SerializeField, Tooltip("X coord for the player avatar if Right-sided")]
        private float avatarContainerPosXRightPlayer = default;
        [Space]
        [SerializeField, Tooltip("X coord for the player dialog text if Left-sided")]
        private float dialogBoxPosXLeftPlayer = default;
        [SerializeField, Tooltip("X coord for the player dialog text if Right-sided")]
        private float dialogBoxPosXRightPlayer = default;

        [Header("NPC")]
        [SerializeField, Tooltip("X coord for the npc name if Left-sided")]
        private float nicknameBgPosXLeftNpc = default;
        [SerializeField, Tooltip("X coord for the npc name if Right-sided")]
        private float nicknameBgPosXRightNpc = default;
        [Space]
        [SerializeField, Tooltip("Container that will display the npc icon/avatar")]
        private Image avatarContainerNpc = default;
        [Space]
        [SerializeField, Tooltip("X coord for the npc avatar if Left-sided")]
        private float avatarContainerPosXLeftNpc = default;
        [SerializeField, Tooltip("X coord for the npc avatar if Right-sided")]
        private float avatarContainerPosXRightNpc = default;
        [Space]
        [SerializeField, Tooltip("X coord for the npc dialog text if Left-sided")]
        private float dialogBoxPosXLeftNpc = default;
        [SerializeField, Tooltip("X coord for the npc dialog text if Right-sided")]
        private float dialogBoxPosXRightNpc = default;

        public float NicknameBgPosXLeftPlayer { get => nicknameBgPosXLeftPlayer; set => nicknameBgPosXLeftPlayer = value; }
        public float NicknameBgPosXRightPlayer { get => nicknameBgPosXRightPlayer; set => nicknameBgPosXRightPlayer = value; }

        public Image AvatarContainerPlayer { get => avatarContainerPlayer; set => avatarContainerPlayer = value; }

        public float AvatarContainerPosXLeftPlayer { get => avatarContainerPosXLeftPlayer; set => avatarContainerPosXLeftPlayer = value; }
        public float AvatarContainerPosXRightPlayer { get => avatarContainerPosXRightPlayer; set => avatarContainerPosXRightPlayer = value; }

        public float DialogBoxPosXLeftPlayer { get => dialogBoxPosXLeftPlayer; set => dialogBoxPosXLeftPlayer = value; }
        public float DialogBoxPosXRightPlayer { get => dialogBoxPosXRightPlayer; set => dialogBoxPosXRightPlayer = value; }


        public float NicknameBgPosXLeftNpc { get => nicknameBgPosXLeftNpc; set => nicknameBgPosXLeftNpc = value; }
        public float NicknameBgPosXRightNpc { get => nicknameBgPosXRightNpc; set => nicknameBgPosXRightNpc = value; }

        public Image AvatarContainerNpc { get => avatarContainerNpc; set => avatarContainerNpc = value; }

        public float AvatarContainerPosXLeftNpc { get => avatarContainerPosXLeftNpc; set => avatarContainerPosXLeftNpc = value; }
        public float AvatarContainerPosXRightNpc { get => avatarContainerPosXRightNpc; set => avatarContainerPosXRightNpc = value; }

        public float DialogBoxPosXLeftNpc { get => dialogBoxPosXLeftNpc; set => dialogBoxPosXLeftNpc = value; }
        public float DialogBoxPosXRightNpc { get => dialogBoxPosXRightNpc; set => dialogBoxPosXRightNpc = value; }

        protected override void SetNicknameText(string titleId, DialogNode.LayoutElementSide characterNameSide, DialogPanel currentPanel)
        {
            RectTransform nicknameBGRT = currentPanel.nicknameBg.GetComponent<RectTransform>();
            float nicknameBgPosXLeft = currentPanel.isPlayer ? nicknameBgPosXLeftPlayer : nicknameBgPosXLeftNpc;
            float nicknameBgPosXRight = currentPanel.isPlayer ? nicknameBgPosXRightPlayer : nicknameBgPosXRightNpc;
            nicknameBGRT.anchoredPosition = new Vector2(
                characterNameSide == DialogNode.LayoutElementSide.Left ? nicknameBgPosXLeft : nicknameBgPosXRight,
                nicknameBGRT.anchoredPosition.y
            );

            base.SetNicknameText(titleId, characterNameSide, currentPanel);
        }
        protected override void SetAvatar(Sprite texture, DialogNode.LayoutElementSide characterAvatarSide, DialogPanel currentPanel)
        {
            var avatarContainer = currentPanel.isPlayer ? avatarContainerPlayer : avatarContainerNpc;
            if (!currentPanel.showAvatarContainer || texture == null)
                avatarContainer.gameObject.SetActive(false);
            else
            {
                float avatarContainerPosXLeft = currentPanel.isPlayer ? avatarContainerPosXLeftPlayer : avatarContainerPosXLeftNpc;
                float avatarContainerPosXRight = currentPanel.isPlayer ? avatarContainerPosXRightPlayer : avatarContainerPosXRightNpc;
                float dialogBoxPosXLeft = currentPanel.isPlayer ? dialogBoxPosXLeftPlayer : dialogBoxPosXLeftNpc;
                float dialogBoxPosXRight = currentPanel.isPlayer ? dialogBoxPosXRightPlayer : dialogBoxPosXRightNpc;

                RectTransform avatarContainerRT = avatarContainer.GetComponent<RectTransform>();
                avatarContainerRT.anchoredPosition = new Vector2(
                    characterAvatarSide == DialogNode.LayoutElementSide.Left ? avatarContainerPosXLeft : avatarContainerPosXRight,
                    avatarContainerRT.anchoredPosition.y);
                avatarContainer.gameObject.SetActive(true);
                avatarContainer.sprite = texture;

                var currentDialogBoxRT = currentPanel.dialogText.GetComponent<RectTransform>();
                currentDialogBoxRT.anchoredPosition = new Vector2(
                    characterAvatarSide == DialogNode.LayoutElementSide.Left ? dialogBoxPosXLeft : dialogBoxPosXRight,
                    currentDialogBoxRT.anchoredPosition.y);
            }
        }
    }
}