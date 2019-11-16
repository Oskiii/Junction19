using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MoveMonsterToClickPoint : MonoBehaviour
{
    [SerializeField]
    private float _maxMoveRadius;
    [SerializeField]
    private MoveRadius _moveRadiusPrefab;
    private MoveRadius _moveRadiusInstance;

    private Selectable _selectable;

    private Tweener _moveTween;

    private MonsterContainer _monsterContainer;

    private void Start()
    {
        _selectable = GetComponentInChildren<Selectable>();
        _selectable.Selected += OnSelected;
        _selectable.UnSelected += OnUnSelected;

        _moveRadiusInstance = Instantiate(_moveRadiusPrefab, transform);
        _moveRadiusInstance.gameObject.SetActive(false);
        _monsterContainer = GetComponent<MonsterContainer>();
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
        var localWorldPosition = WorldManager.Instance.World.transform.InverseTransformPoint(transform.position);
        var posWorldPosition = WorldManager.Instance.World.transform.InverseTransformPoint(pos);
        float dist = (localWorldPosition - posWorldPosition).magnitude;
        Debug.DrawLine(localWorldPosition, posWorldPosition, Color.red, 5f);

        _moveTween.Complete();

        if (dist < _maxMoveRadius)
        {
            MonsterManager.Instance.MoveMonster(_monsterContainer.Monster.monsterId, posWorldPosition, dist, (pos - transform.position).normalized);
            _selectable.UnSelect();
        }
    }
}