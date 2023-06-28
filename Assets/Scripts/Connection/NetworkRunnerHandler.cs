using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;

    NetworkRunner _currentRunner;

    public event Action OnJoinedLobby = delegate { };
    public event Action<List<SessionInfo>> OnSessionListUpdate = delegate { };

    #region Lobby
    public void JoinLobby()
    {
        if (_currentRunner) Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);
        _currentRunner.AddCallbacks(this);
        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {

        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to Join Lobby");
        }
        else
        {
            Debug.Log("[Custom Msg] Joined Lobby");

            OnJoinedLobby();
        }
    }

    #endregion

    #region Start-Join Game

    public void CreateGame(string sessionName, string sceneName)
    {
        var clientTask = InitializeGame(_currentRunner, GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public void JoinGame(SessionInfo sessionInfo)
    {
        var clientTask = InitializeGame(_currentRunner, GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex);
    }

    async Task InitializeGame(NetworkRunner runner, GameMode gameMode, string sessionName, SceneRef scene)
    {
        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager,
            PlayerCount = 2
        });

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to start game");
        }
        else
        {
            Debug.Log("[Custom Msg] Game Started");
        }
    }

    #endregion

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate(sessionList);
    }

    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
