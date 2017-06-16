using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {
    Animator anim;
    private UnitController unitController;
    public GameObject blood_particle;
    public GameObject missile_particle;
    private bool missileAttack = true;
    protected GameObject gameController;
    private int skillType = 0; // init for movement

	// Use this for initialization
	void Start () {
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.SetInteger("anim_type", 0);
        unitController = GetComponent<UnitController>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
        //anim.SetInteger("anim_type", 0);
        
	}

    public void punch() {
        StartCoroutine(timedAnimation(0.1f, 2));
    }

    public void animateAttack(int target_x, int target_y)
    {
        if (missileAttack)
        {
            GameObject missile = Instantiate(missile_particle);
            missile.transform.position = transform.position+new Vector3(0, 1.5f, 0);
            missile.GetComponent<ParticleController>().setTarget(gameController.GetComponent<BoardController>().getTile(target_x, target_y).transform.position+new Vector3(0, 1.5f, 0));
            Debug.Log("Killing it!");
        }
        else
        {
            //unitController.rotate2Neighbor(target.GetComponent<UnitController>().getX() - unitController.getX(), target.GetComponent<UnitController>().getY() - unitController.getY());
            StartCoroutine(timedAnimation(0.1f, 2));
        }
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

    public bool checkIfMissileAttack()
    {
        if (skillType == 1)
            return missileAttack;
        else
            return false;
    }

    public float getAttackRange(int skill_type) {
        return 10f;}

    public void setSkillType(int new_type) {
        skillType = new_type;}

}
