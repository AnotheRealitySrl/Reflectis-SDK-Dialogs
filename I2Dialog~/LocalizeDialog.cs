using I2.Loc;

using UnityEngine;
using UnityEngine.Video;

namespace Reflectis.PLG.Dialogs.Utils
{
    public class LocalizeDialog : MonoBehaviour
    {
        [SerializeField, Tooltip("Enable description localization")]
        private bool localizeDescription = false;
        [SerializeField, Tooltip("Enable image localization")]
        private bool localizeImage = false;
        [SerializeField, Tooltip("Enable videoClip localization")]
        private bool localizeVideo = false;

        [Space]
        [SerializeField, Tooltip("I2 key relative to the dialog's description")]
        private LocalizedString descriptionKey;
        [SerializeField, Tooltip("I2 key relative to the dialog's sprite")]
        private LocalizedString imageKey;
        [SerializeField, Tooltip("I2 key relative to the dialog's video")]
        private LocalizedString videoKey;


        void OnEnable()
        {
            Dialog dialog = GetComponent<Dialog>();

            //Localize dialog description
            if (localizeDescription)
                dialog.Node.Description = descriptionKey;

            //Localize dialog image
            if (localizeImage)
                dialog.Node.Image = LocalizationManager.GetTranslatedObjectByTermName<Sprite>(imageKey.mTerm);

            //Localize dialog video
            if (localizeVideo)
                dialog.Node.VideoClip = LocalizationManager.GetTranslatedObjectByTermName<VideoClip>(videoKey.mTerm);
        }
    }
}
