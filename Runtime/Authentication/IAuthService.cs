﻿using UnityEngine.Promise;

namespace d4160.Authentication
{
    /// <summary>
    /// Contract for authentication
    /// systems (PlayFab, ...).
    /// </summary>
    public interface IAuthService
    {
        string DisplayName { get; }
        string Id { get; }
        string SessionTicket { get; }
        bool HasSession { get; }

        /// <summary>
        /// Authenticate this auth layer.
        /// </summary>
        /// <param name="completer">
        /// When done, this completer is resolved or rejected.
        /// </param>
        void Login(Completer completer);

        void Register(Completer completer);

        void Logout(Completer completer);
    }
}