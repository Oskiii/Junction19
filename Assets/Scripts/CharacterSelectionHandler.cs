using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionHandler : MonoBehaviour {
  public List<GameObject> objectOptions;
  public GameObject selectionPanel;
  private int selectedIndex = 0;

  public void SetIndex(int index) {
    selectedIndex = index;
  }

  public void JoinGame() {
    Instantiate(objectOptions[selectedIndex], new Vector3(0, 0, 0), Quaternion.identity);
    selectionPanel.SetActive(false);
  }
}