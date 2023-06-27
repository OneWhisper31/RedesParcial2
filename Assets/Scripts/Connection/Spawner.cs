using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _playerPrefab;

    CharacterInputHandler _characterInputHandler;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    { 
        if (runner.IsServer)
        {
            Debug.Log("[Custom Msg] Player Joined, I´m the Host");

            runner.Spawn(_playerPrefab, null, null, player);
        }
        else
        {
            Debug.Log("[Custom Msg] Player Joined, I´m not the Host");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (!NetworkPlayer.Local) return;

        if (!_characterInputHandler)
        {
            _characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();
        }
        else
        {
            input.Set(_characterInputHandler.GetInputs());
        }
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) 
    {
        runner.Shutdown();
    }

    #region Unused Callbacks

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
