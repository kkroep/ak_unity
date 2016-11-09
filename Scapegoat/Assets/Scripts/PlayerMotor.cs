using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    private Vector3 moveVector;


    //jumping parameters
    private float jumpforce = 16;
    private float gravity = 20;
    private float verticalVelocity;


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = Vector3.zero;

        //x
        

        //y
        if (controller.isGrounded)
        {
            verticalVelocity = -1;

            if (Input.GetKeyDown("space"))
            {
                verticalVelocity = jumpforce;
            }
        }

        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveVector.y = verticalVelocity;

        //Z
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
    }
}
