using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour {

  public List<GameObject> objectOptions;
  public TMP_Dropdown dropdown;

  private int selectedIndex = 0;

  void Start() {
    dropdown = GetComponent<TMP_Dropdown>();
    List<string> optionsNames = new List<string>();
    foreach (var item in objectOptions) {
      optionsNames.Add(item.name);
    }
    dropdown.AddOptions(optionsNames);
    dropdown.onValueChanged.AddListener(SetIndex);
  }
  public void SpawnOption() {
    Debug.Log("Spawning " + selectedIndex);
    Instantiate(objectOptions[selectedIndex], new Vector3(0, 1, 0), Quaternion.identity);
  }

  public void SetIndex(int index) {
    selectedIndex = index;
  }
}