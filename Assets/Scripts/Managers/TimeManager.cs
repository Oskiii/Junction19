using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : NetworkBehaviour
{
    [SyncVar]
    [Range(0, 1)]
    public float time;

    public static TimeManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    [Server]
    public void SetTime(float newTime)
    {
        time = newTime;
    }
}
