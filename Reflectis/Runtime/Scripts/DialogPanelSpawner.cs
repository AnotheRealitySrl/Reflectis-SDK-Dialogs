using Reflectis.PLG.Dialogs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Reflectis.PLG.DialogsDialogsReflectis
{
    public class DialogPanelSpawner : MonoBehaviour
    {
        [SerializeField]
        private float charactersPerSecond;
        [SerializeField]
        private float interpunctuationDelay;
        [SerializeField]
        private bool enableSkip;
        [SerializeField]
        private bool quickSkip;
        [SerializeField]
        private int skipSpeedup;
        [SerializeField]
        private bool showPlayerNickname;
        [SerializeField]
        private bool useReflectisNickname;
        [SerializeField]
        private bool showNpcNickname;
        [SerializeField]
        private bool useReflectisAvatar;
        [SerializeField]
        private bool showPlayerAvatarContainer;
        [SerializeField]
        private bool showNpcAvatarContainer;

        private void Start()
        {
            Addressables.LoadAssetAsync<GameObject>("DialogPanel").Completed += OnLoadCompleted;
        }

        private void OnLoadCompleted(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(obj.Result).GetComponent<DialogPanelControllerGeneric>().SetSettings(charactersPerSecond, interpunctuationDelay, enableSkip, quickSkip, skipSpeedup,
            showPlayerNickname, showNpcNickname, showPlayerAvatarContainer, showNpcAvatarContainer, useReflectisNickname, useReflectisAvatar);
            }
            else
            {
                Debug.LogError($"Loading Error");
            }
        }
    }
}
