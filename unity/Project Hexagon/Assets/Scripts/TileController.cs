using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
        private int turn_edited;
        private int current_team;
        public Material team_0_mat;
        public Material team_1_mat;
        public Material no_team_mat;
	// Use this for initialization
	void Start () {
            turn_edited = -1;
            current_team = -1;
            Debug.Log(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

        public void claimTerritorium(int turn, int team) {
        if (turn == turn_edited)
            if (current_team != team) {
                current_team = -1;
                Debug.Log(this.gameObject);
            }
        return;
        }
}
