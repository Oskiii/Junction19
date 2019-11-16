using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    public GameObject CharacterSelect;
    public List<GameObject> CharacterModels;
    public List<Character> Characters;

    public Client LocalClient;
    public static PlayerManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    
    [Client]
    public void ShowCharacterScreen()
    {
        CharacterSelect.SetActive(true);
    }
    
    [Client]
    public void SelectCharacter(int id)
    {
        CharacterSelect.SetActive(false);
        LocalClient.SelectCharacter(id);
    }

    [Server]
    public void SetCharacter(int characterId, int playerId)
    {
        Debug.Log($"Player {playerId }Set characterId as {characterId}");
        var character = Instantiate(CharacterModels[characterId], WorldManager.Instance.World.transform);
        character.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        character.GetComponent<MoveToClickPoint>();
        Characters.Add(new Character()
        {
            playerId = playerId,
            characterId = characterId,
            obj = character
        });
    }

    [ClientRpc]
    private void RpcCreateCharacter(int playerId, int characterId)
    {
        if (isServer) return;
        var character = Instantiate(CharacterModels[characterId], WorldManager.Instance.World.transform);
        Characters.Add(new Character()
        {
            playerId = playerId,
            characterId = characterId,
            obj = character
        });
    }
    
    [ClientRpc]
    private void RpcSyncedPos(
        int characterId,
        int playerId,
        Vector3 transformLocalPosition, 
        Quaternion transformLocalRotation,
        Vector3 transformLocalScale)
    {
        if (isServer) return;
        var character = Characters.Find(_ => _.playerId == playerId);
        if (character == null)
        {
            character = new Character()
            {
                characterId = characterId,
                playerId = playerId,
                obj = Instantiate(CharacterModels[characterId], WorldManager.Instance.World.transform)
            };
            Characters.Add(character);
        };
        character.obj.transform.localPosition = transformLocalPosition;
        character.obj.transform.localRotation = transformLocalRotation;
        character.obj.transform.localScale = transformLocalScale;
    }
    
    

    [Server]
    private void UpdateItemPositions()
    {
        foreach (var character in Characters)
        {
            RpcSyncedPos(
                character.characterId,
                character.playerId,
                character.obj.transform.localPosition,
                character.obj.transform.localRotation,
                character.obj.transform.localScale);
        }
    }

    private void Update()
    {
        if (isServer && WorldManager.Instance.World)
        {
            UpdateItemPositions();
        }
    }
}

[Serializable]
public class Character
{
    public int playerId;
    public int characterId;
    public GameObject obj;
}