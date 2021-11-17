using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBg : MonoBehaviour
{
    [Header("Components")]
    public GameObject target;

    [Header("Variables")] 
    [Range(0, 1)] public float parallaxX;
    [Range(0, 1)] public float parallaxY;

    private float pivotX;
    private float pivotY;
    private float bgLen;

    private void Start()
    {
        target = GameObject.Find("PlayerPrefab");
        pivotX = transform.position.x;
        pivotY = transform.position.y;
        bgLen = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float xLoop = target.transform.position.x * (1-parallaxX);

        float xDist = target.transform.position.x * parallaxX;
        float yDist = target.transform.position.y * parallaxY;

        transform.position = new Vector3(pivotX + xDist, pivotY + yDist, transform.position.z);

        if (xLoop > pivotX + bgLen) { pivotX += bgLen; }
        else if (xLoop < pivotX - bgLen) { pivotX -= bgLen; }
    }
}
