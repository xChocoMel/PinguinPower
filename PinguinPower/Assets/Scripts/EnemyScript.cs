using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private GameObject playerobject;
	public enum Status {returning, attacking, patrolling ,waiting};
	private Vector3 returnPosition; 
	 
	public Status status = Status.patrolling;
	public int routeindex;
	public GameObject[] routes;

	//how far the enemy can go
	public int maxDistance = 12;

	private bool canBeKilled = true;
	private AudioClip dying;
	private AudioClip hitsound;
	private AudioClip loselife;
	private int amountoflives;
	public int sightRange = 6;
	private Rigidbody enemyRigidbody;
	 
    private Animator animator;
	private bool collidingWithPlayer = false;

    private AudioClip sealClip;

    private AudioSource audioSource;
    private float minDelay = 5f;
    private float maxDelay = 20f;    

	// Use this for initialization
	void Start () {
		getAudioClips ();
		playerobject = GameObject.FindGameObjectWithTag ("Penguin");
		enemyRigidbody = GetComponent<Rigidbody>();
		amountoflives = 1;
		returnPosition = transform.position;
        this.animator = this.GetComponentInChildren<Animator>();
        this.audioSource = this.GetComponentInChildren<AudioSource>();
        StartCoroutine(PlaySealSound());

		if(playerobject == null)
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

        if (status != Status.attacking)
        {
            audioSource.PlayOneShot(sealClip);
        }

        StartCoroutine(PlaySealSound());
    }
	
	// Update is called once per frame
	void Update () {
		if (collidingWithPlayer && playerobject.GetComponent<CharacterManager>().GetLives() >= 1)
		{
			OnCollidingWithPlayer();
		}

		if (status == Status.patrolling)
		{
			Patrolling();
		} 
		else if (status == Status.waiting)
		{
			transform.Translate(Vector3.back *0.5F* Time.deltaTime);	
		} 
		else if (status == Status.attacking){
			
			Attacking();
		}
		else if (status == Status.returning)
		{
			Returning();
		}

		if (status == Status.returning || status == Status.patrolling) 
		{	
			Vector3 directionToTarget = transform.position - playerobject.transform.position;
			float angel = Vector3.Angle(transform.forward, directionToTarget);
			float distance = Vector3.Distance (transform.position, playerobject.transform.position);

			if (distance < sightRange&&(Mathf.Abs(angel) > 90 && Mathf.Abs(angel) < 270))
			{ 				
				RaycastHit hit;

				if (Physics.Raycast(transform.position,playerobject.transform.position-transform.position,out hit, sightRange+3))
				{
					if (hit.collider.gameObject.name == playerobject.name)
					{
						if (routes.Length > 0)
						{
							returnPosition = transform.position;
						}

						status = Status.attacking;						
					} 
				}
			}
		}
	}

	void OnCollidingWithPlayer()
	{
		if (playerobject.GetComponent<CharacterMovement>().IsKicking())
		{
			if (canBeKilled)
			{
				StartCoroutine(Dying());
			}	 
		}
		else
		{
			if (status == Status.attacking && canBeKilled == true)
			{
				playerobject.GetComponent<CharacterManager>().Damage();
				transform.LookAt (new Vector3(playerobject.transform.position.x,transform.position.y,playerobject.transform.position.z));
				GetComponent<AudioSource>().PlayOneShot(hitsound);
				this.animator.SetTrigger("Attack");
				status = Status.waiting;
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

	void Returning()
	{
		Quaternion toRotation= Quaternion.LookRotation (new Vector3(returnPosition.x,transform.position.y ,returnPosition.z)- transform.position );
		transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 4 * Time.deltaTime);
 
		Moveforward(2);

		if (Vector3.Distance (transform.position, returnPosition) < 1)
		{
			status = Status.patrolling;
		}
	}

	void Attacking()
	{
		if (playerobject.GetComponent<CharacterManager>().GetLives() >= 1)
		{
			Quaternion toRotation= Quaternion.LookRotation (new Vector3(playerobject.transform.position.x,transform.position.y ,playerobject.transform.position.z)- transform.position );
			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 4 * Time.deltaTime);
			 		 
			Moveforward(4);

			if (Vector3.Distance (transform.position, returnPosition) > maxDistance)
			{
				status = Status.returning;
			}
		}
	}

	IEnumerator Wait()
	{		 
		Moveforward (0);		 
		yield return new WaitForSeconds(3.0F);
		status = Status.attacking;		 
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.name == playerobject.name) {
			collidingWithPlayer = true;
		}
	}

	void OnCollisionExit(Collision collisionInfo) 
	{
		if (collisionInfo.gameObject.name == playerobject.name) 
		{
			collidingWithPlayer = false;
		}
	}

	void OnTriggerExit(Collider  collisionInfo) 
	{
		if (collisionInfo.gameObject.name == playerobject.name) 
		{		
			collidingWithPlayer = false;
		}
	}

	void LoseLife(int attackpoint)
	{
		amountoflives -= attackpoint;

        if (amountoflives == 0)
        {
            StartCoroutine(Dying());
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(loselife);
            this.animator.SetTrigger("Damage");
        }
	}

	void Moveforward(int speed)
	{
		if(canBeKilled)
		{
			Vector3 v3 = transform.TransformDirection(Vector3.forward)* speed;
			v3.y = enemyRigidbody.velocity.y;
			enemyRigidbody.velocity = v3;
		}
	}

	IEnumerator Dying()
	{
		canBeKilled = false;
		status = Status.waiting;
		GetComponent<AudioSource>().PlayOneShot (loselife);
		this.animator.SetTrigger("Damage");
		yield return new WaitForSeconds(1.0F);
		GetComponent<AudioSource>().PlayOneShot (dying);
        this.animator.SetTrigger("Dead");
		yield return new WaitForSeconds(2.0F);
		Destroy (gameObject);
	}
}
