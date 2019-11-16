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
    public void SelectCharacter(int characterId)
    {
        CmdSetCharacter(characterId, netId);
    }

    [Command]
    private void CmdSetCharacter(int characterId, NetworkInstanceId playerId)
    {
        PlayerManager.Instance.SetCharacter(characterId, (int)playerId.Value);
    }

    public override void OnStartLocalPlayer()
    {
        PlayerManager.Instance.LocalClient = this;
    }

    public void MoveCharacter(int playerId, Vector3 localPosition, float dist)
    {
        CmdMoveCharacter(playerId, localPosition, dist);
    }

    [Command]
    public void CmdMoveCharacter(int playerId, Vector3 worldLocalPosition, float dist)
    {
        PlayerManager.Instance.SetCharacterMovePoint(playerId, worldLocalPosition, dist);
    }
}
