using UnityEngine;
using Fusion;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }

    NicknameText _myNickname;

    public event Action OnLeft = delegate { };

    public override void Spawned()
    {
        Color newColor = Color.white;

        if (Object.HasInputAuthority)
        {
            Local = this;

            newColor = Color.blue;

            RPC_SetNickname("John Doe " + UnityEngine.Random.Range(1, 1001));
        }
        else if (Object.HasStateAuthority && !Object.HasInputAuthority)
        {
            newColor = Color.yellow;
        }
        else
        {
            newColor = Color.red;
        }


        GetComponentInChildren<SkinnedMeshRenderer>().material.color = newColor;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNickname(string newName)
    {
        Nickname = newName;
    }

    static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        var networkPlayerBehaviour = changed.Behaviour;

        networkPlayerBehaviour.UpdateNickname(networkPlayerBehaviour.Nickname.ToString());
    }

    void UpdateNickname(string newName)
    {
        //_myNickname.UpdateText(newName);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
