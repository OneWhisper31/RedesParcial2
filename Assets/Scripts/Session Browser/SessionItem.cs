using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class SessionItem : MonoBehaviour
{
    [SerializeField] Text _sessionNameText;
    [SerializeField] Text _playerCountText;
    [SerializeField] Button _joinButton;

    SessionInfo _sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    public void SetSessionInfo(SessionInfo session)
    {
        _sessionInfo = session;

        _sessionNameText.text = _sessionInfo.Name;

        _playerCountText.text = $"{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}";

        //_joinButton.enabled = (_sessionInfo.PlayerCount < _sessionInfo.MaxPlayers) ? true : false;
        _joinButton.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }

    public void OnClick()
    {
        OnJoinSession?.Invoke(_sessionInfo);
    }
}
