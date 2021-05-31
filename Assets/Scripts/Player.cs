using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float RotationY;
	public float RotationSpeedX;
	public float RotationSpeedY;
	public float MoveSpeed;
	public float GravityY;//落下速度(これにrigidbodyの落下加速度を足す)
	public float JumpCount=0;
	public float JumpHigh;
	public float DistancePlayerToCamera = 10;

	public int isDamage;
	public Vector3 ImpactVector;

	public GameObject magic1;

	CharacterController controller;
	Animator anim;

	// Use this for initialization
	void Start () {

		controller = GetComponent<CharacterController> ();
		anim = GameObject.Find ("unitychan").GetComponent<Animator> ();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			if (Cursor.visible==true) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (isDamage == 0) {
			anim.SetBool ("isDamage", false);
			Move ();
		} else {
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isDamage", true);
			Impact ();
		}
		Cam ();
		Atk ();


	}

	//player moving process(called once per frame)
	void Move(){
		RotationY += (Input.GetAxisRaw ("Mouse X") * RotationSpeedX * Time.deltaTime);
		transform.rotation = Quaternion.Euler (0, RotationY, 0);

		Vector3 GoMove = new Vector3 (0, 0, 0);
		if (Input.GetAxisRaw ("Vertical") > 0) GoMove.z = 1;
		if (Input.GetAxisRaw ("Vertical") < 0) GoMove.z = -1;
		if (Input.GetAxisRaw ("Horizontal") > 0) GoMove.x = 1;
		if (Input.GetAxisRaw ("Horizontal") < 0) GoMove.x = -1;

		if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") == 0) {
			anim.SetBool ("isRunning", false);
		} else {
			anim.SetBool ("isRunning", true);
		}

		GravityY += Physics.gravity.y * Time.deltaTime / 2;
		if (controller.isGrounded) {
			GravityY = 0;
			JumpCount = 0;
		}

		/*ジャンプ
		if (Input.GetButtonDown ("Jump") && JumpCount==0) {
			GravityY = JumpHigh;
			JumpCount = 1;
		}*/

		GoMove.y = GravityY;

		controller.Move (transform.rotation * GoMove * MoveSpeed * Time.deltaTime);

		if (JumpCount == 0) {
			//this.GetComponent<MeshRenderer> ().material.color = Color.red;
		} else {
			//this.GetComponent<MeshRenderer> ().material.color = Color.white;
		}
		
	}

	//player shooting magic process(called once per frame)
	void Atk(){

		if (Input.GetButtonDown ("Fire1")) {
			if (Camera.main.transform.eulerAngles.x<90) {
				Instantiate (magic1, transform.position, transform.rotation);
			}
			if (Camera.main.transform.eulerAngles.x>270) {
				Instantiate (magic1, transform.position, Camera.main.transform.rotation);
			}
		}

	}

	//camera moving process(called once per frame)
	void Cam(){

		Vector3 lookAt = transform.position+Vector3.up * 1.2f;
		if (Input.GetAxisRaw ("Mouse Y")<0 && (Camera.main.transform.eulerAngles.x<50 || Camera.main.transform.eulerAngles.x>270)) {
			Camera.main.transform.RotateAround (lookAt, transform.right, Input.GetAxisRaw ("Mouse Y")*-RotationSpeedY*Time.deltaTime);
		}
		if (Input.GetAxisRaw ("Mouse Y")>0 && (Camera.main.transform.eulerAngles.x>340 || Camera.main.transform.eulerAngles.x<90)) {
			Camera.main.transform.RotateAround (lookAt, transform.right, Input.GetAxisRaw ("Mouse Y")*-RotationSpeedY*Time.deltaTime);
		}

		for (int i = 0; i < 2; i++) {//preventing comeing into ground
			Camera.main.transform.position = lookAt - Camera.main.transform.forward * DistancePlayerToCamera;
			if (Camera.main.transform.position.y < 1 && DistancePlayerToCamera > 0f) {
				DistancePlayerToCamera -= 0.1f;
				i = 0;
			}
			if (Camera.main.transform.position.y > 1.2f && DistancePlayerToCamera < 10f) {
				DistancePlayerToCamera += 0.1f;
				i = 0;
			}
		}


	}

	void Impact(){
		RotationY += (Input.GetAxisRaw ("Mouse X") * RotationSpeedX * Time.deltaTime);
		transform.rotation = Quaternion.Euler (0, RotationY, 0);
		//ImpactVector = new Vector3 (0, 0, 0);
		if (isDamage==50) GravityY = 1.5f;

		GravityY += Physics.gravity.y * Time.deltaTime / 2;
		ImpactVector.y = GravityY;

		controller.Move (ImpactVector * 10 * Time.deltaTime);
		if (controller.isGrounded) {
			GravityY = 0;
		}
		isDamage--;
	}

	//indicating GUI process(called once per frame)
	void OnGUI () {
		GUI.Label(new Rect(0, 0, 300, 300), ""+Camera.main.transform.eulerAngles.x);
		GUI.Label(new Rect(0, 20, 300, 300), ""+Vector3.Distance(this.transform.position,Camera.main.transform.position));
		GUI.Label(new Rect(0, 40, 300, 300), ""+Camera.main.transform.position.y);
	}
}
