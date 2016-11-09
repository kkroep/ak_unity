using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    private Vector3 moveVector;


    //jumping parameters
    public float jumpforce;
    public float gravity;
    private float verticalVelocity;


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Turn")
        {
            speed *= -1;
            //controller.transform.Rotate(new Vector3(0,180,0));
        }
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
