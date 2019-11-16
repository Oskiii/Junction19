using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Selectable _selectable;
    private Vector3 mOffset;

    private bool _allowDragging => _selectable.IsSelected;

    private void Start()
    {
        _selectable = GetComponentInChildren<Selectable>();
    }

    private void OnMouseDown()
    {
        mOffset = transform.position - _selectable.GetMouseAsWorldPoint();
    }

    private void OnMouseDrag()
    {
        transform.position = _selectable.GetMouseAsWorldPoint() + mOffset;
    }
}