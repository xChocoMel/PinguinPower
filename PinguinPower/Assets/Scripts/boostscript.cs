using UnityEngine;
using System.Collections;

public class boostscript : MonoBehaviour {

	public int speed = 50;
	private GameObject player;
	private bool colliding = false;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag("Penguin");
	}
	
	// Update is called once per frame
	void Update () {
		if(colliding) {
			player.GetComponent<Rigidbody>().AddForce(player.transform.TransformDirection(Vector3.forward)* speed);
		}
	}

	void OnTriggerEnter(Collider collision) {
		this.GetComponentInChildren<ParticleSystem> ().Play ();
		player.GetComponent<Rigidbody>().AddForce(player.transform.TransformDirection(Vector3.forward)* speed);
		colliding = true;	 		 
	}

	void OnTriggerExit(Collider collision) {		
		colliding = false;
	}
}
