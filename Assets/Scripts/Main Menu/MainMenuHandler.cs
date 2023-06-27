using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkHandler;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] GameObject _statusPanel;
    [SerializeField] GameObject _browserPanel;
    [SerializeField] GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] Button _joinLobbyBTN;
    [SerializeField] Button _openHostBTN;
    [SerializeField] Button _hostGameBTN;

    [Header("Inputfields")]
    [SerializeField] InputField _hostSessionName;

    [Header("Texts")]
    [SerializeField] Text _statusText;

    void Start()
    {
        //A cada boton que tenemos le agregamos por codigo el metodo que deberian ejecutar cuando son clickeados
        _joinLobbyBTN.onClick.AddListener(BTN_JoinLobby);
        _openHostBTN.onClick.AddListener(BTN_ShowHostPanel);
        _hostGameBTN.onClick.AddListener(BTN_CreateGameSession);

        //Cuando el Network Runner se termine de conectar al Lobby
        //Le decimos mediante la suscripcion al evento que apague el Panel de Status y prenda el Browser
        _networkHandler.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _browserPanel.SetActive(true);
        };
    }

    #region Button Methods

    void BTN_JoinLobby()
    {
        _networkHandler.JoinLobby();

        _initialPanel.SetActive(false);

        _statusPanel.SetActive(true);

        _statusText.text = "Joining Lobby...";
    }

    void BTN_ShowHostPanel()
    {
        _browserPanel.SetActive(false);

        _hostGamePanel.SetActive(true);
    }

    void BTN_CreateGameSession()
    {
        _networkHandler.CreateSession(_hostSessionName.text, "Game");
    }

    #endregion
}
