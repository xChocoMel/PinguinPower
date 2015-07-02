using UnityEngine;
using System.Collections;
using System;

public class LeverCode : MonoBehaviour {

	public enum Status {enabled, disabled};
	private bool canBePressed;
	public Status status = Status.disabled;
	private Animator animationcontroller;
	private GameObject player;
	public bool collidingwithplayer = false;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Penguin");
		animationcontroller = GetComponent<Animator>();

        if (status == Status.enabled)
		{
			StartCoroutine (Wait ());

            try
            {
                animationcontroller.SetTrigger("EnableTrigger");
            }
            catch (Exception) { }
		}

		canBePressed = true; 
	}

	//ForceMode.Impulse
	// Update is called once per frame
	void Update () 
	{
		if (collidingwithplayer == true)
		{
			if (player.GetComponent<CharacterMovement> ().IsKicking ())
			{
				if (status == Status.disabled && canBePressed) 
				{
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

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.name.Equals(player.name))
		{			
			collidingwithplayer = true;
		} 
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.name.Equals(player.name))
		{
			
			collidingwithplayer = false;
		} 
	}
 
	public bool LeverEnabled()
	{
		if (status == Status.enabled)
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
