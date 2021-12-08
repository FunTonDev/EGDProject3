using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    private PlayerController playerCont;
    private CameraController camCont;
    private TransitionManager tranMan;

    [Header("SELECT VOLUME TYPE")]
    public States.VolumeType volumeType;

    [Header("Control Data")]
    public Mesh playerMesh;
    public Material playerMaterial;
    public States.GameGenre primaryGenre;   //Primary -> Controls/Position/Orientation
    public States.GameGenre subGenre;       //Secondary -> Camera
    private Vector3 modelEulers;
    private Vector3 modelScale;
    private RigidbodyConstraints constraint;

    [Header("Transition Data")]
    public string targetScene;

    [Header("Damage Data")]
    public bool isKillVolume;
    public float damage;
    public float damageTimer;
    public float damageDelay;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        playerCont = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>();
        switch (volumeType)
        {
            case States.VolumeType.Control:
                camCont = GameObject.Find("Main Camera").GetComponent<CameraController>();
                modelEulers = Vector3.zero;
                modelScale = Vector3.one;
                constraint = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                if (primaryGenre == States.GameGenre.Platformer || primaryGenre == States.GameGenre.None)
                {
                    modelEulers = new Vector3(90, 180, 0);
                    modelScale = new Vector3(0.1f, 0.1f, 0.1f);
                    constraint = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                }
                break;
            case States.VolumeType.Damage:
                damage = -Mathf.Abs(isKillVolume ? float.MaxValue : damage);
                damageTimer = 0;
                break;
            case States.VolumeType.Transition:
                tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
                break;
        } 
    }

    private void Update()
    {
        if (volumeType == States.VolumeType.Damage && !isKillVolume)
        {
            damageTimer -= damageTimer > 0 ? Time.deltaTime : 0;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag != "PathNode")
        {
            if (coll.gameObject.tag == "Player")
            {
                switch (volumeType)
                {
                    case States.VolumeType.Control:
                        PlayerModeUpdate();
                        break;
                    case States.VolumeType.Transition:
                        tranMan.SceneSwitch(targetScene, true);
                        break;
                    case States.VolumeType.Cutscene:
                        coll.GetComponent<PlayerController>().so.lastPosition = coll.transform.position;
                        SaveManager.Save(coll.GetComponent<PlayerController>().so);
                        tranMan.SceneSwitch("CutsceneScene");
                        break;
                }
            }
        }
        
    }

    private void OnTriggerStay(Collider coll)
    {
        if(coll.gameObject.tag != "PathNode")
        {
            if (coll.gameObject.tag == "Player")
            {
                if (volumeType == States.VolumeType.Damage && damageTimer <= 0)
                {
                    playerCont.HealthUpdate(damage);
                    damageTimer = !isKillVolume ? damageDelay : 0;
                }
            }
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
        playerCont.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerCont.playerMeshF.gameObject.transform.eulerAngles = modelEulers;
        playerCont.playerMeshF.gameObject.transform.localScale = modelScale;
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
