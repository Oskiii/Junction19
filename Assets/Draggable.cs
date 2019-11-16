using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField]
    private LayerMask _rayCastMask;

    private Selectable _selectable;
    private Vector3 mOffset;
    private Collider _collider;

    private bool _allowDragging => _selectable.IsSelected;
    private Camera _camera;

    private void Start()
    {
        _collider = GetComponentInChildren<Collider>();
        _camera = FindObjectOfType<Camera>();
        _selectable = GetComponentInChildren<Selectable>();
    }

    private void OnMouseDown()
    {
        mOffset = transform.position - _selectable.GetMouseAsWorldPoint();
    }

    private void OnMouseDrag()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100f, _rayCastMask))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = _selectable.GetMouseAsWorldPoint() + mOffset;
        }
    }
}