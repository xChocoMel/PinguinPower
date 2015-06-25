using UnityEngine;
using System.Collections;

public class TrampolineScript : MonoBehaviour {

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
			//player.transform.TransformDirection(Vector3.forward)
			player.GetComponent<Rigidbody>().AddForce(new Vector3(0,40,0));
		}
	}
	void OnTriggerEnter(Collider  collision) 
	{
 
		colliding=true;
	}
	void OnTriggerExit(Collider  collision) {
		Vector3 jumpspeed= player.GetComponent<Rigidbody>().velocity;
		jumpspeed.y = speed;
		player.GetComponent<Rigidbody> ().velocity = jumpspeed;
		colliding=false;
	}
}
