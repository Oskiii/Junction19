using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnTest : MonoBehaviour
{

    public GameObject Prefab;
    
    public void SpawnPrefab()
    {
        var obj = Instantiate(Prefab, transform.position, transform.rotation);
        NetworkServer.Spawn(obj);
    }
}
