using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonmovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public bool sprint = false;
    public int sprintspeed;
    public float speed = 1f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothvelocity;
  
    private Rigidbody rb;
    public float jumpForce = 15f;
   
    public bool crouching = false;
    
   
    public float currentstamina = 100f;
    public float stamina = 100f;
    public float MoveSpeed = 20f;


    Vector3 jumpVector = Vector3.zero;
    private void Start()
    {
        
        //rb = GetComponent<Rigidbody>();
    }
    private Vector3 InputVector;

   
    void Update()   
    {
        

        // sprint toggle on and off and sets speed based on movespeed
        speed = MoveSpeed * sprintspeed / 6;
        if (sprint == true)
        {
            sprintspeed = 4;

        }
        else if (sprint == false)
        {
            if (crouching == false)
            {
                sprintspeed = 2;
            }
            else if (crouching == true)
            {
                sprintspeed = 1;
            }
        }

        // toggles on and off crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (crouching == false)
            {
                sprint = false;
                StartCoroutine(staminaregen());
                crouching = true;
            }
            else if (crouching == true)
            {
                crouching = false;
            }
        }

        // tells player to jump
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumpVector.y = jumpForce;
        }
 
        // toggles sprint on and off 
        if (crouching == false)
        {
            if (currentstamina > 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (sprint == false)
                    {
                        sprint = true;
                        StartCoroutine(staminaLose());

                    }
                    else if (sprint == true)
                    {
                        sprint = false;
                        StartCoroutine(staminaregen());

                    }
                }
            }
        }
        // stops sprtinging if run out of stamina
        if (currentstamina <= 0)
        {
            sprint = false;
            StartCoroutine(staminaregen());
        }
        
        //player direction movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 moveDir = Vector3.zero;
        if (direction.magnitude >= 0.1f)
        {

            // turn smoothing
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            // movedirection
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            // rb.velocity = moveDir.normalized * speed *Time.deltaTime;
        }
        // makes player fall down and actully moves charter
        jumpVector += Physics.gravity * Time.deltaTime * 2;// * Time.deltaTime;

        controller.Move(((moveDir.normalized * speed) + jumpVector) * Time.deltaTime);
     
        if (controller.isGrounded == true)
        {
            jumpVector = Vector3.down * 2;
        }

    }
    // stam usage and regenration
    IEnumerator staminaregen()
    {
        while (sprint == false)
        {
            if (currentstamina < stamina)
            {
               currentstamina = currentstamina + 5;
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }

    IEnumerator staminaLose()
    {
        while (sprint == true)
        {
            currentstamina = currentstamina - 3f;

            yield return new WaitForSecondsRealtime(1);
        }
    }
}


