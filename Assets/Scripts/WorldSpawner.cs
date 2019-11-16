using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _worldBasePrefab;

    private void Start()
    {
        ARScanner.Instance.CloudAnchorCreated += OnCloudAnchorCreated;
    }

    private void OnDestroy()
    {
        ARScanner.Instance.CloudAnchorCreated -= OnCloudAnchorCreated;
    }

    private void OnCloudAnchorCreated(Transform t)
    {
        var obj = Instantiate(_worldBasePrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(t, false);
        print("WORLD BASE SPAWNED " + t.position);

        ARScanner.Instance.StopScanningAndHide();
    }
}