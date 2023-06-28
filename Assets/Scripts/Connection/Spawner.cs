using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _playerPrefab;

    CharacterInputHandler _characterInputHandler;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        switch (runner.ActivePlayers.Count())
        {
            case 1:
                GameManager.GM._textConnecting.gameObject.SetActive(true);
                Debug.Log("[Custom Msg] First Player Joined, Waiting for more player");
                break;
            case 2:
                if (runner.IsServer)
                {
                    Debug.Log("[Custom Msg] Second Player Joined, I'm the Host");
                    GameManager.GM._textConnecting.gameObject.SetActive(false);
                    runner.ActivePlayers.Select(x =>
                    {
                        NetworkObject obj = runner.Spawn(_playerPrefab, null, null, x).Object;

                        obj.transform.position = obj.HasInputAuthority ?
                            GameManager.GM.InitialPos1.position : GameManager.GM.InitialPos2.position;

                        return obj;
                    }).ToArray();
                    
                }
                else
                {
                    Debug.Log("[Custom Msg] Second Player Joined, I'm not the Host");
                }
                break;
            default:
                break;
        }
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() > 3)
            return;
        //NetworkPlayer.Local.player.OnDisconected();
        foreach (var item in FindObjectsOfType<NetworkPlayer>())
        {
            item.player.OnDisconected();
        }
        GameManager.GM._textConnecting.gameObject.SetActive(true);
        GameManager.GM.OnQuitEndScreen();
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
        SceneManager.LoadScene("MainMenu");
    }

    #region Unused Callbacks

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
