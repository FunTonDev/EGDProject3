using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameManager gameMan;
    [SerializeField] private InputManager inputMan;
    [SerializeField] private GameMenuManager gameMMan;
    [SerializeField] private TransitionManager tranMan;
    [SerializeField] private CameraController camCont;
    [SerializeField] private RectTransform cursorRecT;
    [SerializeField] private Rigidbody playerRigB;
    [SerializeField] private BoxCollider playerBoxC;
    [SerializeField] private AudioSource playerAudS;
    [SerializeField] private AudioClip playerJumpClip;
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private GameObject secondaryAxis;
    public GameObject bulletPrefab;
    public List<AudioClip> playerClips;
    public Mesh SpriteMesh;
    public Mesh ModelMesh;

    [Header("Variables")]
    public float currentHP;
    public float maxHP;

    public float xForce;
    public float yForce;
    public float jumpForce;
    public float dashForce;

    public float maxXVelocity;
    public float maxYVelocity;
    public float maxJumpVelocity;
    public float maxFallVelocity;

    public int jumpCount;
    public int maxJumps;
    public int wallJumpCount;
    public int maxWallJumps;
    public int dashCount;
    public int maxDashes;
    
    public float jumpTimer;
    public float jumpDelayTime;
    public float wallJumpTimer;
    public float wallJumpDelayTime;
    public float dashTimer;
    public float dashDelayTime;
    public float shootTimer;
    public float shootDelayTime;
    public float toggleTimer;
    public float toggleDelayTime;   //1.0
    public float rollTimer;
    public float rollDelayTime;

    public bool grounded;
    public bool walled;
    public bool canClimb;
    public bool climbing;

    public Vector3 wallNorm;
    public LayerMask groundingMask;

    public States.GameGenre playerGenre;
    private delegate void ControlDelegate();
    private ControlDelegate controlDel;

    private GameObject closestTile;
    private float RPGTileSize;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        RPGTileSize = 10.0f;
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        gameMMan = GameObject.Find("[MANAGER]").GetComponent<GameMenuManager>();
        tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
        cursorRecT = GameObject.Find("Canvas/GamePanel/Cursor").GetComponent<RectTransform>();
        camCont = GameObject.Find("Main Camera").GetComponent<CameraController>();
        playerRigB = GetComponent<Rigidbody>();
        playerBoxC = GetComponent<BoxCollider>();
        playerAudS = GetComponent<AudioSource>();
        //playerJumpClip = Resources.Load<AudioClip>("Audio/Sound_Effects/8_BIT_[50_SFX]_Jump_Free_Sound_Effects_N1_BY_jalastram/SFX_Jump_05");
        //playerShootClip = (AudioClip)Resources.Load("Audio/Sound_Effects/Fire_5");
        secondaryAxis = gameObject.transform.Find("SecondaryAxis").gameObject;
    }

    private void Update()
    {
        if (!gameMan.paused)
        {
            CursorMoveUpdate();
            FireUpdate();
            TimerUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!gameMan.paused && controlDel != null) { controlDel(); } //Delegate uses current genre control schema
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = GroundCheck();
        walled = WallCheck();
        jumpCount = grounded ? 0 : jumpCount;
        wallJumpCount = grounded ? 0 : wallJumpCount;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = GroundCheck();
        walled = WallCheck();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        TriggerCheck(trigger, true);
    }

    private void OnTriggerStay(Collider trigger)
    {
        TriggerCheck(trigger, false);
    }

    private void OnTriggerExit(Collider trigger)
    {
        TriggerCheck(trigger, false);
    }

    /*============================================================================
     * MOVEMENT UPDATE METHODS
     ============================================================================*/
    private void PlatformerMoveUpdate()
    {
        if (!grounded && !climbing)  //Airborne check
        {
            playerRigB.AddForce(Physics.gravity, ForceMode.Force);
            if (playerRigB.velocity.y < maxFallVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxFallVelocity, playerRigB.velocity.z); }
            else if (playerRigB.velocity.y > maxJumpVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxJumpVelocity, playerRigB.velocity.z); }
        }

        if (inputMan.inputAlt1 == 1 && jumpTimer <= 0) //Jump check
        {
            if (walled && !grounded && wallJumpCount < maxWallJumps)
            {
                playerAudS.PlayOneShot(playerJumpClip);
                wallJumpCount++;
                Vector3 jumpDir = (wallNorm + transform.up).normalized;
                playerRigB.AddForce(jumpDir * 50000.0f, ForceMode.Force);
                wallJumpTimer = wallJumpDelayTime;
                jumpTimer = jumpDelayTime;
            }
            else if (jumpCount < maxJumps)
            {
                playerAudS.PlayOneShot(playerJumpClip);
                jumpCount++;
                playerRigB.AddForce(new Vector3(0, jumpForce * jumpCount, 0), ForceMode.Impulse);
                jumpTimer = jumpDelayTime;
            }
        }

        if (wallJumpTimer <= 0)    //X move check
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        }
        if (canClimb)       //Y move check
        {
            //WORK ON LATER
            //playerRigB.AddForce(new Vector3(0, inputMan.inputY * yForce, 0), ForceMode.VelocityChange);
            //float yClampVel = inputMan.inputY == 0 ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.y), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.y);
            //playerRigB.velocity = new Vector3(playerRigB.velocity.x, yClampVel, playerRigB.velocity.z);
        }
        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    private void ShooterMoveUpdate()
    {
        playerRigB.AddForce(new Vector3(0, 0, inputMan.inputY * yForce), ForceMode.VelocityChange);
        playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);  //X move check
        float yClampVel = (inputMan.inputY == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.z), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.z);  //Y move check
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, yClampVel);

        if (inputMan.inputAlt2 == 1 && rollTimer <= 0) //Roll check
        {
            Debug.Log("ROLL");
        }
        //float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        //playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    private void RPGMoveUpdate()
    {
        float xClampVel = 0, yClampVel = 0;
        if (inputMan.inputX != 0)
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * xForce / 2, 0, 0), ForceMode.VelocityChange);
            xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);  //X move check
        }
        else if (inputMan.inputY != 0)
        {
            playerRigB.AddForce(new Vector3(0, 0, inputMan.inputY * yForce / 2), ForceMode.VelocityChange);
            yClampVel = (inputMan.inputY == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.z), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.z);  //Y move check
        }
        else if (closestTile != null)
        {
            transform.position = new Vector3(closestTile.transform.position.x, transform.position.y, closestTile.transform.position.z);
        }
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, yClampVel);
    }

    private void CursorMoveUpdate()
    {
        cursorRecT.position = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
        float aimAngle = ((Mathf.Atan2(inputMan.inputMY - Screen.height / 2, inputMan.inputMX - Screen.width / 2) * Mathf.Rad2Deg) + 360) % 360;
        secondaryAxis.transform.localEulerAngles = (playerGenre == States.GameGenre.Platformer) ? new Vector3(-aimAngle, 90, -90) : new Vector3(0, -aimAngle + 90, 0);
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void FireUpdate()
    {
        if (inputMan.inputFire1 == 1 && shootTimer <= 0 && playerGenre == States.GameGenre.Shooter)
        {
            playerAudS.PlayOneShot(playerShootClip);
            Instantiate(bulletPrefab, transform.position, secondaryAxis.transform.rotation);
            shootTimer = shootDelayTime;
        }
    }

    private void TimerUpdate()
    {
        jumpTimer -= jumpTimer > 0 ? Time.deltaTime : 0;
        wallJumpTimer -= wallJumpTimer > 0 ? Time.deltaTime : 0;
        dashTimer -= dashTimer > 0 ? Time.deltaTime : 0;
        shootTimer -= shootTimer > 0 ? Time.deltaTime : 0;
        rollTimer -= rollTimer > 0 ? Time.deltaTime : 0;
        toggleTimer -= toggleTimer > 0 ? Time.deltaTime : 0;
    }

    public float ChangeHealth(float change)
    {
        currentHP += change;
        return currentHP;
    }

    /*============================================================================
     * COLLISION CHECK METHODS
     ============================================================================*/
    private bool GroundCheck()
    {
        Vector3 rightPos = transform.position + new Vector3(0.275f, 0, 0);
        Vector3 leftPos = transform.position + new Vector3(-0.275f, 0, 0);

        bool castContact = false;
        castContact = castContact | Physics.Raycast(rightPos, -transform.up, 0.4f, groundingMask);  //RIGHT CAST
        castContact = castContact | Physics.Raycast(leftPos, -transform.up, 0.4f, groundingMask);   //LEFT CAST
        return castContact;
    }

    private bool WallCheck()
    {
        wallNorm = Vector3.zero;
        float closestDist = 100.0f;
        Vector3[] sides = { transform.position + new Vector3(0, 0.275f, 0), transform.position + new Vector3(0, -0.275f, 0) };
        RaycastHit hit;

        foreach (Vector3 v in sides)
        {
            if (Physics.Raycast(v, transform.right, out hit, 0.4f, groundingMask))
            {
                if (hit.distance < closestDist)
                {
                    closestDist = hit.distance;
                    wallNorm = hit.normal;
                }
            }
            if (Physics.Raycast(v, -transform.right, out hit, 0.4f, groundingMask))
            {
                if (hit.distance < closestDist)
                {
                    closestDist = hit.distance;
                    wallNorm = hit.normal;
                }
            }
        }
        return wallNorm != Vector3.zero;
    }

    private void TriggerCheck(Collider trigger, bool entered)
    {
        switch (trigger.gameObject.tag)
        {
            case "Control":
                if (entered)
                {
                    switch (trigger.gameObject.name[trigger.gameObject.name.Length - 1])
                    {
                        case 'P':
                            SetPlayerMode(States.GameGenre.Platformer);
                            break;
                        case 'S':
                            SetPlayerMode(States.GameGenre.Shooter);
                            break;
                        case 'R':
                            SetPlayerMode(States.GameGenre.RPG);
                            break;
                    }
                }
                break;
            case "Grid":
                if (closestTile == null || Vector3.Distance(transform.position, trigger.transform.position) < Vector3.Distance(transform.position, closestTile.transform.position))
                {
                    closestTile = trigger.gameObject;
                }
                break;
            case "TransitionArea":
                tranMan.SceneSwitch(trigger.gameObject.name.Substring(10));
                break;
            case "Climbable":
                canClimb = entered;
                break;
        }
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        currentHP = 6.0f;
        maxHP = 10.0f;

        xForce = 4.0f;
        yForce = 4.0f;
        jumpForce = 100.0f;
        dashForce = 80.0f;

        maxXVelocity = 6.0f;
        maxYVelocity = 6.0f;
        maxJumpVelocity = 6.0f;
        maxFallVelocity = -12.0f;

        jumpCount = 0;
        maxJumps = 2;
        wallJumpCount = 0;
        maxWallJumps = 6;
        dashCount = 0;
        maxDashes = 1;

        jumpTimer = 0;
        jumpDelayTime = 0.5f;
        wallJumpTimer = 0;
        wallJumpDelayTime = 0.2f;
        dashTimer = 0;
        dashDelayTime = 1.0f;
        shootTimer = 0;        
        shootDelayTime = 0.5f;
        toggleTimer = 0;
        toggleDelayTime = 1.0f;
        rollTimer = 0;
        rollDelayTime = 2.0f;

        grounded = false;
        walled = false;
        canClimb = false;
        climbing = false;
    }

    public void SetPlayerMode(States.GameGenre mode)
    {
        switch(mode)
        {
            case States.GameGenre.Platformer:
                controlDel = PlatformerMoveUpdate;
                playerRigB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                break;
            case States.GameGenre.Shooter:
                controlDel = ShooterMoveUpdate;
                playerRigB.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
            case States.GameGenre.RPG:
                controlDel = RPGMoveUpdate;
                playerRigB.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
        }
        camCont.SetCameraMode(mode);
        playerGenre = mode;
    }
}