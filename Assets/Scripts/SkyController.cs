using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour {
  public float rotationAngle = 1f;
  public float pingPongDuration = 30f;
  public Renderer rend;

  private void Start() {
    rend = GetComponent<Renderer>();
  }

  void Update() {
    float lerp = Mathf.PingPong(Time.time, pingPongDuration) / pingPongDuration;
    transform.RotateAround(Vector3.zero, Vector3.right, rotationAngle * Time.deltaTime);
    rend.material.SetFloat("_DayNightTime", lerp);
  }
}