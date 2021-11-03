using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EnemyAI : MonoBehaviour
{
    public float speed = 1;

    public bool platformer;

    // Update is called once per frame
    void Update()
    {
        if (platformer)
        {
            transform.Translate(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed, 0f);
        }

        else
        {
            transform.Translate(Input.GetAxisRaw("Horizontal") * speed, 0f, Input.GetAxisRaw("Vertical") * speed);
        }
    }


}
