using UnityEngine;
using System.Collections;

public class Dieded : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            Destroy(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
