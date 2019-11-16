using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deletable : MonoBehaviour
{
    private Selectable _selectable;
    private WorldItemContainer _worldItemContainer;
    private MonsterContainer _monsterContainer;

    private void Start()
    {
        _selectable = GetComponentInChildren<Selectable>();
        _worldItemContainer = GetComponent<WorldItemContainer>();
        _monsterContainer = GetComponent<MonsterContainer>();
    }

    private void Update()
    {
        if (_selectable.IsSelected && Input.GetKeyDown(KeyCode.Delete) && _worldItemContainer && _worldItemContainer.WorldItem != null)
        {
            WorldManager.Instance.DeleteItem(_worldItemContainer.WorldItem.id);
        }
        if (_selectable.IsSelected && Input.GetKeyDown(KeyCode.Delete) && _monsterContainer && _monsterContainer.Monster != null)
        {
            MonsterManager.Instance.DeleteItem(_monsterContainer.Monster.id);
        }
    }
}
