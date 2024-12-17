using Reflectis.PLG.Dialogs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Reflectis.PLG.DialogsDialogsReflectis
{
    public class DialogPanelSpawner : MonoBehaviour
    {
        public float charactersPerSecond;
        public float interpunctuationDelay;
        public bool enableSkip;
        public bool quickSkip;
        public int skipSpeedup;
        public bool showPlayerNickname;
        public bool useReflectisNickname;
        public bool showNpcNickname;
        public bool useReflectisAvatar;
        public bool showPlayerAvatarContainer;
        public bool showNpcAvatarContainer;

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
