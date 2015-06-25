using UnityEngine;
using System.Collections;

public class boostscript : MonoBehaviour {

	// Use this for initialization
	public int speed;
	public GameObject player;
	bool colliding = false;

	void Start () {

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
