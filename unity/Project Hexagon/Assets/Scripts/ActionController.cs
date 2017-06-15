using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {
    Animator anim;
    public GameObject blood_particle;

	// Use this for initialization
	void Start () {
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.SetInteger("anim_type", 0);
        
	}
	
	// Update is called once per frame
	void Update () {
        //anim.SetInteger("anim_type", 0);
        
	}

    public void punch() {
        StartCoroutine(timedAnimation(0.1f, 2));
    }

    public void walk() {
        StartCoroutine(timedAnimation(0.1f, 1));
    }

    IEnumerator timedAnimation(float timer, int anim_type)
    {
        anim.SetInteger("anim_type", anim_type);
        yield return new WaitForSeconds(timer); // Calls for the function WaitForSeconds. Yeild break breaks this.
        anim.SetInteger("anim_type", 0);
    }

    public void bleed() {
        GameObject blood = Instantiate(blood_particle);
        blood.transform.position = transform.position+new Vector3(0, 1.5f, 0);
    }
}
