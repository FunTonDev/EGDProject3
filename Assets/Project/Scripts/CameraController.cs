using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private GameObject playerPrefab;

    [Header("Variables")]
    public Vector3 targetPos;
    public bool isTrackingMovement;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float xDiff = 0.0f, yDiff = 0.0f, scale = 2.0f;
        if (isTrackingMovement)
        {
            
            Vector3 currVel = playerPrefab.GetComponent<Rigidbody>().velocity;
            xDiff = currVel.x == 0 ? 0 : Mathf.Round(currVel.x / Mathf.Abs(currVel.x) * 0.1f);
            yDiff = currVel.y == 0 ? 0 : Mathf.Round(currVel.y / Mathf.Abs(currVel.y) * 0.1f);
        }
        targetPos = cameraTarget.position;
        Vector3 newPos = new Vector3(targetPos.x, targetPos.y, -8.0f) + new Vector3(xDiff, yDiff, 0) * scale;
        transform.position = newPos;
    }

    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        playerPrefab = GameObject.Find("PlayerPrefab");
        SetCameraTarget("PlayerPrefab");
    }

    private void SetCameraTarget(string targetStr)
    {
        cameraTarget = GameObject.Find(targetStr).transform;
        isTrackingMovement = targetStr == "PlayerPrefab";
    }
}