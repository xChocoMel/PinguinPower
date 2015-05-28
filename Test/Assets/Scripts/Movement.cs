using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public Camera maincamera;
	public Camera puzzlecamera;
	private bool move;
	private bool turnleft;
	private bool turnright;
	private bool puzzle;

	// Use this for initialization
	void Start () {
		maincamera.GetComponent<Camera>().enabled = true;
		puzzlecamera.GetComponent<Camera>().enabled = false;
		puzzle = false;
		move = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (turnleft) {
			this.transform.Rotate(Vector3.up, -2);
		}

		if (turnright) {
			this.transform.Rotate(Vector3.up, 2);
		}

		if (move) {
			this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*15);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (move) {
				move = false;
			} else {
				move = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			move = false;
			turnleft = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow)) {
			turnleft = false;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			move = false;
			turnright = true;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow)) {
			turnright = false;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			this.GetComponent<Rigidbody>().AddForce (
				new Vector3 (GetComponent<Rigidbody>().velocity.x, 400, GetComponent<Rigidbody>().velocity.z));
		}
	}

	public void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag.Equals ("enterpuzzle")) {
			puzzle = true;
			maincamera.GetComponent<Camera>().enabled = false;
			puzzlecamera.GetComponent<Camera>().enabled = true;
		}

		if (col.gameObject.tag.Equals ("leavepuzzle")) {
			puzzle = false;
			maincamera.GetComponent<Camera>().enabled = true;
			puzzlecamera.GetComponent<Camera>().enabled = false;
		}
	}

	public void OnCollisionEnter(Collision hit) {
		if (hit.gameObject.tag.Equals ("icecube")) {
			Debug.Log("icecube");
			hit.gameObject.GetComponent<Rigidbody>().AddForce (Vector3.forward*6);
		}
	}
}
