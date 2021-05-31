using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	Plyer2 player;

	public Image green;
	public Image red;

	public Text B_text;
	public Image B_waku;
	public Image B_red;
	public Image B_green;
	public Image B_hai;

	SpawnEnemy spawn;
	BossContl boss;

	bool UI_isBoss;

	int frameCount=0;
	float prevTime=0.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").gameObject.GetComponent<Plyer2>();
		spawn = GameObject.Find ("SpawnEnemy").GetComponent<SpawnEnemy> ();
		UI_isBoss = false;
	}
	
	// Update is called once per frame
	void Update () {

		green.fillAmount = (float)(player.HP / 800.0);
		if (red.fillAmount > green.fillAmount) {
			red.fillAmount -= 0.00125f;
		}

		if (UI_isBoss == true) {
			B_green.fillAmount = (float)(boss.HP / 2000.0);
			if (B_red.fillAmount > B_green.fillAmount) {
				B_red.fillAmount -= 0.0005f;
			}
		}

		if (spawn.isBoss == true && UI_isBoss==false) {
			boss = GameObject.Find ("dsdragon(Clone)").gameObject.GetComponent<BossContl>();
			UI_isBoss = true;
			B_text.gameObject.SetActive (true);
			B_hai.gameObject.SetActive (true);
			B_red.gameObject.SetActive (true);
			B_green.gameObject.SetActive (true);
			B_waku.gameObject.SetActive (true);
		}
		if (spawn.isBoss == false && UI_isBoss == true) {
			UI_isBoss = false;
			B_text.gameObject.SetActive (false);
			B_hai.gameObject.SetActive (false);
			B_red.gameObject.SetActive (false);
			B_green.gameObject.SetActive (false);
			B_waku.gameObject.SetActive (false);
		}

		++frameCount;
		float time = Time.realtimeSinceStartup - prevTime;

		if (time >= 0.5f) {
			//Debug.LogFormat("{0}fps", frameCount / time);

			frameCount = 0;
			prevTime = Time.realtimeSinceStartup;
		}

	}
	void OnGUI () {
		float time = Time.realtimeSinceStartup - prevTime;
		//GUI.Label(new Rect(0, 100, 300, 400), "fps:"+frameCount / time);
		//GUI.Label(new Rect(0, 120, 300, 420), "target:"+Application.targetFrameRate);
	}
}
