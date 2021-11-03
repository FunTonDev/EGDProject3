using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameMan;
    private InputManager inputMan;
    private TransitionManager tranMan;
    private AudioSource playerAudS; 
    private GameObject secondaryAxis;
    private List<GameObject> genreCosmetics;
    [HideInInspector] public Rigidbody playerRigB;
    [HideInInspector] public MeshFilter playerMeshF;
    [HideInInspector] public MeshRenderer playerMeshR;
    [HideInInspector] public GameObject closestTile;

    [Header("Components")]
    public GameObject bulletPrefab;
    public GameObject deathPrefab;
    public List<AudioClip> playerClips;

    [Header("General Vars")]
    public float currentHP;
    public float maxHP;

    [Header("General Movement")]
    public float xForce;
    public float yForce;
    public float maxXVelocity;
    public float maxYVelocity;

    [Header("General Timers")]
    public float toggleTimer;
    public float toggleDelayTime;

    [Header("Platformer Movement")]
    public float jumpForce;
    public float dashForce;
    public float maxJumpVelocity;
    public float maxFallVelocity;

    [Header("Platformer Trackers")]
    public int jumpCount;
    public int maxJumps;
    public int wallJumpCount;
    public int maxWallJumps;
    public int dashCount;
    public int maxDashes;
    private Vector3 wallNorm;

    [Header("Platformer Timers")]
    public float jumpTimer;
    public float jumpDelayTime;
    public float wallJumpTimer;
    public float wallJumpDelayTime;
    public float dashTimer;
    public float dashDelayTime;

    [Header("Platformer Data")]
    public bool grounded;
    public bool walled;
    public LayerMask groundingMask;

    [Header("Shooter Timers")]
    public float shootTimer;
    public float shootDelayTime;
    public float rollTimer;
    public float rollDelayTime;

    [Header("Genre-Control Data")]
    public States.GameGenre playerPrimaryGenre;
    public States.GameGenre playerSubGenre;
    public delegate void ControlDelegate();
    public ControlDelegate controlDel;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
        playerAudS = GetComponent<AudioSource>();
        playerRigB = GetComponent<Rigidbody>();
        playerMeshF = gameObject.transform.GetChild(0).GetComponent<MeshFilter>();
        playerMeshR = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        secondaryAxis = gameObject.transform.GetChild(1).gameObject;
        genreCosmetics = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.GetChild(2).childCount; i++)
        {
            genreCosmetics.Add(gameObject.transform.GetChild(2).GetChild(i).gameObject);
        }
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
        if (!gameMan.paused && !gameMan.dead && controlDel != null) { controlDel(); }
        //Delegate uses current genre control schema
        //Actions currently use keyhold value(not KEYDOWN) since input may get dropped, might revise later
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

    /*============================================================================
     * MOVEMENT UPDATE METHODS
     ============================================================================*/
    public void PlatformerMoveUpdate()
    {
        if (!grounded)  //Airborne check
        {
            playerRigB.AddForce(Physics.gravity, ForceMode.Force);
            if (playerRigB.velocity.y < maxFallVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxFallVelocity, playerRigB.velocity.z); }
            else if (playerRigB.velocity.y > maxJumpVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxJumpVelocity, playerRigB.velocity.z); }
        }

        if (inputMan.inputAct4 == 1 && jumpTimer <= 0) //Jump check
        {
            if (!grounded && walled && wallJumpCount < maxWallJumps)
            {
                playerAudS.PlayOneShot(playerClips[2]);
                wallJumpCount++;
                Vector3 jumpDir = (wallNorm + transform.up).normalized;
                playerRigB.AddForce(jumpDir * 50000.0f, ForceMode.Force);
                wallJumpTimer = wallJumpDelayTime;
                jumpTimer = jumpDelayTime;
            }
            else if (jumpCount < maxJumps)
            {
                playerAudS.PlayOneShot(playerClips[2]);
                jumpCount++;
                playerRigB.AddForce(new Vector3(0, jumpForce * jumpCount, 0), ForceMode.Impulse);
                jumpTimer = jumpDelayTime;
            }
        }

        //Dash Stuff
        if (inputMan.inputAct3 == 1 && dashCount < 1)
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * dashForce, 0, 0), ForceMode.VelocityChange);
            dashCount += 1;
            dashDelayTime = 1.0f;
        }
        if (dashDelayTime > 0) dashDelayTime -= Time.deltaTime;
        else if (dashDelayTime <= 0)
        {
            dashCount = 0;
            dashDelayTime = 0;
        }

        if (wallJumpTimer <= 0)    //X move check
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        }

        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    public void ShooterMoveUpdate()
    {
        playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        playerRigB.AddForce(new Vector3(0, 0, inputMan.inputY * yForce), ForceMode.VelocityChange);
        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);  //X move check
        float yClampVel = (inputMan.inputY == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.z), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.z);  //Y move check
        if (xClampVel != 0 && yClampVel != 0)
        {
            float newVel = Mathf.Sqrt(Mathf.Pow(maxXVelocity, 2) / 2);
            xClampVel = newVel * Mathf.Sign(xClampVel);
            yClampVel = newVel * Mathf.Sign(yClampVel);
        }
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, yClampVel);

        if (inputMan.inputAct7 == 1 && rollTimer <= 0) //Roll check
        {

            Debug.Log("ROLL");
            rollTimer = rollDelayTime;
        }
        //float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        //playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    public void RPGMoveUpdate()
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
            transform.position = closestTile.transform.position;//.new Vector3(position.x, transform.position.y, closestTile.transform.position.z);
        }
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, yClampVel);
    }

    public void HubMoveUpdate()
    {
        playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        playerRigB.AddForce(new Vector3(0, inputMan.inputY * yForce, 0), ForceMode.VelocityChange);
        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);  //X move check
        float yClampVel = (inputMan.inputY == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.y), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.y);  //Y move check
        playerRigB.velocity = new Vector3(xClampVel, yClampVel, playerRigB.velocity.z);
    }

    private void CursorMoveUpdate()
    {
        float aimAngle = ((Mathf.Atan2(inputMan.inputMY - Screen.height / 2, inputMan.inputMX - Screen.width / 2) * Mathf.Rad2Deg) + 360) % 360;
        secondaryAxis.transform.localEulerAngles = (playerPrimaryGenre == States.GameGenre.Platformer) ? new Vector3(-aimAngle, 90, -90) : new Vector3(0, -aimAngle + 90, 0);
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void FireUpdate()
    {
        if (inputMan.inputFire1 == 1 && shootTimer <= 0 && playerPrimaryGenre == States.GameGenre.Shooter)
        {
            playerAudS.PlayOneShot(playerClips[4]);
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

    public void GenreCosmeticUpdate(int index)
    {
        foreach (GameObject g in genreCosmetics) { g.SetActive(false); }
        if (index > -1 && index < genreCosmetics.Count) { genreCosmetics[index].SetActive(true); }
    }

    public float HealthUpdate(float change)
    {
        currentHP += change;
        if (currentHP <=  0 && !gameMan.dead)
        {
            currentHP = 0;
            gameMan.dead = true;
            playerRigB.constraints = RigidbodyConstraints.FreezeAll;
            playerMeshF.mesh = null;
            GenreCosmeticUpdate(-1);
            playerAudS.PlayOneShot(playerClips[6]);
            Instantiate(deathPrefab, transform.position, transform.rotation);
        }
        return currentHP;
    }

    /*============================================================================
     * COLLISION METHODS
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

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        maxHP = 10.0f;
        currentHP = maxHP;
        
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
    }
}