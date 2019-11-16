using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 618

public class Client : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MultiplayerManager.Instance.LobbyCanvas.SetActive(false);
        
        if (isServer)
        {
            MultiplayerManager.Instance.DmCanvas.SetActive(true);
            WorldManager.Instance.SpawnWorld();
        }
        else
        {
            MultiplayerManager.Instance.PlayerCanvas.SetActive(true);
        }
    }
    
    [Client]
    public void SelectCharacter(int characterId, int playerID)
    {
        CmdSetCharacter(characterId, playerID);
    }

    [Command]
    private void CmdSetCharacter(int characterId, int playerId)
    {
        PlayerManager.Instance.SetCharacter(characterId, playerId);
    }
}
