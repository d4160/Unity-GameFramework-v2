#if PHOTON_UNITY_NETWORKING
using System;
using System.Collections;
using System.Collections.Generic;
using d4160.Collections;
using d4160.Core;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Logger = d4160.Logging.M31Logger;

namespace d4160.Networking.Photon {
    [CreateAssetMenu (menuName = "d4160/Networking/Photon Room")]
    public class PhotonRoomSO : ScriptableObject {

        [Header ("ROOM")]
        [SerializeField] private HashtableStruct[] _customRoomProperties;
        [SerializeField] private HashtableStruct[] _expectedRoomProperties;

        [Header ("PLAYER")]
        [Tooltip("This hashtable is for LocalPlayer only")]
        [SerializeField] private HashtableStruct[] _customPlayerProperties;

        public bool InRoom => _roomService.InRoom;
        public Room CurrentRoom => _roomService.CurrentRoom;
        public Dictionary<int, Player> Players => _roomService.Players; // id|actorNumber
        public int PlayerCount => _roomService.PlayerCount;
        public Player[] PlayerList => _roomService.PlayerList; // In room also
        public Player[] PlayerListOthers => _roomService.PlayerListOthers;
        public byte MaxPlayers { get => _roomService.MaxPlayers; set => _roomService.MaxPlayers = value; }
        public int PlayerTtl { get => _roomService.PlayerTtl; set => _roomService.PlayerTtl = value; }
        public int EmptyRoomTtl { get => _roomService.EmptyRoomTtl; set => CurrentRoom.EmptyRoomTtl = value; }

        public event Action<Player> OnPlayerEnteredRoomEvent;
        public event Action<Player> OnPlayerLeftRoomEvent;
        public event Action<ExitGames.Client.Photon.Hashtable> OnRoomPropertiesUpdateEvent;
        public event Action<Player, ExitGames.Client.Photon.Hashtable> OnPlayerPropertiesUpdateEvent;
        public event Action<Player> OnMasterClientSwitchedEvent;

        private void CallOnPlayerEnteredRoomEvent (Player player) => OnPlayerEnteredRoomEvent?.Invoke (player);
        private void CallOnPlayerLeftRoomEvent (Player player) => OnPlayerLeftRoomEvent?.Invoke (player);
        private void CallOnRoomPropertiesUpdateEvent (ExitGames.Client.Photon.Hashtable propertiesThatChanged) => OnRoomPropertiesUpdateEvent?.Invoke (propertiesThatChanged);
        private void CallOnPlayerPropertiesUpdateEvent(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
        }
        private void CallOnMasterClientSwitchedEvent (Player player) => OnMasterClientSwitchedEvent?.Invoke (player);

        private readonly PhotonRoomService _roomService = PhotonRoomService.Instance;

        public void RegisterEvents () {
            PhotonRoomService.OnPlayerEnteredRoomEvent += CallOnPlayerEnteredRoomEvent;
            PhotonRoomService.OnPlayerLeftRoomEvent += CallOnPlayerLeftRoomEvent;
            PhotonRoomService.OnRoomPropertiesUpdateEvent += CallOnRoomPropertiesUpdateEvent;
            PhotonRoomService.OnPlayerPropertiesUpdateEvent += CallOnPlayerPropertiesUpdateEvent;
            PhotonRoomService.OnMasterClientSwitchedEvent += CallOnMasterClientSwitchedEvent;
        }

        public void UnregisterEvents () {
            PhotonRoomService.OnPlayerEnteredRoomEvent -= CallOnPlayerEnteredRoomEvent;
            PhotonRoomService.OnPlayerLeftRoomEvent -= CallOnPlayerLeftRoomEvent;
            PhotonRoomService.OnRoomPropertiesUpdateEvent -= CallOnRoomPropertiesUpdateEvent;
            PhotonRoomService.OnPlayerPropertiesUpdateEvent -= CallOnPlayerPropertiesUpdateEvent;
            PhotonRoomService.OnMasterClientSwitchedEvent -= CallOnMasterClientSwitchedEvent;
        }

        public GameObject InstantiateRoomObject (string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null) {
            return _roomService.InstantiateRoomObject(prefabName, position, rotation, group, data);
        }

        [Button("SetRoomCustomProperties")]
        public bool SetCustomProperties () {
            _roomService.CustomRoomProperties = _customRoomProperties;
            _roomService.ExpectedProperties = _expectedRoomProperties;
            return _roomService.SetCustomProperties();
        }

        public bool SetMasterClient (Player masterClientPlayer) {
            return _roomService.SetMasterClient(masterClientPlayer);
        }

        public bool SetPropertiesListedInLobby (string[] lobbyProps) {
            return _roomService.SetPropertiesListedInLobby(lobbyProps);
        }

        public Player GetPlayer (int id, bool findMaster = false) { // id 0 return master
            return _roomService.GetPlayer(id, findMaster);
        }

        public bool AddPlayer (Player player) {
            return _roomService.AddPlayer(player);
        }

        [Button]
        public bool ClearExpectedUsers () {
            return _roomService.ClearExpectedUsers();
        }

        public bool SetExpectedUsers (string[] newExpectedUsers) {
            return _roomService.SetExpectedUsers(newExpectedUsers);
        }

        public void AllocateRoomViewID (PhotonView view) {
            _roomService.AllocateRoomViewID(view);
        }

        [Button]
        public bool SetPlayerCustomProperties () {
            return SetPlayerCustomProperties(HashtableStruct.GetPhotonHashtable(_customPlayerProperties));
        }

        public bool SetPlayerCustomProperties (ExitGames.Client.Photon.Hashtable customProperties) {
            return _roomService.SetPlayerCustomProperties(customProperties);
        }

        public void RemovePlayerCustomProperties (string[] customPropertiesToDelete) {
            _roomService.RemovePlayerCustomProperties(customPropertiesToDelete);
        }

        public void DestroyPlayerObjects (Player targetPlayer) {
            _roomService.DestroyPlayerObjects(targetPlayer);
        }

        [Button]
        public void DestroyAll () {
            _roomService.DestroyAll();
        }

        public string GetCustomRoomPropKey(int index) {
            if (_customRoomProperties.IsValidIndex(index))
                return _customRoomProperties[index].key;
            return string.Empty;
        }

        public string GetCustomRoomPropValue(int index) {
            if (_customRoomProperties.IsValidIndex(index))
                return _customRoomProperties[index].value;
            return string.Empty;
        }

        public void SetCustomRoomProperty(string key, CommonType type, string value) {
            for (int i = 0; i < _customRoomProperties.Length; i++) {
                bool found = _customRoomProperties[i].SetValue(key, type, value);
                if (found) break;
            }
        }

        public string GetExpectedRoomPropKey(int index) {
            if (_expectedRoomProperties.IsValidIndex(index))
                return _expectedRoomProperties[index].key;
            return string.Empty;
        }

        public void SetExpectedRoomProperty(string key, CommonType type, string value) {
            for (int i = 0; i < _expectedRoomProperties.Length; i++) {
                bool found = _expectedRoomProperties[i].SetValue(key, type, value);
                if (found) break;
            }
        }

        public string GetCustomPlayerPropKey(int index) {
            if (_customPlayerProperties.IsValidIndex(index))
                return _customPlayerProperties[index].key;
            return string.Empty;
        }

        public string GetCustomPlayerPropValue(int index) {
            if (_customPlayerProperties.IsValidIndex(index))
                return _customPlayerProperties[index].value;
            return string.Empty;
        }

        public T GetCustomPlayerPropValue<T>(int index) {
            if (_customPlayerProperties.IsValidIndex(index)) {
                return _customPlayerProperties[index].GetValue<T>();
            }
            return default;
        }

        public void SetCustomPlayerProperty(string key, CommonType type, string value) {
            for (int i = 0; i < _customPlayerProperties.Length; i++) {
                bool found = _customPlayerProperties[i].SetValue(key, type, value);
                if (found) break;
            }
        }
    }
}
#endif