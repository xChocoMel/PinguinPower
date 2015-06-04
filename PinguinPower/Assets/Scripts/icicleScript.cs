using UnityEngine;
using System.Collections;

public class icicleScript : MonoBehaviour {
	CapsuleCollider collider;
	public int height;
	public AudioClip fallingSound;
	public AudioClip shatterSound;
	public GameObject Player;
	bool falling=false;
	// Use this for initialization
	void Start () {
		collider=gameObject.GetComponent<CapsuleCollider>();
		//transform.position = new Vector3 (transform.position.x,height,transform.position.z);
		collider.center = new Vector3 (collider.center.x,height*-1,collider.center.z);
		collider.radius = 0.4F;
		collider.height= 0.1F;
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other) {

		if(falling==true)
		{
			StartCoroutine(Shatter());
		}
		if(other.gameObject.name==Player.name)
		{
			print (1);
			GetComponent<AudioSource>().PlayOneShot(shatterSound);
			collider.radius = 0.1F;
			collider.height= 1.3F;
			collider.center=new Vector3 (0,0,0);
			GetComponent<Rigidbody>().useGravity=true;
			falling=true;
		}
	}
	IEnumerator Shatter()
	{
		GetComponent<AudioSource>().PlayOneShot (fallingSound);
		yield return new WaitForSeconds(0.1F);
		Destroy (gameObject);
	}
}
