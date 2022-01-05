using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    float acceleration = 0.48f;
    float deceleration = 0.5f;
    int velocityHash;
    /*int isWalkingHash;
    int isRunningHash;*/
    // Start is called before the first frame update
    void Start()
    {
        //set refrence for animator
        animator = GetComponent<Animator>();

        //increase performance
        velocityHash = Animator.StringToHash("Velocity");
        /*isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");*/
    }

    // Update is called once per frame
    void Update()
    {
        //get player's input key
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");

        /*
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        
        //if player is not walking and presses W key.
        if (!isWalking && forwardPressed)
        {
            //then set the isWalking boolean to start walking animation.
            animator.SetBool(isWalkingHash, true);
        }

        //if player is walking and is not pressing W key.
        if (isWalking && !forwardPressed)
        {
            //then set the isWalking boolean to stop walking animation.
            animator.SetBool(isWalkingHash, false);
        }

        //if player is not running and presses left shift key
        if (!isRunning && (forwardPressed && runPressed))
        {
            //then set the isRunning to true to start running animation.
            animator.SetBool(isRunningHash, true);
        }

        //if player is running and stops running or walking
        if (isRunning && (!forwardPressed || !runPressed))
        {
            //then set the isRunning to false to stop running animation.
            animator.SetBool(isRunningHash, false);
        }*/

        //if player presses W key
        if (forwardPressed && velocity < 1.0f)
        {
            //then increase the velocity of the player.
            velocity += Time.deltaTime * acceleration;
        }

        //if player not pressing W key
        if (!forwardPressed && velocity > 0.0f)
        {
            //then decrease the velocity of the player.
            velocity -= Time.deltaTime * deceleration;
        }

        //reset velocity to zero if it ever gets below zero.
        if (!forwardPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        //at the end update the velocity.
        animator.SetFloat(velocityHash, velocity);
    }
}
