using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerEnemy : Enemy
{

    [SerializeField] private bool facingLeft;


    public override void ClassUpdate()
    {
        Move(Vector3.zero);
    }


    /*
    public override void Introduction()
    {
        Debug.Log("I AM THE BOX GHOST OF THE 2D PlatformingTest");
    }
    */

    public override void Move(Vector3 Pos)
    {
        
        //Enemy on a path
        if (pathNodes.Count != 0)
        {
            //Debug.Log(this.transform.eulerAngles);

            base.Move(PathFollow());

            //Moving Right
            if (this.transform.position.x < base.PathFollow().x)
            {
                //Debug.Log("Eneemy Moving right");
                this.transform.eulerAngles = new Vector3(0, 180, 0);
                facingLeft = false;
            }            

            //Moving left
            else
            {
                //Debug.Log("Enemy Moving left");
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        }

        else
        {
            if (facingLeft)
            { transform.position += new Vector3(1, 0, 0) * moveSpd * Time.deltaTime; }

            else
            {transform.position += new Vector3(-1, 0, 0) * moveSpd * Time.deltaTime; }           

        }
                
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Checks to see if 
        if(collision.collider.tag == "Player")
        {
            float xDiff = Mathf.Abs(this.transform.position.x - collision.collider.transform.position.x);
            float yDiff = collision.collider.transform.position.y - this.transform.position.y;

            //Player jumped on enemy
            if((yDiff > 0.5f) && (collision.relativeVelocity.y < 0) )
            {
                Debug.Log(string.Format("Enemy {0} was jumped on by Player", type));

                this.TakeDamage(health);               

                collision.rigidbody.AddForce(Vector3.up * 3);
            }

            //Player takes damage
            else if(xDiff > 0.55f)
            {
                Debug.Log(string.Format("Enemy {0} hit Player", type));
            }
        }

        else
        {
            //Flip enemy direction
            //Make sure enemy didn't hit ground
            if(rgbdy.velocity.y == 0.0f)
            {
                Debug.Log(string.Format("Enemy {0} hit Wall", type));
                facingLeft = !facingLeft;
                this.transform.eulerAngles += new Vector3(0, 180, 0);
            }
        }

    }



}
