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
        MultiplayerManager.Instance.LobbyItems.SetActive(false);

        if (isServer)
        {
            MultiplayerManager.Instance.DmItems.SetActive(true);
            WorldManager.Instance.SpawnWorld();
        }
        else
        {
            WorldManager.Instance.HideWorld();
            MultiplayerManager.Instance.PlayerItems.SetActive(true);
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
        PlayerManager.Instance.SetCharacter(characterId, (int) playerId.Value);
    }

    public override void OnStartLocalPlayer()
    {
        PlayerManager.Instance.LocalClient = this;
    }

    public void MoveCharacter(int playerId, Vector3 localPosition, float dist, Vector3 worldDir)
    {
        CmdMoveCharacter(playerId, localPosition, dist, worldDir);
    }

    [Command]
    public void CmdMoveCharacter(int playerId, Vector3 worldLocalPosition, float dist, Vector3 worldDir)
    {
        PlayerManager.Instance.SetCharacterMovePoint(playerId, worldLocalPosition, dist, worldDir);
    }
}