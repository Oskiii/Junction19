using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderListener : MonoBehaviour {
  private Slider slider;
  private void Start() {
    slider = GetComponent<Slider>();
    slider.onValueChanged.AddListener(UpdateTime);
  }

  private void UpdateTime(float time) {
    if (TimeManager.Instance != null) TimeManager.Instance.SetTime(time);
  }
}