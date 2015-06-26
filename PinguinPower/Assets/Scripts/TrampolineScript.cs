using UnityEngine;
using System.Collections;

public class TrampolineScript : MonoBehaviour {

	public int speed;
	public GameObject player;
	private bool colliding = false;
	private Animator animator;

	// Use this for initialization
	void Start () {
		if (player == null)
		{
			player = GameObject.Find ("Penguin");
		}

		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(colliding)
		{
			player.GetComponent<Rigidbody>().AddForce(new Vector3(0,40,0));
		}
	}

	void OnTriggerEnter(Collider  collision) 
	{
		if (animator != null) {
			animator.SetTrigger ("Bounce");
		}

		colliding = true;
	}

	void OnTriggerExit(Collider  collision) 
	{
		Vector3 jumpspeed = player.GetComponent<Rigidbody>().velocity;
		jumpspeed.y = speed;
		player.GetComponent<Rigidbody> ().velocity = jumpspeed;
		colliding = false;
	}
}
