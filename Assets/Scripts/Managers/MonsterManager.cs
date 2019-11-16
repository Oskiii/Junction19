using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterManager : NetworkBehaviour
{
    public List<Selectable> prefabs;
    public static MonsterManager Instance { get; private set; }
    private int _runningId;
    public List<Monster> monsters = new List<Monster>();

    public void Awake()
    {
        Instance = this;
    }

    [Server]
    public void CreateMonster(int monsterId)
    {
        var obj = Instantiate(prefabs[monsterId], WorldManager.Instance.World.transform);

        _runningId++;
        var monster = new Monster()
        {
            id = _runningId,
            obj = obj.gameObject,
            monsterId = monsterId
        };
        obj.GetComponent<MonsterContainer>().Monster = monster;
        monsters.Add(monster);
        RpcCreateMonster(monster.id, monster.monsterId);
    }

    [ClientRpc]
    private void RpcCreateMonster(int id, int monsterId)
    {
        if (isServer) return;
        var obj = Instantiate(prefabs[id], WorldManager.Instance.World.transform);
        var monster = new Monster()
        {
            id = id,
            obj = obj.gameObject,
            monsterId = monsterId
        };
        obj.GetComponent<MonsterContainer>().Monster = monster;
        Destroy(monster.obj.GetComponent<MoveMonsterToClickPoint>());
        Destroy(monster.obj.GetComponent<Draggable>());
        monsters.Add(monster);
    }

    [Server]
    private void UpdateItemPositions()
    {
        foreach (var monster in monsters)
        {
            RpcSyncedPos(
                monster.id,
                monster.monsterId,
                monster.obj.transform.localPosition,
                monster.obj.transform.localRotation,
                monster.obj.transform.localScale);
        }
    }

    [ClientRpc]
    private void RpcSyncedPos(
        int id,
        int monsterId,
        Vector3 transformLocalPosition, 
        Quaternion transformLocalRotation,
        Vector3 transformLocalScale)
    {
        if (isServer) return;
        var monster = monsters.Find(_ => _.id == id);
        if (monster == null)
        {
            monster = new Monster()
            {
                id = id,
                monsterId = monsterId,
                obj = Instantiate(prefabs[monsterId], WorldManager.Instance.World.transform).gameObject
            };
            monster.obj.GetComponent<MonsterContainer>().Monster = monster;
            Destroy(monster.obj.GetComponent<MoveMonsterToClickPoint>());
            Destroy(monster.obj.GetComponent<Draggable>());
            monsters.Add(monster);
        };
        monster.obj.transform.localPosition = transformLocalPosition;
        monster.obj.transform.localRotation = transformLocalRotation;
        monster.obj.transform.localScale = transformLocalScale;
    }

    private void Update()
    {
        if (isServer && WorldManager.Instance.World)
        {
            UpdateItemPositions();
        }
    }

    [Server]
    public void DeleteItem(int id)
    {
        var monster = monsters.Find(_ => _.id == id);
        if (monster == null)
        {
            return;
        }

        RpcDeleteMonster(id);
        monsters.Remove(monster);
        Destroy(monster.obj);
    }

    [ClientRpc]
    private void RpcDeleteMonster(int id)
    {
        var monster = monsters.Find(_ => _.id == id);
        if (monster == null)
        {
            return;
        }

        monsters.Remove(monster);
        Destroy(monster.obj);
    }

    [Server]
    public void MoveMonster(int monsterId, Vector3 posWorldPosition, float dist)
    {
        var monster = monsters.Find(_ => _.monsterId == monsterId);
        if (monster != null)
        {
            monster.obj.GetComponent<MoveCharacter>().MoveTowards(posWorldPosition, dist);
        }
    }
}

[Serializable]
public class Monster
{
    public int id;
    public int monsterId;
    public GameObject obj;
}