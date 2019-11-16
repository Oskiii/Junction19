using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
#pragma warning disable 618

public class MultiplayerManager : NetworkManager
{
    public GameObject LobbyCanvas;
    public GameObject PlayerCanvas;
    public GameObject DmCanvas;
    
    
    private NetworkClient _client;

    public static MultiplayerManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (matchMaker == null)
        {
            StartMatchMaker();
        }
    }

    public void CreateGame()
    {
        matchMaker.CreateMatch(
            matchName,
            matchSize, 
            true, 
            "", 
            "", 
            "", 
            0, 
            0, 
            OnMatchCreate);
    }

    public void JoinGame()
    {
        matchMaker.ListMatches(
            0, 
            20, 
            "", 
            false, 
            0, 
            0, 
            OnMatchList);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);

        if (matches.Count == 1)
        {
            var match = matches[0];
            matchName = match.name;
            matchMaker.JoinMatch(
                match.networkId,
                "", 
                "", 
                "", 
                0, 
                0,
                OnMatchJoined);
        }
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Client " + conn.connectionId + " Connected!"); ;
    }
}
