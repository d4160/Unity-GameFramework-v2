#if PHOTON_UNITY_NETWORKING
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine;
using d4160.Singleton;
using d4160.Singleton.Photon;

namespace d4160.SceneManagement.Photon 
{
    /// <summary>
    /// When don't want to use AutomaticallySyncScene
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PhotonSceneLoaderBehaviour : SingletonBehaviourPun<PhotonSceneLoaderBehaviour>
    {
        [Expandable]
        [SerializeField] private SceneManagerSO _sceneManagerAsset;
        [DropdownIndex("SceneCollectionsNames")]
        [SerializeField] private int _sceneCollection;

        public int SceneCollection { get => _sceneCollection; set => _sceneCollection = value; }

#if UNITY_EDITOR
        private string[] SceneCollectionsNames => _sceneManagerAsset?.GetSceneCollectionNames;
#endif

        private bool _loadingLevelAndPausedNetwork;

        void OnEnable() 
        {
            if (_sceneManagerAsset) 
            {
                _sceneManagerAsset.RegisterEvents();
                _sceneManagerAsset.OnCollectionLoaded += ScnMngSO_OnCollectionLoaded;
            }
        }

        void OnDisable()
        {
            if (_sceneManagerAsset) 
            {
                _sceneManagerAsset.UnregisterEvents();
                _sceneManagerAsset.OnCollectionLoaded -= ScnMngSO_OnCollectionLoaded;
            }
        }

        private void ScnMngSO_OnCollectionLoaded(int index, string label) {

            if (_loadingLevelAndPausedNetwork) 
            {
                PhotonNetwork.IsMessageQueueRunning = true;
            }
        }

        [Button]
        [PunRPC]
        public void LoadSceneCollectionAsync ()
        {
            PauseNetwork();
            _sceneManagerAsset?.LoadSceneCollectionAsync(_sceneCollection);
        }

        [PunRPC]
        public void LoadSceneCollectionLabelAsync (string label)
        {
            PauseNetwork();
            _sceneManagerAsset?.LoadSceneCollectionAsync(label);
        }

        [PunRPC]
        public void LoadSceneCollectionIndexAsync (int index)
        {
            PauseNetwork();
            _sceneManagerAsset?.LoadSceneCollectionAsync(index);
        }

        private void PauseNetwork()
        {
            _loadingLevelAndPausedNetwork = true;
            PhotonNetwork.IsMessageQueueRunning = false;
        }
    }
}
#endif