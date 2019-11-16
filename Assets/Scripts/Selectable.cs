using System;
using UnityEngine;

public class Selectable : MonoBehaviour
{

  public Material highlightMaterial;
  private Material originalMaterial;
  private static Selectable selectedObject;
  private Renderer renderer;
  private Vector3 mOffset;
  private float mZCoord;

  public bool IsSelected { get; private set; }
  public bool ClickedThisFrame { get; private set; }

  public event Action Selected;
  public event Action UnSelected;

  private void Start()
  {
    renderer = GetComponent<Renderer>();
    originalMaterial = renderer.material;
  }

  private void Update()
  {
    // Reset the selectedObject
    if (Input.GetKeyDown(KeyCode.R))
    {
      ResetSelectedObject();
    }
  }

  private void LateUpdate()
  {
    ClickedThisFrame = false;
  }

  private void OnMouseDown()
  {
    if (!selectedObject || gameObject != selectedObject.gameObject)
    {
      if (selectedObject) selectedObject.UnSelect();
      Select();
    }

    mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
    mOffset = transform.position - GetMouseAsWorldPoint();
  }

  private Vector3 GetMouseAsWorldPoint()
  {

    // Pixel coordinates of mouse (x,y)
    Vector3 mousePoint = Input.mousePosition;

    // z coordinate of game object on screen
    mousePoint.z = mZCoord;

    // Convert it to world points
    return Camera.main.ScreenToWorldPoint(mousePoint);

  }

  private void ResetSelectedObject()
  {
    if (selectedObject) selectedObject.UnSelect();
  }

  private void OnMouseDrag()
  {
    if (selectedObject && selectedObject.gameObject == gameObject)
      transform.position = GetMouseAsWorldPoint() + mOffset;
  }

  public void Select()
  {
    IsSelected = true;
    ClickedThisFrame = true;
    renderer.material = highlightMaterial;
    Selected?.Invoke();
  }

  public void UnSelect()
  {
    IsSelected = false;
    selectedObject = null;
    renderer.material = originalMaterial;
    UnSelected?.Invoke();
  }
}