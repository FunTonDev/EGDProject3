using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody playerRigB;
    [SerializeField] private CapsuleCollider playerCapC;
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
    
    public bool grounded;
    public bool jumped;
    public bool floorContact;
    public bool canClimb;

    //Input values: moveX, moveY, jump, mouseX, mouseY, leftMouseButton, rightMouseButton
    private float inputX, inputY, inputJump, inputMX, inputMY, inputLMB, inputRMB;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        playerRigB = GetComponent<Rigidbody>();
        playerCapC = GetComponent<CapsuleCollider>();
        playerAudS = GetComponent<AudioSource>();
        jumped = false;
        //Cursor.visible = false;   //Uncomment when mouse move/aim implemented
    }

    private void Update()
    {
        grounded = !jumped && floorContact;
        InputUpdate("Horizontal", ref inputX);
        InputUpdate("Vertical", ref inputY);
        InputUpdate("MouseX", ref inputMX);
        InputUpdate("MouseY", ref inputMY);
        InputUpdate("Jump", ref inputJump);
        InputUpdate("Fire1", ref inputLMB);
        InputUpdate("Fire2", ref inputRMB);
        MouseMoveUpdate();
        //FireUpdate();
    }

    private void FixedUpdate()
    {
        //Movement/jumping are physics based changes
        CharMovementUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        floorContact = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        floorContact = false;
    }

    /*============================================================================
     * INPUT METHOD(S)
     ============================================================================*/
    private void InputUpdate(string InputAxis, ref float inputVar)
    {
        inputVar = Input.GetAxisRaw(InputAxis);
        if (inputVar != 0)
        {
            switch (InputAxis)
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
            }
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

            playerRigB.AddForce(new Vector3(0, inputJump * jumpForce, 0), ForceMode.Impulse);   //DONT MOVE THIS LINE
        }
        playerRigB.AddForce(new Vector3(inputX * xForce, 0, 0), ForceMode.VelocityChange);
        float xClampVel = (inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, playerRigB.velocity.z);
    }

    private void MouseMoveUpdate()
    {
        
    }

    private void FireUpdate()
    {
        if (inputLMB != 0)
        {

        }
        if (inputRMB != 0)
        {

        }
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
        
        grounded = false;
        jumped = false;
        floorContact = false;
        canClimb = false;
    }
}