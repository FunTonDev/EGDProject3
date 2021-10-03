using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject playerGamO;

    [Header("Variables")]
    public Vector3 playerPos;
    public bool isFollowingPlayer;

    private void Start()
    {
        playerGamO = gameObject;
        playerPos = playerGamO.transform.position;
        isFollowingPlayer = true;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(playerPos.x, playerPos.y + 1, -8.0f);
    }
}