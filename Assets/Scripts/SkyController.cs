using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour {
  public float rotationAngle = 1f;
  private Renderer _rend;

  private void Start() {
    _rend = GetComponent<Renderer>();
  }

  void Update() {
    transform.RotateAround(Vector3.zero, Vector3.right, rotationAngle * Time.deltaTime);
    _rend.material.SetFloat("_DayNightTime", TimeManager.Instance ? TimeManager.Instance.time : 0f);
  }
}