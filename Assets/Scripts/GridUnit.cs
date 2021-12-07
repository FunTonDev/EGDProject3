using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{
    public bool occupied = false;

    private void OnTriggerStay(Collider coll)
    {

        if (coll.gameObject.tag != "PathNode")
        {
            if (coll.gameObject.name.Substring(0, 12) == "PlayerPrefab")
            {
                PlayerController playerCont = coll.GetComponent<PlayerController>();
                playerCont.transform.rotation = transform.rotation;
                if (!occupied && (playerCont.closestTile == null
                    || Vector3.Distance(transform.position, coll.transform.position) < Vector3.Distance(transform.position, playerCont.closestTile.transform.position)))
                {
                    playerCont.closestTile = gameObject;
                }
            }
            else if (coll.gameObject.tag == "Enemy")
            {
                RPG_Enemy enemy = coll.GetComponent<RPG_Enemy>();
                if (enemy.closestTile == null || Vector3.Distance(this.transform.position, coll.transform.position) < Vector3.Distance(this.transform.position, enemy.closestTile.transform.position))
                {
                    enemy.closestTile = gameObject;
                }
            }
        }       
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PathNode")
        {
            //coll.transform.position = transform.position;
            coll.gameObject.active = false;
        }
    }

}
