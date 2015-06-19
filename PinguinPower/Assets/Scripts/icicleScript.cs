using UnityEngine;
using System.Collections;

public class icicleScript : MonoBehaviour {
	CapsuleCollider collider;
	public int height;
	public AudioClip fallingSound;
	public AudioClip shatterSound;
	public GameObject Player;
 
	//public float colliderheight;
	//public float colliderradius;
	 
	// Use this for initialization
	void Start () {
		collider=gameObject.GetComponent<CapsuleCollider>();
		//transform.position = new Vector3 (transform.position.x,height,transform.position.z);
		collider.center = new Vector3 (0,height*-1,0);
		if(Player==null)
		{
			Player=GameObject.Find ("Penguin");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other) {

		 
		if(other.gameObject.name==Player.name)
		{
			print (1);
			GetComponent<AudioSource>().PlayOneShot(shatterSound);
			GetComponent<AudioSource>().PlayOneShot (fallingSound);
			collider.center=new Vector3 (0,0,0);
			GetComponent<Rigidbody>().useGravity=true;
			GetComponent<CapsuleCollider>().isTrigger=false;
		}
	}
	void OnCollisionEnter(Collision other)
	{
		 
		foreach(Transform child in transform) {
			Destroy(child.gameObject);
		}
		GetComponent<CapsuleCollider> ().enabled = false;

		StartCoroutine(Shatter());
	}
	IEnumerator Shatter()
	{

	 
		GetComponent<Rigidbody> ().isKinematic =true;
		GetComponent<ParticleSystem> ().Play();
		yield return new WaitForSeconds(3F);
		Destroy (gameObject);
	}
}
