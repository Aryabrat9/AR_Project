using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneScanningState : IState
{
    private GameObject planeScanningCanvas;
    private GameObject enemyBase;

    private ARPlaneManager aRPlaneManager;
    private List<ARPlane> foundPlanes;

    private GameObject enemyBasePrefab;

    public PlaneScanningState(GameObject planeScanningCanvas, GameObject enemyBase) 
    {
        this.planeScanningCanvas = planeScanningCanvas;
        this.enemyBasePrefab = enemyBase;
    }
    
    public void EnterState()
    {
        Debug.Log("<<<<<<<< Enter Plane Scanning State >>>>>>>>");
        planeScanningCanvas.SetActive(true);
        aRPlaneManager = GameObject.FindObjectOfType<ARPlaneManager>();
        aRPlaneManager.planesChanged += PlanesChanged;
        foundPlanes = new List<ARPlane>();
    }

    public void ExecuteState()
    {
        Debug.Log("<<<<<<<< Execute Plane Scanning State >>>>>>>>");
    }

    private void PlanesChanged(ARPlanesChangedEventArgs data)
    {
        //Plane stuff is done here
        if (data.added != null && data.added.Count > 0)
        {
            foundPlanes.AddRange(data.added);
        }

        foreach (ARPlane plane in foundPlanes.Where(plane => plane.extents.x * plane.extents.y >= 0.3f))
        {
            if (plane.alignment.IsVertical() && enemyBase == null)
            {
                enemyBase = GameObject.Instantiate(enemyBasePrefab);
                enemyBase.transform.position = plane.center;
                enemyBase.transform.forward = plane.normal;

                GameManager.instance.EnemyBase = enemyBase;
                GameManager.instance.StartImageScanning();
            }
        }
    }

    public void ExitState()
    {
        Debug.Log("<<<<<<<< Exit Plane Scanning State >>>>>>>>");
        planeScanningCanvas.SetActive(false);
        aRPlaneManager.planesChanged -= PlanesChanged;
    }
}
