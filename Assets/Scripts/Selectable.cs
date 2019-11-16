using UnityEngine;

public class Selectable : MonoBehaviour {

  public Material highlightMaterial;
  private Material originalMaterial;
  private static Selectable selectedObject;
  private Renderer renderer;
  private Vector3 mOffset;
  private float mZCoord;

  private void Start() {
    renderer = GetComponentInChildren<Renderer>();
    originalMaterial = renderer.material;
  }

  private void Update() {
    // Reset the selectedObject
    if (Input.GetKeyDown(KeyCode.R)) {
      ResetSelectedObject();
    }
  }

  private void OnMouseDown() {
    if (!selectedObject || gameObject != selectedObject.gameObject) {
      if (selectedObject) selectedObject.UnSelect();
      selectedObject = this;
      Select();
    }

    mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
    mOffset = transform.position - GetMouseAsWorldPoint();
  }

  private Vector3 GetMouseAsWorldPoint() {

    // Pixel coordinates of mouse (x,y)

    Vector3 mousePoint = Input.mousePosition;

    // z coordinate of game object on screen

    mousePoint.z = mZCoord;

    // Convert it to world points

    return Camera.main.ScreenToWorldPoint(mousePoint);

  }

  private void ResetSelectedObject() {
    if (selectedObject) selectedObject.UnSelect();
    selectedObject = null;
  }

  private void OnMouseDrag() {
    if (selectedObject && selectedObject.gameObject == gameObject)
      transform.position = GetMouseAsWorldPoint() + mOffset;
  }

  public void Select() {
    renderer.material = highlightMaterial;
  }

  public void UnSelect() {
    renderer.material = originalMaterial;
  }
}