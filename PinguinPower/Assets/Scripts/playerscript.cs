using UnityEngine;
using System.Collections;

public class playerscript : MonoBehaviour {

	// Use this for initialization
	/*
	private Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	float gravity =0F;
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		gravity -= 9.81F * Time.deltaTime;
		if ( controller.isGrounded )
		{
			print ("grounded");
			gravity = 0F; 
		}
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical"));
		controller.Move(moveDirection * Time.deltaTime*10);
	}
	*/
	 float speed = 1.0F;
	float jumpSpeed = 1.0F;
	 float gravity = 20.0F;
	CharacterController controller;
	public Vector3 moveDirection =new Vector3(0,0,0);
	Rigidbody rb;
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	void Update() 
	{		  
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed,ForceMode.Impulse);	
	}
}
