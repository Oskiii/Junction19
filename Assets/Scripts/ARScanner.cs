using System;
using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class ARScanner : MonoBehaviour
{
    public List<Transform> CloudAnchors;
    public event Action<Transform> CloudAnchorCreated;
    [SerializeField]
    private string idToLoad = "ua-72534c9bd7f327a513597f1354706d75";

    [SerializeField]
    private GameObject HostedPointPrefab;
    [SerializeField]
    private GameObject ResolvedPointPrefab;
    [SerializeField]
    private ARReferencePointManager ReferencePointManager;
    [SerializeField]
    private ARRaycastManager RaycastManager;
    [SerializeField]
    private Canvas _scanningCanvas;
    [SerializeField]
    private Text OutputText;

    private enum AppMode
    {
        // Wait for user to tap screen to begin hosting a point.
        TouchToHostCloudReferencePoint,
        // Poll hosted point state until it is ready to use.
        WaitingForHostedReferencePoint,
        // Wait for user to tap screen to begin resolving the point.
        TouchToResolveCloudReferencePoint,
        // Poll resolving point state until it is ready to use.
        WaitingForResolvedReferencePoint,
    }
    private AppMode m_AppMode = AppMode.TouchToHostCloudReferencePoint;
    private ARCloudReferencePoint m_CloudReferencePoint;
    private string m_CloudReferenceId;
    private List<GameObject> _debugAnchorObjects = new List<GameObject>();

    public static ARScanner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadPointFromIdInTime(idToLoad, 5f));
    }

    public void StopScanningAndHide()
    {
        gameObject.SetActive(false);
        _scanningCanvas.gameObject.SetActive(false);

        foreach (var item in _debugAnchorObjects)
        {
            Destroy(item.gameObject);
        }
    }

    private IEnumerator LoadPointFromIdInTime(string id, float time)
    {
        yield return new WaitForSeconds(time);

        LoadPointFromId(id);
    }

    private void LoadPointFromId(string id)
    {
        m_CloudReferenceId = string.Empty;
        m_CloudReferencePoint =
            ReferencePointManager.ResolveCloudReferenceId(id);
        if (m_CloudReferencePoint == null)
        {
            OutputText.text = "Resolve Failed!";
            m_AppMode = AppMode.TouchToHostCloudReferencePoint;
            return;
        }
        // Wait for the reference point to be ready.
        m_AppMode = AppMode.WaitingForResolvedReferencePoint;
    }

    private void Update()
    {
        if (m_AppMode == AppMode.TouchToHostCloudReferencePoint)
        {
            Update_TouchToHostCloudReferencePoint();
        }
        else if (m_AppMode == AppMode.WaitingForHostedReferencePoint)
        {
            Update_WaitingForHostedReferencePoint();
        }
        else if (m_AppMode == AppMode.TouchToResolveCloudReferencePoint)
        {
            Update_TouchToResolveCloudReferencePoint();
        }
        else if (m_AppMode == AppMode.WaitingForResolvedReferencePoint)
        {
            Update_WaitingForResolvedReferencePoint();
        }
    }

    private void Update_TouchToHostCloudReferencePoint()
    {
        OutputText.text = "Scan area and tap to place anchor.";

        if (Input.touchCount >= 1 &&
            Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(
                Input.GetTouch(0).fingerId))
        {
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            RaycastManager.Raycast(Input.GetTouch(0).position, hitResults);
            if (hitResults.Count > 0)
            {
                Pose pose = hitResults[0].pose;
                // Create a reference point at the touch.
                ARReferencePoint referencePoint =
                    ReferencePointManager.AddReferencePoint(
                        hitResults[0].pose);
                // Create Cloud Reference Point.
                m_CloudReferencePoint =
                    ReferencePointManager.AddCloudReferencePoint(
                        referencePoint);
                if (m_CloudReferencePoint == null)
                {
                    OutputText.text = "Create Failed!";
                    return;
                }
                // Wait for the reference point to be ready.
                m_AppMode = AppMode.WaitingForHostedReferencePoint;
            }
        }
    }

    private void Update_WaitingForHostedReferencePoint()
    {
        OutputText.text = "Waiting for server";
        CloudReferenceState cloudReferenceState =
            m_CloudReferencePoint.cloudReferenceState;
        OutputText.text += " - " + cloudReferenceState.ToString();

        if (cloudReferenceState == CloudReferenceState.Success)
        {
            GameObject cloudAnchor = Instantiate(
                HostedPointPrefab,
                Vector3.zero,
                Quaternion.identity);
            cloudAnchor.transform.SetParent(
                m_CloudReferencePoint.transform, false);

            _debugAnchorObjects.Add(cloudAnchor);

            m_CloudReferenceId = m_CloudReferencePoint.cloudReferenceId;
            m_CloudReferencePoint = null;
            m_AppMode = AppMode.TouchToResolveCloudReferencePoint;
        }
    }

    private void Update_TouchToResolveCloudReferencePoint()
    {
        OutputText.text = "Created anchor. Scan more and tap again to resolve.";
        if (Input.touchCount >= 1 &&
            Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(
                Input.GetTouch(0).fingerId))
        {
            m_CloudReferencePoint =
                ReferencePointManager.ResolveCloudReferenceId(
                    m_CloudReferenceId);
            if (m_CloudReferencePoint == null)
            {
                OutputText.text = "Resolve Failed!";
                m_CloudReferenceId = string.Empty;
                m_AppMode = AppMode.TouchToHostCloudReferencePoint;
                return;
            }
            m_CloudReferenceId = string.Empty;
            // Wait for the reference point to be ready.
            m_AppMode = AppMode.WaitingForResolvedReferencePoint;
        }
    }

    private void Update_WaitingForResolvedReferencePoint()
    {
        OutputText.text = "Waiting for server to resolve";
        CloudReferenceState cloudReferenceState =
            m_CloudReferencePoint.cloudReferenceState;
        OutputText.text += " - " + cloudReferenceState.ToString();

        if (cloudReferenceState == CloudReferenceState.Success)
        {
            CloudAnchors.Add(m_CloudReferencePoint.transform);
            CloudAnchorCreated?.Invoke(m_CloudReferencePoint.transform);

            m_CloudReferencePoint = null;
            m_AppMode = AppMode.TouchToHostCloudReferencePoint;
        }
        else
        {
            print("Failed, TRYING AGAIN");
            OutputText.text = "Failed, Retrying in 5s";
            StartCoroutine(LoadPointFromIdInTime(idToLoad, 5f));
        }
    }
}