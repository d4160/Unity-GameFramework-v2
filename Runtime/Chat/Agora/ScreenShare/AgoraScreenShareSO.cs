#if AGORA
using agora_gaming_rtc;
using UnityEngine;
using NaughtyAttributes;

namespace d4160.Chat.Agora
{
    [CreateAssetMenu(menuName = "d4160/Agora/ScreenShare")]
    public class AgoraScreenShareSO : ScriptableObject
    {
        [SerializeField] private ScreenCaptureParametersStruct _screenCaptureParams;

        private readonly AgoraScreenShareService _screenShareService = AgoraScreenShareService.Instance;

        /// <summary>
        /// The first and default display
        /// </summary>
        /// <value></value>
        public int DisplayID => _screenShareService.DisplayID;
        public VideoSurface VideoSurface { get; set; }

        [Button]
        public void StartScreenCapture() {
            StartScreenCapture(_screenCaptureParams);
        }

        public void StartScreenCapture(ScreenCaptureParametersStruct sparams)
        {
            _screenShareService.VideoSurface = VideoSurface;
            _screenShareService.StartScreenCapture(sparams);
        }

        [Button]
        public void StopScreenCapture(){
            _screenShareService.StopScreenCapture();
        } 
    }
}
#endif