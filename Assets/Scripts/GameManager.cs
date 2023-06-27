using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask wallLayer;

    public Transform player;

    //public event Action<Transform> onDetected;

    [SerializeField]NPC[] npcs;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OnPlayerDetected(NPC npcAsked)
    {
        foreach (NPC npc in npcs)
        {
            if (npc == npcAsked)
                continue;

            Debug.Log("="+ npc.name);

            npc.TrackNewPathPlayer(player);
            npc.onPlayerNotice = true;
        }
    }



}
