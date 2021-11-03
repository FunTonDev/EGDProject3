using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{
    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.name.Substring(0, 12) == "PlayerPrefab")
        {
            PlayerController playerCont = coll.GetComponent<PlayerController>();
            if (playerCont.closestTile == null || Vector3.Distance(transform.position, coll.transform.position) < Vector3.Distance(transform.position, playerCont.closestTile.transform.position))
            {
                playerCont.closestTile = gameObject;
            }
        }
    }
}
