using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBg : MonoBehaviour
{
    public InputManager inputMan;

    public PlayerController pla;

    [SerializeField]
    public float moveSpeed = 1f;

    [SerializeField]

    public float offset;

    private Vector3 startPosition;

    private Quaternion startRotation;

    private float neXPosition;
    private float neYPosition;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        pla = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>();

    }

    private void Update()
    {
        if (pla.playerRigB.velocity.x != 0)
        neXPosition = pla.transform.position.x * 0.3f;
        if (pla.playerRigB.velocity.y != 0)
        neYPosition = pla.transform.position.y * 0.8f;
        transform.position = startPosition + (Vector3.right * neXPosition * (pla.playerRigB.velocity.x)/6) + (Vector3.up * neYPosition);
        transform.rotation = startRotation;
    }
}
