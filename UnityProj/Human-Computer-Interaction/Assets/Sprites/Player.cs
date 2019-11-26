using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XBOX;
using Ardunity;

public class Player : MonoBehaviour
{
    float speed = 50f;
    public float firstJump = 100f;
    private float secondJump = 75f;

    public Vector2 move_input;
    public Rigidbody2D playerBody;

    private bool jumping;
    private bool canJump;

    public ArdunityApp controller;
    public AnalogInput controllerX;
    public DigitalInput controllerButton;

    private bool contBut;

    // Start is called before the first frame update
    void Start()
    {
        playerBody.GetComponent<Rigidbody2D>();

        controller = GameObject.FindGameObjectWithTag("Ardunity").GetComponent<ArdunityApp>();
        controllerX = GameObject.FindGameObjectWithTag("PlayerX").GetComponent<AnalogInput>();
        controllerButton = GameObject.FindGameObjectWithTag("PlayerButton").GetComponent<DigitalInput>();

        canJump = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller.connected)
        {
            ArduinoInputs();
        }
        else if (XInput.GetConnected())
        {
            ControllerInputs();
        }
        else
        {
            KeyboardInputs();
        }
    }
    
    void KeyboardInputs()
    {
        KeyboardMovement(move_input);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerDash();
        }
    }

    void ControllerInputs()
    {
        ControllerMovement(move_input);
        if(XInput.GetKeyPressed(0, (int)Buttons.A))
        {
            PlayerJump();
        }

        if(XInput.GetKeyPressed(0, (int)Buttons.RTrig))
        {
            PlayerDash();
        }
    }

    void ArduinoInputs()
    {
        ArduinoMovement(move_input);
        
        if(!contBut && controllerButton.Value)
        {
            if (canJump)
                PlayerJump();
            else
                PlayerDash();
        }

        contBut = controllerButton.Value;
    }

    void KeyboardMovement(Vector2 moveInput)
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = 0f;

        MovePlayer(moveInput);
    }

    void ControllerMovement(Vector2 moveInput)
    {
        moveInput.x = XInput.GetLeftX();
        moveInput.y = 0f;

        MovePlayer(moveInput);
    }

    void ArduinoMovement(Vector2 moveInput)
    {
        if(controllerX.Value < 0.48f || controllerX.Value > 0.52f)
        {
            moveInput.x = (controllerX.Value - 0.5f) * -2f;
        }
        moveInput.y = 0f;

        MovePlayer(moveInput);
    }

    void MovePlayer(Vector2 moveInput)
    {
        playerBody.velocity = moveInput * speed * Time.fixedDeltaTime;

        //if(moveInput.x < 0f)
        //{
        //    GetComponentInChildren<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    GetComponentInChildren<SpriteRenderer>().flipX = false;
        //}
    }

    void PlayerJump()
    {
        if (canJump)
        {
            if (!jumping)
            {
                playerBody.velocity += secondJump * Vector2.up;
                //playerBody.AddForce(firstJump * Vector2.up, ForceMode2D.Impulse);
                jumping = true;
            }
            else
            {
                playerBody.velocity += firstJump * Vector2.up;
                //playerBody.AddForce(secondJump * Vector2.up, ForceMode2D.Impulse);
                canJump = false;
            }
        }
    }

    void PlayerDash()
    {
        if (jumping)
        {
            Vector2 tempVec = new Vector2(40f * move_input.x, 0f);

            playerBody.velocity = tempVec;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.position.y < playerBody.position.y)
        {
            canJump = true;
            jumping = false;
        }
    }
}
