using UnityEngine;
using System.Collections;

public class icicleScript : MonoBehaviour {
	CapsuleCollider collider;
	public int height;
	public AudioClip fallingSound;
	public AudioClip shatterSound;
	bool falling=false;
	// Use this for initialization
	void Start () {
		collider=gameObject.GetComponent<CapsuleCollider>();
		//transform.position = new Vector3 (transform.position.x,height,transform.position.z);
		collider.center = new Vector3 (collider.center.x,height*-1,collider.center.z);
		collider.radius = 2;
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other) {

		if(falling==true)
		{
			StartCoroutine(Shatter());
		}
		if(other.gameObject.name=="Player")
		{
			audio.PlayOneShot(shatterSound);
			collider.radius = 0.5F;
			collider.center=transform.position;
			rigidbody.useGravity=true;
			falling=true;
		}
	}
	IEnumerator Shatter()
	{
		audio.PlayOneShot (fallingSound);
		yield return new WaitForSeconds(0.1F);
		Destroy (gameObject);
	}
}
