using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour {

  public List<Selectable> objectOptions;
  public bool populateFromEnum = false;
  public TMP_Dropdown dropdown;
  public WeatherManager weatherManager;
  private int selectedIndex = 0;

  void Start() {
    dropdown = GetComponent<TMP_Dropdown>();
    List<string> optionsNames = new List<string>();

    if (!populateFromEnum && WorldManager.Instance != null) {
      objectOptions = WorldManager.Instance.prefabs;
      foreach (var item in objectOptions) {
        optionsNames.Add(item.name);
      }
    } else {
      string[] weatherEnumNames = System.Enum.GetNames(typeof(WeatherType));
      foreach (var item in weatherEnumNames) {
        optionsNames.Add(item);
      }
    }
    dropdown.AddOptions(optionsNames);
  }
  public void SpawnOption() {
    Debug.Log("Spawning " + dropdown.value);
    Instantiate(objectOptions[dropdown.value], new Vector3(0, 1, 0), Quaternion.identity);
  }

  public void SetWeather() {
    WeatherType selectedEnum = (WeatherType) System.Enum.Parse(typeof(WeatherType), dropdown.options[dropdown.value].text);
    weatherManager.SetWeather(selectedEnum);
  }
}