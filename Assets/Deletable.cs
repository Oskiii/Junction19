using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deletable : MonoBehaviour
{
    private Selectable _selectable;
    private WorldItemContainer _worldItemContainer;

    private void Start()
    {
        _selectable = GetComponentInChildren<Selectable>();
        _worldItemContainer = GetComponent<WorldItemContainer>();
    }

    private void Update()
    {
        if (_selectable.IsSelected && Input.GetKeyDown(KeyCode.Delete) && _worldItemContainer.WorldItem != null)
        {
            WorldManager.Instance.DeleteItem(_worldItemContainer.WorldItem.id);
        }
    }
}
