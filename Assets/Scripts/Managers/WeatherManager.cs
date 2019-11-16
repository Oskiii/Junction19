using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

#pragma warning disable 618

public class WeatherManager : NetworkBehaviour {
  public GameObject clouds;
  public GameObject rainyClouds;
  [SyncVar]
  public WeatherType currentWeather;

  private WeatherType _lastWeather;
  public static WeatherManager Instance { get; private set; }

  public void Awake() {
    Instance = this;
  }

  [Server]
  public void SetWeather(WeatherType weatherType) {
    currentWeather = weatherType;
  }

  [Server]
  public void ToggleWeather() {
    switch (currentWeather) {
      case WeatherType.Clear:
        SetWeather(WeatherType.Cloudy);
        break;
      case WeatherType.Cloudy:
        SetWeather(WeatherType.Clear);
        break;
      case WeatherType.Rainy:
        SetWeather(WeatherType.Rainy);
        break;
      default:
        Debug.Log("Unknown weather type set");
        return;
    }
  }

  private void Update() {
    if (_lastWeather != currentWeather && WorldManager.Instance.Active) {
      _lastWeather = currentWeather;
      switch (currentWeather) {
        case WeatherType.Clear:
          clouds.SetActive(false);
          rainyClouds.SetActive(false);
          break;
        case WeatherType.Cloudy:
          clouds.SetActive(true);
          rainyClouds.SetActive(false);
          break;
        case WeatherType.Rainy:
          rainyClouds.SetActive(true);
          break;
        default:
          Debug.Log("Unknown weather type set");
          return;
      }
    }
  }
}

public enum WeatherType {
  Clear,
  Cloudy,
  Rainy
}