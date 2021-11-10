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
        neXPosition = pla.transform.position.x * 0.3f;
        neYPosition = pla.transform.position.y * 0.8f;
        transform.position = startPosition + (Vector3.right * neXPosition * inputMan.inputX) + (Vector3.up * neYPosition);
        transform.rotation = startRotation;
    }
}
