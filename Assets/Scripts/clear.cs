using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clear : MonoBehaviour {

	float DistanceToPlayer;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		DistanceToPlayer = Vector2.Distance (new Vector2 (this.transform.position.x, this.transform.position.z), new Vector2 (player.transform.position.x, player.transform.position.z));
	
		if (DistanceToPlayer < 5) {
			SceneManager.LoadScene ("result");
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
