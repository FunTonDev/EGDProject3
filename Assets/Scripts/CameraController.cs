using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    private InputManager inputMan;
    private GameObject playerPrefab;
    private Transform cameraTarget;
    private RectTransform cursorRecT;
    private Image hpBar;
    [HideInInspector] public Camera cam;

    [Header("Variables")]
    public float mouseLeftBound;
    public float mouseRightBound;
    public float mouseUpBound;
    public float mouseDownBound;
    public float moveAheadMax;
    public float mouseAheadMax;
    public float xChange;
    public float yChange;
    public float camVelX;
    public float camVelY;

    public bool isTrackingMovement;
    public bool shaking;
    public States.GameGenre currMode;

    private Vector3 mousePos;
    private Vector3 targetPos;
    private Vector3 genrePos;
    private Quaternion targetRot;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        cursorRecT = GameObject.Find("Canvas/GamePanel/Cursor").GetComponent<RectTransform>();
        hpBar = GameObject.Find("Canvas/GamePanel/HealthBase/HealthBar").GetComponent<Image>();
        cam = GetComponent<Camera>();
        playerPrefab = GameObject.Find("PlayerPrefab");
        SetCameraTarget("PlayerPrefab");
        SyncScreen();
        SetCameraMode(States.GameGenre.Platformer);
        isTrackingMovement = true;
        shaking = false;
        moveAheadMax = 15.0f;
        mouseAheadMax = 450.0f;
        xChange = 0.0f;
        yChange = 0.0f;
        camVelX = 0.1f;
        camVelY = 0.1f;
    }

    private void Update()
    {
        cursorRecT.position = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
        PlayerController playerCont = playerPrefab.GetComponent<PlayerController>();
        hpBar.fillAmount = playerCont.currentHP / playerCont.maxHP;
        if (playerCont.shootTimer > 0 && !shaking)
        {
            StartCoroutine(CamEffectShake(0.1f, new Vector3(0.25f, 0, 0.25f)));
        }
    }

    private void FixedUpdate()
    {
        xChange = yChange = 0.0f;
        targetPos = cameraTarget.position;
        switch (cameraTarget.tag)
        {
            case "Player":
                PlayerCamUpdate();
                break;
        }
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void PlayerCamUpdate()
    {
        CursorBiasUpdate();
        //CharMoveBiasUpdate()
        Vector3 playerADSDir = ADSBiasUpdate();
        transform.position = targetPos + genrePos + playerADSDir;// + shotShake;
        transform.rotation = targetRot;
        //Incorporate averaging of mouse position, movement, character position later using x/y change //Vector3(xChange, yChange, 0);
    }
    private void CursorBiasUpdate()
    {
        Vector3 playerScreenPos = cam.WorldToScreenPoint(playerPrefab.transform.position);
        playerScreenPos = new Vector3(playerScreenPos.x, playerScreenPos.y, 0);
        Vector3 cursorScreenPos = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
        Vector3 dirVec = cursorScreenPos - playerScreenPos;
        float adjustedDist = Mathf.Clamp(dirVec.magnitude, 0, mouseAheadMax);

        Vector3 res = dirVec.normalized * adjustedDist;

        
    }

    private void CharMoveBiasUpdate()
    {
        if (isTrackingMovement)
        {
            Vector3 currVel = playerPrefab.GetComponent<Rigidbody>().velocity;
            float xDir = Mathf.Abs(currVel.x) > 0 ? currVel.x/Mathf.Abs(currVel.x) : 0;// * 0.1f);
            float yDir = Mathf.Abs(currVel.y) > 0 ? currVel.y/Mathf.Abs(currVel.y) : 0;// * 0.1f);

            Vector3 lookAheadPos = new Vector3(xDir, yDir, 0).normalized * moveAheadMax;
            if (transform.position.x != lookAheadPos.x)
            {
                xChange = xDir;// * camVelX;
                yChange = yDir;// * camVelY;
            }
        }
    }

    private Vector3 ADSBiasUpdate() //Currently a naive implementation, planning to add cursor cam
    {
        if (cameraTarget.GetComponent<PlayerController>().playerSubGenre != States.GameGenre.Shooter || inputMan.inputFire2 != 1) { return Vector3.zero; }
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(inputMan.inputMX, inputMan.inputMY, 2.0f));
        mouseWorldPos = new Vector3(mouseWorldPos.x, playerPrefab.transform.position.y, mouseWorldPos.z);
        Vector3 dirVec = (mouseWorldPos - playerPrefab.transform.position).normalized;
        float dist = Mathf.Clamp(Vector3.Distance(mouseWorldPos, playerPrefab.transform.position), 0, 1.0f) * 3.0f;
        return dirVec * dist;
    }

    /*============================================================================
     * CAMERA METHODS
     ============================================================================*/
    private void SetCameraTarget(string targetStr)
    {
        cameraTarget = GameObject.Find(targetStr).transform;
        isTrackingMovement = targetStr == "PlayerPrefab";
    }

    public void SetCameraMode(States.GameGenre mode)
    {
        currMode = mode;
        switch (currMode)
        {
            case (States.GameGenre.Shooter):
                cam.orthographic = false;
                genrePos = new Vector3(0, 10.0f, 0);
                targetRot = Quaternion.Euler(90, 0, 0);
                break;
            case (States.GameGenre.RPG):
                cam.orthographic = false;
                genrePos = new Vector3(0, 5.0f, -6.0f);
                targetRot = Quaternion.Euler(45, 0, 0);
                break;
            default:    //Applies same cam to platformer and hub; subject to change
                cam.orthographic = true;
                genrePos = new Vector3(0, 0, -8.0f);
                targetRot = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    /*============================================================================
     * COROUTINES
     ============================================================================*/
    private IEnumerator CamEffectShake(float time, Vector3 magnitude)
    {
        shaking = true;
        float timer = 0;
        float interval = 0.05f;
        Vector3 sPos = Vector3.zero;
        while (timer < time)
        {
            if (sPos == Vector3.zero)
            {
                sPos = new Vector3(Random.value * magnitude.x, Random.value * magnitude.y, Random.value * magnitude.z);
                transform.position += sPos;
                yield return new WaitForSeconds(interval);
                timer += interval;
                transform.position -= sPos;
                sPos = Vector3.zero;
            }
            else
            {
                yield return null;
                timer += Time.deltaTime;
            }
            Debug.Log(timer);
        }
        shaking = false;
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
        private void SyncScreen()
    {
        int xShift = 500, yShift = 300;
        mouseLeftBound = Screen.width/2 - xShift;
        mouseRightBound = Screen.width/2 + xShift;
        mouseUpBound = Screen.height/2 + yShift;
        mouseDownBound = Screen.height/2 - yShift;
    }
}