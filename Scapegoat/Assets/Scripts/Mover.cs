using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed;
    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;
    private float gravity = 10;



    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveVector = Vector3.zero;
        verticalVelocity = -gravity;
        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Turn" || other.tag == "Enemy_Turn")
        {
            speed *= -1;
        }
    }
}
