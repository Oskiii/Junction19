using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour {

  public List<GameObject> spawnableObjects;
  public TMP_Dropdown dropdown;

  private int selectedIndex = 0;

  void Start() {
    dropdown = GetComponent<TMP_Dropdown>();
    List<string> spawnableOptions = new List<string>();
    foreach (var item in spawnableObjects) {
      spawnableOptions.Add(item.name);
    }
    dropdown.AddOptions(spawnableOptions);
    dropdown.onValueChanged.AddListener(SetIndex);
  }
  public void SpawnOption() {
    Debug.Log("Spawning " + selectedIndex);
    Instantiate(spawnableObjects[selectedIndex], new Vector3(0, 10, 0), Quaternion.identity);
  }

  public void SetIndex(int index) {
    selectedIndex = index;
  }
}