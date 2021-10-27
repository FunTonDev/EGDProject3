using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVolumeData : MonoBehaviour
{
    private PlayerController playerCont;
    private CameraController camCont;

    [Header("Components")]
    public Mesh playerMesh;
    public Material playerMaterial;

    [Header("Variables")]
    public States.GameGenre primaryGenre;   //Primary -> Controls/Position/Orientation
    public States.GameGenre subGenre;       //Secondary -> Camera

    private Vector3 modelEulers;
    private Vector3 modelScale;
    private RigidbodyConstraints constraint;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    void Start()
    {
        playerCont = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>();
        camCont = GameObject.Find("Main Camera").GetComponent<CameraController>();
        if (primaryGenre == States.GameGenre.Platformer || primaryGenre == States.GameGenre.None)
        {
            modelEulers = new Vector3(90, 180, 0);
            modelScale = new Vector3(0.1f, 0.1f, 0.1f);
            constraint = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            modelEulers = Vector3.zero;
            modelScale = Vector3.one;
            constraint = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    public void PlayerModeUpdate()
    {
        switch (primaryGenre)
        {
            case States.GameGenre.Platformer:
                SetMoveValues(5.0f, 5.0f, 5.0f, 5.0f);
                SetPlatformerValues(100.0f, 80.0f, 6.0f, -12.0f);
                break;
            case States.GameGenre.Shooter:
                SetMoveValues(3.0f, 3.0f, 4.0f, 4.0f);
                SetShooterValues();
                break;
            case States.GameGenre.RPG:
                SetMoveValues(2.0f, 2.0f, 4.0f, 4.0f);
                SetRPGValues();
                break;
            default:
                SetMoveValues(6.0f, 6.0f, 6.0f, 6.0f);
                SetHubValues();
                break;
        }
        if (primaryGenre == States.GameGenre.Platformer || primaryGenre == States.GameGenre.None)
        {
            playerCont.transform.position = new Vector3(playerCont.transform.position.x, playerCont.transform.position.y, transform.position.z);
        }
        else if(primaryGenre == States.GameGenre.RPG)
        {
            playerCont.transform.position = new Vector3(playerCont.transform.position.x, transform.position.y, playerCont.transform.position.z);
        }
        playerCont.playerRigB.velocity = Vector3.zero;
        playerCont.playerModelObj.transform.eulerAngles = modelEulers;
        playerCont.playerModelObj.transform.localScale = modelScale;
        playerCont.playerPrimaryGenre = primaryGenre;
        playerCont.playerSubGenre = subGenre;
        playerCont.playerMeshF.mesh = playerMesh;
        playerCont.playerMeshR.material = playerMaterial;
        playerCont.playerRigB.constraints = constraint;
        camCont.SetCameraMode(subGenre);
    }

    private void SetMoveValues(float inXForce, float inYForce, float inMaxXVel, float inMaxYVel)
    {
        playerCont.xForce = inXForce;
        playerCont.yForce = inYForce;
        playerCont.maxXVelocity = inMaxXVel;
        playerCont.maxYVelocity = inMaxYVel;
    }

    private void SetPlatformerValues(float inJumpForce, float inDashForce, float inMaxJumpVel, float inMaxFallVel)
    {
        playerCont.playerRigB.useGravity = true;
        playerCont.controlDel = playerCont.PlatformerMoveUpdate;
        playerCont.GenreCosmeticUpdate(0);
        playerCont.jumpForce = inJumpForce;
        playerCont.dashForce = inDashForce;
        playerCont.maxJumpVelocity = inMaxJumpVel;
        playerCont.maxFallVelocity = inMaxFallVel;
    }

    private void SetShooterValues()
    {
        playerCont.playerRigB.useGravity = true;
        playerCont.controlDel = playerCont.ShooterMoveUpdate;
        playerCont.GenreCosmeticUpdate(1);
    }

    private void SetRPGValues()
    {
        playerCont.playerRigB.useGravity = false;
        playerCont.controlDel = playerCont.RPGMoveUpdate;
        playerCont.GenreCosmeticUpdate(2);
    }
    private void SetHubValues()
    {
        playerCont.playerRigB.useGravity = false;
        playerCont.controlDel = playerCont.HubMoveUpdate;
        playerCont.GenreCosmeticUpdate(-1);
    }
}
