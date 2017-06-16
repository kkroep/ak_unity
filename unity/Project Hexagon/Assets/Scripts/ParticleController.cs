using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    private ParticleSystem ps;
    public float speed=0;
    private Vector3 target_pos; 

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        target_pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (ps)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_pos, speed);
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
	}

    public void setTarget(Vector3 target)
    {
        target_pos = target;
    }
}
