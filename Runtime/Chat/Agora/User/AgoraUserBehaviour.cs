#if AGORA
using agora_gaming_rtc;
using UnityEngine;
using d4160.MonoBehaviourData;
using UltEvents;

namespace d4160.Chat.Agora
{
    public class AgoraUserBehaviour : MonoBehaviourUnityData<AgoraUserSO>
    {
        [Tooltip("Used when you want to select an instanced VideoSurface as default, to pass to the Provider. Useful for UniqueObjectProvider.")]
        [SerializeField] private VideoSurface _videoSurfaceInstance;
        [Tooltip("If is not null, set as parent in each instantiation.")]
        [SerializeField] private Transform _parent;
        [Header("EVENTS")]
        [SerializeField] private UltEvent<uint, int> _onUserJoined;
        [SerializeField] private UltEvent<uint, USER_OFFLINE_REASON> _onUserOffline;

        void OnEnable () {
            if (_data)
            {
                _data.RegisterEvents();
                _data.OnUserJoinedEvent += _onUserJoined.Invoke;
                _data.OnUserOfflineEvent += _onUserOffline.Invoke;
            }
        }

        void Start() {
            if (_data) {
                if (_data.VideoSurfaceProvider)
                {
                    if (_videoSurfaceInstance) _data.VideoSurfaceProvider.Prefab = _videoSurfaceInstance;
                    if (_parent) _data.VideoSurfaceProvider.Parent = _parent;
                }
                _data.Setup();
            }
        }

        void OnDisable(){
            if (_data)
            {
                _data.UnregisterEvents();
                _data.OnUserJoinedEvent -= _onUserJoined.Invoke;
                _data.OnUserOfflineEvent -= _onUserOffline.Invoke;
            }
        }   
    }
}
#endif