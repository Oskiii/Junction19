using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 618

public class PlayerManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MultiplayerManager.Instance.LobbyCanvas.SetActive(false);
        
        if (isServer)
        {
            MultiplayerManager.Instance.DmCanvas.SetActive(true);
        }
        else
        {
            MultiplayerManager.Instance.PlayerCanvas.SetActive(true);
        }
    }
}
