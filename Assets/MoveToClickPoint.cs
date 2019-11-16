using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    [SerializeField]
    private float _maxMoveRadius;
    [SerializeField]
    private MoveRadius _moveRadiusPrefab;
    private MoveRadius _moveRadiusInstance;

    private Selectable _selectable;

    private Tweener _moveTween;

    public Character Character;

    private void Start()
    {
        _selectable = GetComponentInChildren<Selectable>();
        _selectable.Selected += OnSelected;
        _selectable.UnSelected += OnUnSelected;

        _moveRadiusInstance = Instantiate(_moveRadiusPrefab, transform);
        _moveRadiusInstance.gameObject.SetActive(false);
    }

    private void OnSelected()
    {
        _moveRadiusInstance.gameObject.SetActive(true);
        _moveRadiusInstance.SetRadius(_maxMoveRadius);
    }

    private void OnUnSelected()
    {
        _moveRadiusInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_selectable.IsSelected == false) return;
        if (_selectable.ClickedThisFrame) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                MoveToIfInRadiusAndUnselect(hit.point);
            }
        }
    }

    private void MoveToIfInRadiusAndUnselect(Vector3 pos)
    {
        var posInWorldCoords = WorldManager.Instance.World.transform.InverseTransformPoint(transform.position);
        var targetPosInWorldCoords = WorldManager.Instance.World.transform.InverseTransformPoint(pos);
        float dist = (targetPosInWorldCoords - posInWorldCoords).magnitude;
        Debug.DrawLine(posInWorldCoords, targetPosInWorldCoords, Color.red, 5f);

        _moveTween.Complete();

        if (dist < _maxMoveRadius)
        {
            PlayerManager.Instance.LocalClient.MoveCharacter(Character.playerId, targetPosInWorldCoords, dist, (pos - transform.position).normalized);
            _selectable.UnSelect();
        }
    }
}