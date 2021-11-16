using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBg : MonoBehaviour
{
    private InputManager inputMan;
    private PlayerController playerCont;

    [Header("Components")]
    public Material bgMat;
    public GameObject bgPrefab;
    private List<GameObject> bgObjs;

    [Header("Variables")]
    public GameObject cam;
    float startX, startY, bgLen;
    [Range(0, 1)] public float parallaxX;
    [Range(0, 1)] public float parallaxY;

    private void Start()
    {
        /*inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        playerCont = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>();
        bgObjs = new List<GameObject>();
        CloneBG(new Vector3(21.25f, 0, 0));
        CloneBG(new Vector3(-21.25f, 0, 0));*/
        startX = transform.position.x;
        startY = transform.position.y;
        bgLen = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        Debug.Log(transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size);
    }

    private void FixedUpdate()
    {
        float xLoop = cam.transform.position.x * (1-parallaxX);

        float xDist = cam.transform.position.x * parallaxX;
        float yDist = cam.transform.position.y * parallaxY;

        transform.position = new Vector3(startX + xDist, startY + yDist, transform.position.z);

        if (xLoop > startX + bgLen) { startX += bgLen; }
        else if (xLoop < startX - bgLen) { startX -= bgLen; }
    }
}
