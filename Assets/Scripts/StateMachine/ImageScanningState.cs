using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ImageScanningState : IState
{
    private GameObject imageScanningCanvas;
    private ARTrackedImageManager arTrackedImageManager;
    private ARTapToPlace arTapToPlace;

    private TextMeshProUGUI text;
    private Button homeBaseButton;

    public ImageScanningState(GameObject imageScanningCanvas)
    {
        this.imageScanningCanvas = imageScanningCanvas;
    }

    public void EnterState()
    {
        Debug.Log("<<<<<<<< Enter Image Scanning State >>>>>>>>");
        arTrackedImageManager = GameObject.FindObjectOfType<ARTrackedImageManager>();
        arTapToPlace = GameObject.FindObjectOfType<ARTapToPlace>();
        arTapToPlace.OnHomeBaseInstantiated += EnableButton;

        homeBaseButton = imageScanningCanvas.GetComponentInChildren<Button>();
        text = imageScanningCanvas.GetComponentInChildren<TextMeshProUGUI>();
        imageScanningCanvas.SetActive(true);
        homeBaseButton.gameObject.SetActive(false);
    }

    private void EnableButton()
    {
        homeBaseButton.gameObject.SetActive(true);
    }

    public void ExecuteState()
    {
        Debug.Log("<<<<<<<< Execute Image Scanning State >>>>>>>>");
        arTrackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    private void OnImagesChanged(ARTrackedImagesChangedEventArgs data)
    {
        foreach (var image in data.added)
        {
            Debug.Log("<<<<<<<< IMAGE FOUND >>>>>>>>");
            arTapToPlace.enabled = true;
            text.SetText("Tap to place HomeBase.");
        }
    }

    public void ExitState()
    {
        Debug.Log("<<<<<<<< Exit Image Scanning State >>>>>>>>");
        imageScanningCanvas.SetActive(false);
        arTapToPlace.enabled = false;
    }
}
