﻿using UnityEngine;
using System.Collections;

public class enemyScriptGliding : MonoBehaviour {

	public GameObject playerobject;

	public int routeindex;
	public GameObject[] routes;
	 
	//how far the enemy can go
	private bool waiting = false;
	private AudioClip dying;
	private AudioClip hitsound;
	private AudioClip loselife;
	private bool canBeKilled = true;
	private Rigidbody enemyRigidbody;
	 
	private Animator animator;
	private bool collidingWithPlayer = false;

	private AudioClip sealClip;

    private AudioSource audioSource;
    private float minDelay = 5f;
    private float maxDelay = 20f;   

	void Start () {
		getAudioClips ();
		enemyRigidbody = GetComponent<Rigidbody>();
		this.animator = this.GetComponentInChildren<Animator>();
		GetComponent<CapsuleCollider> ().isTrigger = true;
		GetComponent<Rigidbody> ().useGravity = false;
        this.audioSource = this.GetComponentInChildren<AudioSource>();
        StartCoroutine(PlaySealSound());

		if (playerobject == null)
		{
			playerobject = GameObject.Find ("Penguin");
		}
	}

	private void getAudioClips() {
		AudioClip[] audio = Resources.LoadAll<AudioClip>("Sounds");
		
		foreach (AudioClip a in audio) {
			if (a.name.Equals("Seal")) {
				sealClip = a;
			} else if (a.name.Equals("pinguin_collision")) {
				dying = a;
			} else if (a.name.Equals("Collision")) {
				hitsound = a;
			} else if (a.name.Equals("pinguin_collision")) {
				loselife = a;
			}
		}
	}

    private IEnumerator PlaySealSound()
    {
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        if (!waiting)
        {
            audioSource.PlayOneShot(sealClip);
        }

        StartCoroutine(PlaySealSound());
    }
	
	// Update is called once per frame
	void Update() 
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
				if(!waiting && canBeKilled)
				{
				AttackPenguin(); 
				}
			}
	}
	void Patrolling(){

		if (routes.Length > 0)
		{
			animator.SetBool("Walking", true);
			Quaternion toRotation = Quaternion.LookRotation((routes[routeindex].transform.position) - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 3 * Time.deltaTime);
			Moveforward(2);

			if (Vector3.Distance(transform.position, routes[routeindex].transform.position) < 1)
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
		if (collision.gameObject.name == playerobject.name) 
		{
			collidingWithPlayer = true;
			AttackPenguin();
		}
	}

	void OnTriggerExit(Collider collisionInfo) 
	{
		if (collisionInfo.gameObject.name == playerobject.name) {
			
			collidingWithPlayer = false;		 
		}
	} 

	void AttackPenguin()
	{
		playerobject.GetComponent<Rigidbody>().velocity /= 10;
		playerobject.GetComponent<CharacterManager>().Damage();
		transform.LookAt (playerobject.transform.position);
		GetComponent<AudioSource>().PlayOneShot(hitsound);
		this.animator.SetTrigger("Attack");	 
		StartCoroutine(Wait());
	}

	IEnumerator Wait() {	
		waiting = true;		 
		yield return new WaitForSeconds(2.0F);
		waiting = false;		
	}	 
	 
	void Moveforward(int speed)
	{
		Vector3 v3 = transform.TransformDirection(Vector3.forward)* speed;		 
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
