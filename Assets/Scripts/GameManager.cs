using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject enemyBase;
    private GameObject homeBase;

    public GameObject HomeBase { get => homeBase; set{homeBase = value; StartMainGame();}}


    public GameObject EnemyBase { get => enemyBase; set{enemyBase = value; StartImageScanning();}}

    [Header("Main Game References")]
    [SerializeField] private GameObject mainMenuCanvas;

    [Header("PlaneScanning References")]
    [SerializeField] private GameObject planeScanningCanvas;
    [SerializeField] private GameObject enemyBasePrefab;

    [Header("ImageScanning References")]
    [SerializeField] private GameObject imageScanningCanvas;


    private StateMachine stateMachine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
        }
    }

    private void Start() 
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new MainMenuState(mainMenuCanvas));
        stateMachine.ExecuteStateUpdate();
    }

    public void StartPlaneScanning() 
    {
        stateMachine.ChangeState(new PlaneScanningState(planeScanningCanvas, enemyBasePrefab));
    }

    public void StartImageScanning()
    {
        stateMachine.ChangeState(new ImageScanningState(imageScanningCanvas));
        stateMachine.ExecuteStateUpdate();
    }

    private void StartMainGame()
    {
        
    }

}