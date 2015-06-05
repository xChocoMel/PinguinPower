using UnityEngine;
using System.Collections;

public class LeverCode : MonoBehaviour {
	public enum Status {enabled,disabled};
	// Use this for initialization
	private bool canBePressed;
	public Status status;
	public Animator animationcontroller;
	void Start () {
		animationcontroller = GetComponent<Animator>();
		status= Status.disabled;
		canBePressed = true;
	}
	//ForceMode.Impulse
	// Update is called once per frame
	void Update () 
	{
	}
	void OnCollisionEnter(Collision collision) {
		 
		if(status== Status.disabled&&canBePressed )
		{
			StartCoroutine(Wait());
			animationcontroller.SetTrigger("EnableTrigger");
			status=  Status.enabled;
	 
			 
		}
		else if(status== Status.enabled&&canBePressed)
		{
			animationcontroller.SetTrigger("DisableTrigger");
			status= Status.disabled;
			 
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
