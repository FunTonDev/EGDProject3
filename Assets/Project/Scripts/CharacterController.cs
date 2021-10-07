using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputManager inputMan;
    [SerializeField] private RectTransform cursorRecT;

    [SerializeField] private Rigidbody playerRigB;
    [SerializeField] private BoxCollider playerCapC;
    [SerializeField] private AudioSource playerAudS;
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
    public int wallJumpCount;
    public int maxWallJumps;
    public int dashCount;
    public int maxDashes;

    public bool grounded;
    public bool floorContact;
    public bool canClimb;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        playerRigB = GetComponent<Rigidbody>();
        playerCapC = GetComponent<BoxCollider>();
        playerAudS = GetComponent<AudioSource>();
        cursorRecT = GameObject.Find("Canvas/Cursor").GetComponent<RectTransform>();
        Cursor.visible = false;   //Uncomment when mouse move/aim implemented
    }

    private void Update()
    {
        MouseMoveUpdate();
    }

    private void FixedUpdate()
    {
        CharMovementUpdate();   //Movement/jumping are physics based changes
    }

    private void OnCollisionEnter(Collision collision)
    {
        floorContact = true;
        GroundCheck();
    }

    private void OnCollisionExit(Collision collision)
    {
        floorContact = false;
        GroundCheck();
    }

    /*============================================================================
     * INPUT METHOD(S)
     ============================================================================*/
    private void PlayInputClip(float inputVar)
    {
        if (inputVar != 0)
        {
            /*switch (InputAxis)
            {
                case "Horizontal":
                    playerAudS.PlayOneShot(playerClips[0]);
                    break;
                case "Vertical":
                    playerAudS.PlayOneShot(playerClips[1]);
                    break;
                case "Jump":
                    playerAudS.PlayOneShot(playerClips[2]);
                    break;
                case "Fire1":
                    playerAudS.PlayOneShot(playerClips[4]);
                    break;
                case "Fire2":
                    playerAudS.PlayOneShot(playerClips[5]);
                    break;
            }*/
        }
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
        if (grounded)   //IF IS/BECOMES GROUNDED, y movement velocity must be capped
        {
           /* if (canClimb)   //Temporary but other confirmed gameplay functionality could be WIP(entering doorways, climbing ladders, looking up, crouch, etc)
            {
                playerRigB.AddForce(new Vector3(0, inputY * yForce, 0), ForceMode.VelocityChange);
                float yClampVel = inputY == 0 ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.y), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.y);
                playerRigB.velocity = new Vector3(playerRigB.velocity.x, yClampVel, playerRigB.velocity.z);
            }*/
        }
        //float jumpCalculated = Utils.NormalizeNum(jumpCount) * inputMan.inputAlt1 * jumpForce;
        if (jumpCount > maxJumps)
        {
            playerRigB.AddForce(new Vector3(0, inputMan.inputAlt1 * jumpForce, 0), ForceMode.Impulse);
        }
        
        //playerRigB.AddForce(new Vector3(inputX * xForce, 0, 0), ForceMode.VelocityChange);
        //float xClampVel = (inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        //playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    private void MouseMoveUpdate()
    {
        cursorRecT.position = new Vector3(inputMan.inputMX, inputMan.inputMY, 0);
    }

    private void FireUpdate()
    {
        
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        xForce = 5.0f;
        yForce = 5.0f;
        jumpForce = 80.0f;

        maxXVelocity = 6.0f;
        maxYVelocity = 6.0f;
        maxJumpVelocity = 6.0f;
        maxFallVelocity = -12.0f;

        jumpCount = 0;
        maxJumps = 2;
        wallJumpCount = 0;
        maxWallJumps = 1;
        dashCount = 0;
        maxDashes = 1;

        grounded = false;
        floorContact = false;
        canClimb = false;
    }

    [ContextMenu("Set to Platformer")]
    private void SetPlatformerValues()
    {

    }

    [ContextMenu("Set to Shooter")]
    private void SetShooterValues()
    {

    }

    private void GroundCheck()
    {
        grounded = floorContact;
    }
}