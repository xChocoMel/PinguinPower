using UnityEngine;
using System.Collections;

public class LeverCode : MonoBehaviour {
	private enum Status {enabled,disabled};
	// Use this for initialization
	private bool canBePressed;
	private Status status = Status.disabled;
	  Animator animationcontroller;
	public GameObject  playerobject;
	public bool collidingwithplayer = false;
	void Start () {
		animationcontroller = GetComponent<Animator>();
        if (status == null)
        {
            status = Status.disabled;
        }
		canBePressed = true;
		if(playerobject==null){
			playerobject=GameObject.Find ("Penguin");
		}
	 
	}
	//ForceMode.Impulse
	// Update is called once per frame
	void Update () 
	{
		if(collidingwithplayer == true)
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
