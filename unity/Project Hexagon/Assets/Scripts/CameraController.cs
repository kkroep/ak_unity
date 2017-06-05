using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Vector3 start_pos;
    private Vector3 end_pos;
    private Vector3 offset;
    private int speed;


	// Use this for initialization
	void Start () {
        start_pos = transform.position;
        end_pos = transform.position;
        offset = new Vector3(0, 13, -11);
        speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, end_pos, speed);
    }

    public void setTarget(Vector3 target_pos)
    {
        end_pos = target_pos + offset;
    }
}
