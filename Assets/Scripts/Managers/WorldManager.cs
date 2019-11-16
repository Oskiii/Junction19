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
        _world = Instantiate(worldPrefab);
        return _world;
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
                worldItem.item,
                worldItem.obj.transform.localPosition,
                worldItem.obj.transform.localRotation,
                worldItem.obj.transform.localScale);
        }
    }

    [ClientRpc]
    private void RpcSyncedPos(
        int id,
        int item,
        Vector3 transformLocalPosition, 
        Quaternion transformLocalRotation,
        Vector3 transformLocalScale)
    {
        if (isServer) return;
        Debug.Log($"Updating position for {id}");
        var worldItem = worldItems.Find(_ => _.id == id);
        if (worldItem == null)
        {
            worldItem = new WorldItem()
            {
                id = id,
                item = item,
                obj = Instantiate(prefabs[item], _world.transform).gameObject
            };
            worldItems.Add(worldItem);
        };
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

    [Server]
    public void DeleteItem(int id)
    {
        var worldItem = worldItems.Find(_ => _.id == id);
        if (worldItem == null)
        {
            return;
        }

        RpcDeleteItem(id);
        worldItems.Remove(worldItem);
    }

    [ClientRpc]
    private void RpcDeleteItem(int id)
    {
        var worldItem = worldItems.Find(_ => _.id == id);
        if (worldItem == null)
        {
            return;
        }

        worldItems.Remove(worldItem);
    }
}

[Serializable]
public class WorldItem
{
    public int id;
    public int item;
    public GameObject obj;
}