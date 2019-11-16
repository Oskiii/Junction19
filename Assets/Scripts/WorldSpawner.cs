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
        var obj = WorldManager.Instance.SpawnWorld();//Instantiate(_worldBasePrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(t, false);
        obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        ARScanner.Instance.StopScanningAndHide();
    }
}