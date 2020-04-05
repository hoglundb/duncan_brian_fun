using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //the rigidbody of the player game object. 
    private Rigidbody2D rb;

    //some forces to be applied to the player
    public float moveForce = 200.0f;
    public float jumpForce = 475.0f;

    //physics material to assign at runtime so player has friction with ground but doesn't stick to walls. 
    PhysicsMaterial2D noFriction = null;
    PhysicsMaterial2D someFrition = null;

    //determines if the player is touching the ground or not
    bool grounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        noFriction = Resources.Load("noFriction", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        someFrition = Resources.Load("someFriction", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = isGrounded();
        handleWallFriction();
        handleKeyboardInput();
    }


    //sets friction material based on if the player is grounded. (prevents player from sticking to side of wall)
    internal void handleWallFriction()
    {
        if (grounded)
        {
            GetComponent<Collider2D>().sharedMaterial = someFrition;
        }
        else
        {
            GetComponent<Collider2D>().sharedMaterial = noFriction;
        }
    }


    //handles keyboard input from the user. (call isGrounded() before calling this function)  
    internal void handleKeyboardInput()
    {
        //if player not grounded their shouldn't be any drag (prevents getting stuck on side of wall)
        if (grounded == false)
        {
            GetComponent<Collider2D>().sharedMaterial = noFriction;
        }
        else
        {
            GetComponent<Collider2D>().sharedMaterial = someFrition;
        }

        //apply left/right forces to the player's rigidbody based on "a" and "d" keys
        if (Input.GetKey("a"))
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.AddForce(new Vector2(-moveForce, 1.0f));
        }
        else if (Input.GetKey("d"))
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.AddForce(new Vector2(moveForce, 1.0f));
        }

        if (Input.GetKey("w"))
        {
            if (grounded)
            {
                rb.AddForce(new Vector2(0.0f, jumpForce));
            }
        }
    }


    //raycast down to see if player is grounded. Need to take player height into account
    private bool isGrounded()
    {
        //cast a short ray down and see if we are grounded or not
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .41f), new Vector2(0.0f, -1.0f), .01f);

        return hit.collider != null ? true : false;
    }
}
