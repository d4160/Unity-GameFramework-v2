using System.Collections;
using System.Collections.Generic;
using d4160.MonoBehaviourData;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;

namespace d4160.Instancers
{
    public class GameObjectFactoryBehaviour : MonoBehaviourUnityData<GameObjectFactorySO>
    {
        [Header("EVENTS")]
        [SerializeField] private UltEvent<GameObject> _onInstanced;
        [SerializeField] private UltEvent<GameObject> _onDestroy;

        void OnEnable() {
            if (_data) {
                _data.RegisterEvents();
                _data.OnInstanced += _onInstanced.Invoke;
                _data.OnDestroy += _onDestroy.Invoke;
            }
        }

        void OnDisable() {
            if (_data) {
                _data.UnregisterEvents();
                _data.OnInstanced -= _onInstanced.Invoke;
                _data.OnDestroy -= _onDestroy.Invoke;
            }
        }

        void Start() {
            if (_data) {
                _data.Setup();
            }
        }

        [Button]
        public GameObject Instantiate() {
            if (_data) return _data.Instantiate(); return null;
        }

        public void Destroy(GameObject instance) {
            if (_data) _data.Destroy(instance);
        }
    }
}