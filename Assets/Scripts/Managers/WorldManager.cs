using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
#pragma warning disable 618

public class WorldManager : NetworkBehaviour
{
    public List<Selectable> prefabs;
    public GameObject worldPrefab;
    private GameObject _world;

    private int _runningId;
    [FormerlySerializedAs("wordlItems")] [FormerlySerializedAs("NetworkChilds")] public List<WorldItem> worldItems = new List<WorldItem>();
    public static WorldManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    
    public GameObject SpawnWorld()
    {
        return _world = Instantiate(worldPrefab);
    }

    [Server]
    public void CreateItem(int item)
    {
        var obj = Instantiate(prefabs[item], _world.transform);

        _runningId++;
        var worldItem = new WorldItem()
        {
            id = _runningId,
            obj = obj.gameObject,
            item = item
        };
        worldItems.Add(worldItem);
        RpcCreateItem(worldItem.id, worldItem.item);
    }

    [ClientRpc]
    private void RpcCreateItem(int id, int item)
    {
        if (isServer) return;
        var obj = Instantiate(prefabs[item], _world.transform);
        var worldItem = new WorldItem()
        {
            id = id,
            obj = obj.gameObject,
            item = item
        };
        worldItems.Add(worldItem);
    }

    [Server]
    private void UpdateItemPositions()
    {
        foreach (var worldItem in worldItems)
        {
            RpcSyncedPos(
                worldItem.id,
                worldItem.obj.transform.localPosition,
                worldItem.obj.transform.localRotation,
                worldItem.obj.transform.localScale);
        }
    }

    [ClientRpc]
    private void RpcSyncedPos(
        int id, 
        Vector3 transformLocalPosition, 
        Quaternion transformLocalRotation,
        Vector3 transformLocalScale)
    {
        if (isServer) return;
        Debug.Log($"Updating position for {id}");
        var worldItem = worldItems.Find(_ => _.id == id);
        if (worldItem == null) return;
        Debug.Log($"Found object for {id}");
        worldItem.obj.transform.localPosition = transformLocalPosition;
        worldItem.obj.transform.localRotation = transformLocalRotation;
        worldItem.obj.transform.localScale = transformLocalScale;
    }

    private void Update()
    {
        if (isServer && _world)
        {
            UpdateItemPositions();
        }
    }
}

[Serializable]
public class WorldItem
{
    public int id;
    public int item;
    public GameObject obj;
}