using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplayerManager : MonoBehaviour
{
    private NetworkManager _manager;
    private NetworkClient _client;
    
    // Start is called before the first frame update
    void Start()
    {
        _manager = GetComponent<NetworkManager>();
        if (_manager.matchMaker == null)
        {
            _manager.StartMatchMaker();
        }
    }

    public void CreateGame()
    {
        _manager.matchMaker.CreateMatch(
            _manager.matchName,
            _manager.matchSize, 
            true, 
            "", 
            "", 
            "", 
            0, 
            0, 
            _manager.OnMatchCreate);
    }

    public void JoinGame()
    {
        _manager.matchMaker.ListMatches(
            0, 
            20, 
            "", 
            false, 
            0, 
            0, 
            OnMatchList);
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (LogFilter.logDebug)
        {
            Debug.LogFormat(
                "NetworkManager OnMatchList Success:{0}, ExtendedInfo:{1}, matchList.Count:{2}", 
                success, 
                extendedInfo, 
                matchList.Count);
        }

        _manager.matches = matchList;

        if (_manager.matches.Count == 1)
        {
            var match = _manager.matches[0];
            _manager.matchName = match.name;
            _manager.matchMaker.JoinMatch(
                match.networkId,
                "", 
                "", 
                "", 
                0, 
                0,
                OnMatchJoined);
        }
            
    }

    private void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (LogFilter.logDebug)
        {
            Debug.LogFormat("NetworkManager OnMatchJoined Success:{0}, ExtendedInfo:{1}, matchInfo:{2}",
                success,
                extendedInfo,
                matchInfo);
        }

        if (success)
        {
            _client = _manager.StartClient(matchInfo);
        }
    }
}
