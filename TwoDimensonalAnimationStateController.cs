using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensonalAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    //Increase Performance
    int velocityZHash;
    int velocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        //set refrence for animator
        animator = GetComponent<Animator>();

        velocityZHash = Animator.StringToHash("VelocityZ");
        velocityXHash = Animator.StringToHash("VelocityX");

    }

    // Update is called once per frame
    void Update()
    {
        //get key input
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);


        //set current maximum Velocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        //if player presses W key
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityZ += Time.deltaTime * acceleration;
        }

        //if player presses S key
        if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityZ -= Time.deltaTime * acceleration;
        }

        //if player presses A key
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityX -= Time.deltaTime * acceleration;
        }

        //if player presses D key
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityX += Time.deltaTime * acceleration;
        }

        //if player presses S & D key
        if (backwardPressed && rightPressed && velocityZ > -currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityZ -= Time.deltaTime * acceleration;
            velocityX += Time.deltaTime * acceleration;
        }

        //if player presses S & A key
        if (backwardPressed && leftPressed && velocityZ > -currentMaxVelocity)
        {
            //then increase the velocity of the player.
            velocityZ -= Time.deltaTime * acceleration;
            velocityX -= Time.deltaTime * acceleration;
        }

        //decrease velocityZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //increase velocityZ
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        //reset velocityZ
        if (!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }

        //increase velocityX if A key is not pressed and velocityX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //decrease velocityX if D key is not pressed and velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //reset velocityX
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        //lock left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity + 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        //lock right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }

        //set the parameters to our local variable values.
        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);
    }
}
