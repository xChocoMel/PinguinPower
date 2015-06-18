using UnityEngine;
using System.Collections;

public class enemyScriptGliding : MonoBehaviour {

	public GameObject playerobject;

	public int routeindex;
	public GameObject[] routes;
	 
	//how far the enemy can go
	bool waiting;
	public AudioClip dying;
	public AudioClip hitsound;
	public AudioClip loselife;
	int amountoflives;
	private bool canBeKilled=true;
	Rigidbody enemyRigidbody;
	 
	private Animator animator;
	private bool collidingWithPlayer=false;
	void Start () {
		enemyRigidbody=GetComponent<Rigidbody>();
		amountoflives = 1;
		this.animator = this.GetComponentInChildren<Animator>();
		GetComponent<CapsuleCollider> ().isTrigger = true;
		GetComponent<Rigidbody> ().useGravity = false;
		if(playerobject==null)
		{
			playerobject=GameObject.Find ("Penguin");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
			Patrolling();
			if(collidingWithPlayer)
			{
				OnCollidingWithPlayer();
			}
	}
	void OnCollidingWithPlayer()
	{	 
			if(playerobject.GetComponent<CharacterMovement>().IsKicking())
			{
				if(canBeKilled)
				{
					StartCoroutine(Dying());
					 
				}
			}
			else
			{
				if(!waiting&&canBeKilled==true)
				{
					playerobject.GetComponent<Rigidbody>().velocity/=4;
					playerobject.GetComponent<CharacterManager>().Damage();
					transform.LookAt (playerobject.transform.position);
					GetComponent<AudioSource>().PlayOneShot(hitsound);
					this.animator.SetTrigger("Attack");
					print ("colliding");
					StartCoroutine(Wait());
				}
			}
		 
		
	}
	void Patrolling(){
		if (routes.Length > 0)
		{
			animator.SetBool("Walking", true);
			Quaternion toRotation = Quaternion.LookRotation(new Vector3(routes[routeindex].transform.position.x, transform.position.y, routes[routeindex].transform.position.z) - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 3 * Time.deltaTime);
			Moveforward(2);
			if (Vector3.Distance(transform.position, new Vector3(routes[routeindex].transform.position.x, transform.position.y, routes[routeindex].transform.position.z)) < 1)
			{
				routeindex++;
			}
			if (routeindex == routes.Length)
			{
				routeindex = 0;
			}
		}
		else
		{
			animator.SetBool("Walking", false);
			Moveforward(0);
		}
	}
	void OnTriggerEnter(Collider  collision) {
		if (collision.gameObject.name == playerobject.name) {
			collidingWithPlayer = true;
			print (222);
			
		}
		
	}
	void OnTriggerExit(Collider  collisionInfo) 
	{
		if (collisionInfo.gameObject.name == playerobject.name) {
			
			collidingWithPlayer=false;
		}
	} 
	IEnumerator Wait(){
		 
		waiting = true;
		yield return new WaitForSeconds(2.0F);
		waiting = false;
		
	}
	 
	 
	void Moveforward(int speed)
	{
		Vector3 v3 = transform.TransformDirection(Vector3.forward)* speed;
		v3.y = enemyRigidbody.velocity.y;
		enemyRigidbody.velocity = v3;
	}
	IEnumerator Dying(){
		waiting = true;
		canBeKilled = false;
		GetComponent<AudioSource>().PlayOneShot (loselife);
		this.animator.SetTrigger("Damage");
		yield return new WaitForSeconds(1.0F);
		GetComponent<AudioSource>().PlayOneShot (dying);
		this.animator.SetTrigger("Dead");
		yield return new WaitForSeconds(1.0F);
		Destroy (gameObject);
	}
}
