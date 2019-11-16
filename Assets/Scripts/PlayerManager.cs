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
        MultiplayerManager.Instance.LobbyCanvas.gameObject.SetActive(false);
        
        if (isServer)
        {
            MultiplayerManager.Instance.DmCanvas.gameObject.SetActive(true);
        }
        else
        {
            MultiplayerManager.Instance.PlayerCanvas.gameObject.SetActive(true);
        }
    }
}
