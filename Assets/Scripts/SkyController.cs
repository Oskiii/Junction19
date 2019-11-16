using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour {
  public float rotationAngle = 1f;
  public Light directionalLight;
  public Light areaLight;
  private Renderer _rend;
  private float _normalALIntensity;
  private Color _normalALColor;
  private Color _nightALColor;

  private void Start() {
    _rend = GetComponent<Renderer>();
    _normalALIntensity = areaLight.intensity;
    _normalALColor = areaLight.color;
    _nightALColor = new Color32(47, 203, 255, 255);
  }

  void Update() {
    transform.RotateAround(Vector3.zero, Vector3.right, rotationAngle * Time.deltaTime);
    float time = TimeManager.Instance ? TimeManager.Instance.time : 0f;
    if (time <= 0.2f) {
      areaLight.intensity = 0.4f;
      areaLight.color = _nightALColor;
    } else {
      areaLight.intensity = _normalALIntensity;
      areaLight.color = _normalALColor;
    }
    directionalLight.intensity = time;
    directionalLight.transform.rotation = Quaternion.AngleAxis((time * 180 - 90), Vector3.right);
    _rend.material.SetFloat("_DayNightTime", time);
  }
}