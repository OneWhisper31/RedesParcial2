using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class SessionListHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [SerializeField] TextMeshProUGUI _statusText;

    [SerializeField] SessionItem _sessionItemPrefab;

    [SerializeField] VerticalLayoutGroup _verticalLayoutGroup;

    private void OnEnable()
    {
        _networkRunner.OnSessionListUpdate += ReceiveSessionList;
    }

    private void OnDisable()
    {
        _networkRunner.OnSessionListUpdate -= ReceiveSessionList;
    }
    
    void ClearSessionList()
    {
        foreach (Transform child in _verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    void ReceiveSessionList(List<SessionInfo> newSessionList)
    {
        //Limpiamos la lista de sesiones previamente creadas
        ClearSessionList();

        //Si no tenemos sesiones
        if (newSessionList.Count == 0)
        {
            //Mostramos el texto diciendo que no se encontraron
            NoSessionsFound();
        }
        else //Sino
        {
            //Agregamos cada sesion a la lista
            foreach (var session in newSessionList)
            {
                AddSessionToList(session);
            }
        }
    }

    void NoSessionsFound()
    {
        _statusText.text = "No Sessions Found";

        _statusText.gameObject.SetActive(true);
    }

    void AddSessionToList(SessionInfo session)
    {
        var newSessionItem = Instantiate(_sessionItemPrefab, _verticalLayoutGroup.transform);
        newSessionItem.SetSessionInfo(session);

        newSessionItem.OnJoinSession += JoinSelectedSession;
    }

    //Cuando se clickee el boton de Join en una sesion que tiene el buscador
    //se va a ejecutar este metodo ya que lo registramos al evento dentro del Item
    void JoinSelectedSession(SessionInfo session)
    {
        _networkRunner.JoinGame(session);
    }
}
