using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager GM { get; private set; }

    [SerializeField] TextMeshProUGUI _textRestarting;
    public TextMeshProUGUI _textConnecting,_textPlayers;

    public Transform InitialPos1;
    public Transform InitialPos2;

    public GameObject canvas,winSing,loseSing;

    //key ID, Value isready?
    public Dictionary<string, bool> OnReplayReady = new Dictionary<string, bool>();

    private void Awake()
    {
        GM = GetComponent<GameManager>();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_OnEnd(string playerDeadID)
    {
        Time.timeScale = 0;
        canvas.SetActive(true);

        loseSing.SetActive(false);
        winSing.SetActive(false);

        string playerID= NetworkPlayer.Local.Runner.GetPlayerUserId();

        if (NetworkPlayer.Local.player.Life <= 0)
            loseSing.SetActive(true);
        else
            winSing.SetActive(true);

        if(!OnReplayReady.ContainsKey(playerID))
            OnReplayReady.Add(playerID, false);//Seteo el dicionario para poner que ninguno de los dos esta listo para reiniciar

        foreach (var item in OnReplayReady)
        {
            Debug.Log(item.Key);
        }
    }
    public void OnQuitEndScreen()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        winSing.SetActive(false);
        loseSing.SetActive(false);
    }
    public bool IsEveryOneReadyToReset() {
        if (OnReplayReady.Count < 2)
            return false;

        foreach (var item in OnReplayReady)
        {
            if (item.Value == false)
                return false;
        }

        return true;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_OnResetLevel(string IDisReady)
    {
        OnReplayReady[IDisReady] = true;
        _textPlayers.text = $"{OnReplayReady.Where(x => x.Value).Count()}/2";



        if (IsEveryOneReadyToReset())
        {
            OnReplayReady = new Dictionary<string, bool>();
            _textPlayers.text = "0/2";
            OnQuitEndScreen();

            FindObjectsOfType<NetworkPlayer>().Map(x => x.transform.position = x.Object.HasInputAuthority ?
                            InitialPos1.position : InitialPos2.position);

            NetworkPlayer.Local.player.ResetLife();
            //StartCoroutine(ResetLevel());
        }

    }

    IEnumerator ResetLevel()
    {
        
        _textRestarting.gameObject.SetActive(true);
        NetworkPlayer.Local.player.ResetLife();
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        _textRestarting.gameObject.SetActive(false);
    }
}