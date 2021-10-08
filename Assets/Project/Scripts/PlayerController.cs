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
    [SerializeField] private RectTransform cursorRecT;
    [SerializeField] private Rigidbody playerRigB;
    [SerializeField] private BoxCollider playerBoxC;
    [SerializeField] private AudioSource playerAudS;
    [SerializeField] private Camera playerCam;
    public GameObject bulletPrefab;
    public List<AudioClip> playerClips;

    [Header("Variables")]
    public float xForce;
    public float yForce;
    public float jumpForce;

    public float maxXVelocity;
    public float maxYVelocity;
    public float maxJumpVelocity;
    public float maxFallVelocity;

    public int jumpCount;
    public int maxJumps;
    public float jumpDelayTime;
    public int wallJumpCount;
    public int maxWallJumps;
    public float wallJumpDelayTime;
    public Vector3 wallNorm;
    public int dashCount;
    public int maxDashes;
    public float shootDelayTime;

    public bool jumped;
    public bool grounded;
    public bool walled;
    public bool wallJumped;
    public bool canClimb; 
    public bool shot;
    public bool justPaused;

    LayerMask groundMask;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        gameMMan = GameObject.Find("[MANAGER]").GetComponent<GameMenuManager>();
        tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
        cursorRecT = GameObject.Find("Canvas/Cursor").GetComponent<RectTransform>();
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerRigB = GetComponent<Rigidbody>();
        playerBoxC = GetComponent<BoxCollider>();
        playerAudS = GetComponent<AudioSource>();
        groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (!gameMan.paused)
        {
            CursorMoveUpdate();
            FireUpdate();
        }
        PauseUpdate();
    }

    private void FixedUpdate()
    {
        if (!gameMan.paused)
        {
            CharMovementUpdate();   //Movement/jumping are physics based changes
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = GroundCheck();
        walled = WallCheck();
        TransitionCheck(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = GroundCheck();
        walled = WallCheck();
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void CharMovementUpdate()
    {
        if (!grounded)  //IF IS/BECOMES AIRBORNE, y jump and fall velocity must be capped
        {
            playerRigB.AddForce(Physics.gravity, ForceMode.Force);
            if (playerRigB.velocity.y < maxFallVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxFallVelocity, playerRigB.velocity.z); }
            else if (playerRigB.velocity.y > maxJumpVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxJumpVelocity, playerRigB.velocity.z); }
        }
        else            //IF IS/BECOMES GROUNDED, y movement velocity must be capped
        {
            jumpCount = 0;
                /* if (canClimb)   //Temporary but other confirmed gameplay functionality could be WIP(entering doorways, climbing ladders, looking up, crouch, etc)
                 {
                     playerRigB.AddForce(new Vector3(0, inputY * yForce, 0), ForceMode.VelocityChange);
                     float yClampVel = inputY == 0 ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.y), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.y);
                     playerRigB.velocity = new Vector3(playerRigB.velocity.x, yClampVel, playerRigB.velocity.z);
                 }*/
        }

        if (jumpCount < maxJumps && !jumped && inputMan.inputAlt1 == 1)
        {
            jumpCount++;
            if (walled)
            {
                jumpCount--;
                Vector3 jumpDir = (wallNorm + transform.up).normalized;
                playerRigB.AddForce(jumpDir * 50000.0f, ForceMode.Force);
                StartCoroutine(WallJumpDelay(wallJumpDelayTime));
            }
            else { playerRigB.AddForce(new Vector3(0, jumpForce * jumpCount, 0), ForceMode.Impulse); }
            StartCoroutine(JumpDelay(jumpDelayTime));
        }

        if (!wallJumped)
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * xForce, 0, 0), ForceMode.VelocityChange);
        }
        float xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    private void PauseUpdate()
    {
        if (inputMan.inputCancel == 1 && !justPaused)
        {
            gameMMan.TogglePauseMenu();
            StartCoroutine(PauseDelay(1.0f));
        }
    }

    private void CursorMoveUpdate()
    {
        cursorRecT.position = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
    }

    private void FireUpdate()
    {
        if (inputMan.inputFire1 == 1 && !shot)
        {
            Vector3 playerScreenPos = playerCam.WorldToScreenPoint(transform.position);
            Vector3 mouseScreenPos = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
            Vector3 aimDir = new Vector3(mouseScreenPos.x - playerScreenPos.x, mouseScreenPos.y - playerScreenPos.y, 0).normalized;
            Quaternion aimQ = Quaternion.FromToRotation(Vector3.right, aimDir);
            Instantiate(bulletPrefab, transform.position, aimQ);
            StartCoroutine(ShootDelay(shootDelayTime));
        }
        //Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    /*============================================================================
     * ENUMERATOR METHODS
     ============================================================================*/
    private IEnumerator JumpDelay(float time)
    {
        jumped = true;
        yield return new WaitForSeconds(time);
        jumped = false;
    }
    private IEnumerator WallJumpDelay(float time)
    {
        wallJumped = true;
        yield return new WaitForSeconds(time);
        wallJumped = false;
    }
    private IEnumerator ShootDelay(float time)
    {
        shot = true;
        yield return new WaitForSeconds(time);
        shot = false;
    }
    private IEnumerator PauseDelay(float time)
    {
        justPaused = true;
        yield return new WaitForSeconds(time);
        justPaused = false;
    }

    /*============================================================================
     * COLLISION CHECK METHODS
     ============================================================================*/
    private bool GroundCheck()
    {
        Vector3 rightPos = transform.position + new Vector3(0.275f, 0, 0);
        Vector3 leftPos = transform.position + new Vector3(-0.275f, 0, 0);

        bool castContact = false;
        castContact = castContact | Physics.Raycast(rightPos, -transform.up, 0.4f, groundMask);  //RIGHT CAST
        castContact = castContact | Physics.Raycast(leftPos, -transform.up, 0.4f, groundMask);   //LEFT CAST
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
            if (Physics.Raycast(v, transform.right, out hit, 0.4f, groundMask))
            {
                if (hit.distance < closestDist)
                {
                    closestDist = hit.distance;
                    wallNorm = hit.normal;
                }
            }
            if (Physics.Raycast(v, -transform.right, out hit, 0.4f, groundMask))
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

    private void TransitionCheck(Collision coll)
    {
        if (coll.gameObject.tag == "TransitionArea")
        {
            tranMan.SceneSwitch(coll.gameObject.GetComponent<TransitionBlock>().goToName);
        }
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        xForce = 4.0f;
        yForce = 4.0f;
        jumpForce = 100.0f;

        maxXVelocity = 6.0f;
        maxYVelocity = 6.0f;
        maxJumpVelocity = 6.0f;
        maxFallVelocity = -12.0f;

        jumpCount = 0;
        maxJumps = 2;
        jumpDelayTime = 0.5f;
        wallJumpCount = 0;
        maxWallJumps = 1;
        wallJumpDelayTime = 0.2f;
        wallNorm = new Vector3(0, 0, 0);
        dashCount = 0;
        maxDashes = 1;
        shootDelayTime = 0.5f;

        jumped = false;
        grounded = false;
        walled = false;
        wallJumped = false;
        canClimb = false;
        shot = false;
        justPaused = false;
}

    [ContextMenu("Set to Platformer")]
    private void SetPlatformerValues()
    {

    }

    [ContextMenu("Set to Shooter")]
    private void SetShooterValues()
    {

    }
}