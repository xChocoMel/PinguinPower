using UnityEngine;
using System.Collections;

public class LeverCode : MonoBehaviour {
	public enum Status {enabled,disabled};
	// Use this for initialization
	public bool canBePressed;
	public Status status;
	  Animator animationcontroller;
	public GameObject  playerobject;
	private bool collidingwithplayer;
	void Start () {
		animationcontroller = GetComponent<Animator>();
		status= Status.disabled;
		canBePressed = true;
		if(playerobject==null){
			playerobject=GameObject.Find ("Penguin");
		}
	}
	//ForceMode.Impulse
	// Update is called once per frame
	void Update () 
	{
		if(collidingwithplayer = true)
		{
			if(playerobject.GetComponent<CharacterMovement> ().IsKicking ())
			{
				if (status == Status.disabled && canBePressed) {
					StartCoroutine (Wait ());
					animationcontroller.SetTrigger ("EnableTrigger");
					status = Status.enabled;
				
				} 
				else if (status == Status.enabled && canBePressed) 
				{
					animationcontroller.SetTrigger ("DisableTrigger");
					StartCoroutine (Wait ());
					status = Status.disabled;

					print (222);
				}
			}
		}
	}
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name==playerobject.name) 
		{
			
			collidingwithplayer = true;
		} 
	}
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.name==playerobject.name) 
		{
			
			collidingwithplayer = false;
		} 
	}
 
	public bool LeverEnabled()
	{
		if(status== Status.enabled)
		{
			return true;
		}
		return false;
	}
	public IEnumerator Wait()
	{
		canBePressed = false;
		yield return new WaitForSeconds(2);
		canBePressed = true;
	}
}
