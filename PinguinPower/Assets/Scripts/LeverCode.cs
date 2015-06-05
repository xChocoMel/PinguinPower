using UnityEngine;
using System.Collections;

public class LeverCode : MonoBehaviour {
	public enum Status {enabled,enabling,disabled,disabling};
	// Use this for initialization
//	public GameObject handle;
	public Status status= Status.disabled;

	void Start () {
	
	}
	//ForceMode.Impulse
	// Update is called once per frame
	void Update () 
	{
		//print (handle.transform.localRotation.eulerAngles .z);
		if(status== Status.enabling)
		{
		//	handle.transform.Rotate(0, 0, 80*Time.deltaTime );

			//if(handle.transform.localRotation.eulerAngles.z>180)
			//{

				status=Status.enabled;
			//}
		}
		else if(status== Status.disabling)
		{
			//handle.transform.Rotate(0, 0, -80*Time.deltaTime );
		 
			//if(handle.transform.localRotation.eulerAngles .z <90)
			//{

			status=Status.disabled;
			//}
			print (2);
		}
	}
	void OnCollisionEnter(Collision collision) {
		if(status== Status.disabled)
		{
			status= Status.enabling;
			print (3);
		}
		else if(status== Status.enabled)
		{
			status= Status.disabling;
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
}
