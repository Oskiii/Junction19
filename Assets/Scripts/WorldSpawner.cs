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
        WorldManager.Instance.SpawnWorld();
        var obj = WorldManager.Instance.World;
        obj.transform.SetParent(t, false);
        obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        ARScanner.Instance.StopScanningAndHide();
    }
}