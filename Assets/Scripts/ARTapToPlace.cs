using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlace : MonoBehaviour
{
    [SerializeField] private GameObject homeBasePrefab;

    public event Action OnHomeBaseInstantiated;

    private GameObject homeBase;

    private ARRaycastManager arRaycastManager;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isOverUI;

    private void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = touch.position;

                isOverUI = isOverUIObject(touchPos);
            }

            if (!isOverUI && arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (homeBase == null)
                {
                    homeBase = Instantiate(homeBasePrefab, hitPose.position, hitPose.rotation);
                    OnHomeBaseInstantiated.Invoke();
                }
                else
                {
                    homeBase.transform.position = hitPose.position;
                }
            }
        }
    }

    private bool isOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;
    }

    public void HomeBaseLocked()
    {
        GameManager.instance.HomeBase = homeBase;
    }
}
