using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

public class WorldManager : NetworkBehaviour
{
    public List<Selectable> prefabs;
    public GameObject World;

    private int _runningId;
    public List<WorldItem> worldItems = new List<WorldItem>();
    public static WorldManager Instance { get; private set; }

    public bool Active { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    
    public void SpawnWorld()
    {
        World.SetActive(true);
        Active = true;
    }

    [Server]
    public void CreateItem(int item)
    {
        var obj = Instantiate(prefabs[item], World.transform);

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
        var obj = Instantiate(prefabs[item], World.transform);
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
                obj = Instantiate(prefabs[item], World.transform).gameObject
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
        if (isServer && World)
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

    private void DisableClouds()
    {
        
    }
}

[Serializable]
public class WorldItem
{
    public int id;
    public int item;
    public GameObject obj;
}