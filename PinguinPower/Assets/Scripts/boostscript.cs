using UnityEngine;
using System.Collections;

public class boostscript : MonoBehaviour {

	// Use this for initialization
	public int speed;
	public GameObject player;
	bool enumeratorStarted=false;
	bool colliding=false;
	void Start () {
		if(player==null)
		{
			player=GameObject.Find ("Penguin");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(colliding)
		{
			player.GetComponent<Rigidbody>().AddForce(player.transform.TransformDirection(Vector3.forward)* speed);
		}
	}
	void OnTriggerEnter(Collider  collision) 
	{
		if(GetComponent<ParticleSystem>().isPlaying==false)
		{
		GetComponent<ParticleSystem>().Play();
		}
		colliding=true;
		 
		 
	}
	void OnTriggerExit(Collider  collision) {
		
		colliding=false;
	}
}
