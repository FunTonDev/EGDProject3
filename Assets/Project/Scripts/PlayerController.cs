using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject bulletPrefab;
    public List<AudioClip> playerClips;
    public Mesh SpriteMesh;
    public Mesh ModelMesh;

    [Header("Variables")]
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
    public float jumpDelayTime;
    public int wallJumpCount;
    public int maxWallJumps;
    public float wallJumpDelayTime;
    public Vector3 wallNorm;
    public int dashCount;
    public int maxDashes;
    public float shootDelayTime;

    public float maxHP;
    public float currentHP;
    public Image healthBar;

    public bool grounded;
    public bool jumped;
    public bool walled;
    public bool wallJumped;
    public bool canClimb;
    public bool climbing;
    public bool shot;
    public bool justPaused;
    public bool toggled;

    public LayerMask groundingMask;

    private delegate void ControlDelegate();
    ControlDelegate controlDel;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        gameMMan = GameObject.Find("[MANAGER]").GetComponent<GameMenuManager>();
        tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
        cursorRecT = GameObject.Find("Canvas/GamePanel/Cursor").GetComponent<RectTransform>();
        camCont = GameObject.Find("Main Camera").GetComponent<CameraController>();
        playerRigB = GetComponent<Rigidbody>();
        playerBoxC = GetComponent<BoxCollider>();
        playerAudS = GetComponent<AudioSource>();
        controlDel += PlatformerMoveUpdate;
    }

    private void Update()
    {
        if (!gameMan.paused)
        {
            CursorMoveUpdate();
            FireUpdate();
        }
        PauseUpdate();
        DebugUpdate();
    }

    private void FixedUpdate()
    {
        if (!gameMan.paused)
        {
            controlDel();   //Delegate uses current genre control schema
        }
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

    private void OnTriggerExit(Collider trigger)
    {
        TriggerCheck(trigger, false);
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void PlatformerMoveUpdate()
    {
        if (!grounded && !climbing)  //Airborne check
        {
            playerRigB.AddForce(Physics.gravity, ForceMode.Force);
            if (playerRigB.velocity.y < maxFallVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxFallVelocity, playerRigB.velocity.z); }
            else if (playerRigB.velocity.y > maxJumpVelocity) { playerRigB.velocity = new Vector3(playerRigB.velocity.x, maxJumpVelocity, playerRigB.velocity.z); }
        }

        if (inputMan.inputAlt1 == 1 && !jumped) //Jump check
        {
            if (walled && !grounded && wallJumpCount < maxWallJumps)
            {
                wallJumpCount++;
                Vector3 jumpDir = (wallNorm + transform.up).normalized;
                playerRigB.AddForce(jumpDir * 50000.0f, ForceMode.Force);
                StartCoroutine(WallJumpDelay(wallJumpDelayTime));
                StartCoroutine(JumpDelay(jumpDelayTime));
            }
            else if (jumpCount < maxJumps)
            {
                jumpCount++;
                playerRigB.AddForce(new Vector3(0, jumpForce * jumpCount, 0), ForceMode.Impulse);
                StartCoroutine(JumpDelay(jumpDelayTime));
            }
        }

        if (!wallJumped)    //X move check
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
        healthBar.fillAmount = currentHP / maxHP;
        cursorRecT.position = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
    }

    private void FireUpdate()
    {
        if (inputMan.inputFire1 == 1 && !shot)
        {
            Vector3 playerScreenPos = camCont.cam.WorldToScreenPoint(transform.position);
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
            case "TransitionArea":
                tranMan.SceneSwitch(trigger.gameObject.GetComponent<TransitionBlock>().goToName);
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
        maxWallJumps = 6;
        wallJumpDelayTime = 0.2f;
        wallNorm = new Vector3(0, 0, 0);
        dashCount = 0;
        maxDashes = 1;
        shootDelayTime = 0.5f;

        grounded = false;
        jumped = false;
        walled = false;
        wallJumped = false;
        canClimb = false;
        climbing = false;
        shot = false;
        justPaused = false;
        toggled = false;
}

    [ContextMenu("Set to Platformer")]
    private void SetPlatformerValues()
    {

    }

    [ContextMenu("Set to Shooter")]
    private void SetShooterValues()
    {

    }


    private void DebugUpdate()
    {
        if (inputMan.inputAlt3 == 1 && !toggled)
        {
            if (camCont.currMode == States.CameraMode.Platformer)
            {
                camCont.SetCameraMode(States.CameraMode.Shooter);
            }
            else
            {
                camCont.SetCameraMode(States.CameraMode.Platformer);
            }
            StartCoroutine(ToggleDelay(1.0f));
        }
    }
    private IEnumerator ToggleDelay(float time)
    {
        toggled = true;
        yield return new WaitForSeconds(time);
        toggled = false;
    }
}