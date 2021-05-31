using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour {

	public AudioClip battle_1;
	public AudioClip boss_2;

	private AudioSource[] audio;

	public float clip1_length;
	public float clip2_length;

	private int Mode;
	public float time;

	string SceneName;

	SpawnEnemy spawn;

	// Use this for initialization
	void Start () {

		SceneName = SceneManager.GetActiveScene ().name;

		audio = GetComponents<AudioSource> ();
		if (SceneName == "main") {
			spawn = GameObject.Find ("SpawnEnemy").GetComponent<SpawnEnemy> ();
		}

		clip1_length = battle_1.length;
		clip2_length = boss_2.length;

		Mode = 1;
		audio[0].clip = battle_1;
		audio[0].Play ();
		audio [3].Play ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Mode == 1) {
			if (audio[0].time > clip1_length - 2) {
				audio[0].volume = 1 - (audio[0].time - clip1_length + 2) / 2;
			} else if (audio[0].time < 2) {
				audio[0].volume = audio[0].time / 2;
			} else {
				audio[0].volume = 1;
			}
		}
		if (Mode == 2) {
			if (audio[0].time > clip2_length - 2) {
				audio[0].volume = 1 - (audio[0].time - clip2_length + 2) / 2;
			} else if (audio[0].time < 2) {
				audio[0].volume = audio[0].time / 2;
			} else {
				audio[0].volume = 1;
			}
		}

		if (SceneName == "main") {
			if (spawn.isBoss == true && Mode != 2) {
				Mode = 2;
				audio [0].clip = boss_2;
				audio [0].Play ();
			}
			if (spawn.isBoss == false && Mode == 2) {
				Mode = 3;
				audio [0].Stop ();
			}
		}

		time = audio[0].time;
	}

	public void ShotCall(){
		audio [1].Play ();
	}

	public void DamageCall(){
		audio [2].Play ();
	}

	public void WalkCall(bool func){
		if (func) {
			audio [3].UnPause ();
		} else {
			audio [3].Pause ();
		}
	}
}
