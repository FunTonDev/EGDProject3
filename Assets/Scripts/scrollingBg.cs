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
    public bool isStaticObject;
    public bool LoopingX;
    [Range(0, 1)] public float xWeight;
    [Range(0, 1)] public float yWeight;
    

    private void Start()
    {
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        playerCont = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>();
        bgObjs = new List<GameObject>();
        CloneBG(new Vector3(21.25f, 0, 0));
        CloneBG(new Vector3(-21.25f, 0, 0));
    }

    private void FixedUpdate()
    {
        //transform.position = new Vector3(playerCont.gameObject.transform.position.x, playerCont.gameObject.transform.position.y, transform.position.z);
        MoveBGGroup();
    }

    private void MoveBGGroup()
    {
        float velX = -playerCont.playerRigB.velocity.x * xWeight;
        float velY = -playerCont.playerRigB.velocity.y * yWeight;
        foreach (GameObject g in bgObjs)
        {
            g.GetComponent<Rigidbody>().velocity = new Vector3(velX, velY, 0);
            /*if (!g.GetComponent<MeshRenderer>().isVisible)
            {
                Debug.Log(g.name + " NOT VISIBLE");
                //Destroy(g);
            }*/
        }
    }

    private void CloneBG(Vector3 startPos = default(Vector3))
    {
        GameObject newBG = Instantiate(bgPrefab, transform);
        newBG.transform.localPosition = startPos;
        newBG.GetComponent<MeshRenderer>().material = bgMat;
        bgObjs.Add(newBG);
        Debug.Log(newBG.GetComponent<MeshRenderer>().bounds);
    }
}
