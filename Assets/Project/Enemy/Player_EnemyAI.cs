using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EnemyAI : MonoBehaviour
{
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed, 0f, Input.GetAxisRaw("Vertical") * speed);       
    }
}
