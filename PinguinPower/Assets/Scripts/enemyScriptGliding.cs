using UnityEngine;
using System.Collections;

public class enemyScriptGliding : MonoBehaviour {

	public GameObject playerobject;

	public int routeindex;
	public GameObject[] routes;
	 
	//how far the enemy can go
	bool waiting=false;
	public AudioClip dying;
	public AudioClip hitsound;
	public AudioClip loselife;
	int amountoflives;
	private bool canBeKilled=true;
	Rigidbody enemyRigidbody;
	 
	private Animator animator;
	private bool collidingWithPlayer=false;

    public AudioClip sealClip;

    private AudioSource audioSource;
    private float minDelay = 5f;
    private float maxDelay = 20f;   

	void Start () {
		enemyRigidbody=GetComponent<Rigidbody>();
		amountoflives = 1;
		this.animator = this.GetComponentInChildren<Animator>();
		GetComponent<CapsuleCollider> ().isTrigger = true;
		GetComponent<Rigidbody> ().useGravity = false;
        this.audioSource = this.GetComponentInChildren<AudioSource>();
        StartCoroutine(PlaySealSound());
		if(playerobject==null)
		{
			playerobject=GameObject.Find ("Penguin");
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
				if(!waiting&&canBeKilled==true)
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
	void OnTriggerExit(Collider  collisionInfo) 
	{
		if (collisionInfo.gameObject.name == playerobject.name) {
			
			collidingWithPlayer=false;
		 
		}
	} 
	void AttackPenguin()
	{
		 
		waiting = true;
	 
		playerobject.GetComponent<Rigidbody>().velocity/=2F;
		playerobject.GetComponent<CharacterManager>().Damage();
		transform.LookAt (playerobject.transform.position);
		GetComponent<AudioSource>().PlayOneShot(hitsound);
		this.animator.SetTrigger("Attack");
		StartCoroutine(Wait());
	}
	IEnumerator Wait(){
		 

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
