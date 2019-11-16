using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour {

  [Header ("Movement")]
  public float lookSpeedH = 2f;
  public float lookSpeedV = 2f;
  public float zoomSpeed = 2f;
  public float dragSpeed = 6f;

  public float moveSpeed = 3f;
  public float turboSpeed = 5f;

  private float yaw = 0f;
  private float pitch = 0f;

  void Update () {
    Move ();
  }

  private void Move () {
    //Look around or move with right mouse button down
    if (Input.GetMouseButton (1)) {
      yaw += lookSpeedH * Input.GetAxis ("Mouse X");
      pitch -= lookSpeedV * Input.GetAxis ("Mouse Y");
      transform.eulerAngles = new Vector3 (pitch, yaw, 0f);

      float usedSpeed = Input.GetKey (KeyCode.LeftShift) ? turboSpeed : moveSpeed;
      transform.position += transform.forward * Input.GetAxisRaw ("Vertical") * usedSpeed * Time.unscaledDeltaTime;
      transform.position += transform.right * Input.GetAxisRaw ("Horizontal") * usedSpeed * Time.unscaledDeltaTime;
    }

    //drag camera around with Middle Mouse
    if (Input.GetMouseButton (2)) {
      transform.Translate (-Input.GetAxisRaw ("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * dragSpeed, 0);
    }

    //Zoom in and out with Mouse Wheel
    transform.Translate (0, 0, Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed, Space.Self);
  }
}