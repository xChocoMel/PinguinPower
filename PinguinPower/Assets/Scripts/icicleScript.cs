using UnityEngine;
using System.Collections;

public class icicleScript : MonoBehaviour {
	CapsuleCollider collider;
	public int height;
	public AudioClip fallingSound;
	public AudioClip shatterSound;
	public GameObject Player;
	public float colliderheight;
	public float colliderradius;
	 
	// Use this for initialization
	void Start () {
		collider=gameObject.GetComponent<CapsuleCollider>();
		//transform.position = new Vector3 (transform.position.x,height,transform.position.z);
		collider.center = new Vector3 (0,height*-1,0);
		collider.radius = colliderradius ;
		collider.height=colliderheight;
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other) {

		 
		if(other.gameObject.name==Player.name)
		{
			print (1);
			GetComponent<AudioSource>().PlayOneShot(shatterSound);
			collider.radius = colliderradius ;
			collider.height=colliderheight;
			collider.center=new Vector3 (0,0,0);
			GetComponent<Rigidbody>().useGravity=true;
			GetComponent<CapsuleCollider>().isTrigger=false;
		}
	}
	void OnCollisionEnter(Collision other)
	{
		StartCoroutine(Shatter());
	}
	IEnumerator Shatter()
	{
		GetComponent<AudioSource>().PlayOneShot (fallingSound);
		yield return new WaitForSeconds(0.1F);
		Destroy (gameObject);
	}
}
