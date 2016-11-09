using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class enemy_attack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other){
      if(other.tag == "Player")
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
