using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public GameObject playerobject;
	public enum Status {returning, attacking, patrolling ,waiting};
	Vector3 returnPosition; 
	 
	public Status status= Status.patrolling;
	public int routeindex;
	public GameObject[] routes;
	public int maxDistance;
	//how far the enemy can go

	public AudioClip dying;
	public AudioClip hitsound;
	public AudioClip loselife;
	public int amountoflives;
	public int sightRange;
	Rigidbody enemyRigidbody;
	// Use this for initialization
	void Start () {
		enemyRigidbody=GetComponent<Rigidbody>();
		amountoflives = 3;
	}
	
	// Update is called once per frame
	void Update () {
		 
		 
			if(status==Status.patrolling){
				Patrolling();
			}
			if(status==Status.waiting){
				transform.Translate(Vector3.back *0.5F* Time.deltaTime);	
			}
			if(status==Status.returning||status==Status.patrolling){	
				Vector3 directionToTarget = transform.position - playerobject.transform.position;
				float angel = Vector3.Angle(transform.forward, directionToTarget);
				float distance = Vector3.Distance (transform.position, playerobject.transform.position);
				if(distance<sightRange&&(Mathf.Abs(angel) > 90 && Mathf.Abs(angel) < 270))
				{ 
				 
					RaycastHit hit ;
				if(Physics.Raycast(transform.position,playerobject.transform.position-transform.position,out hit, sightRange+3))
					{
						if(hit.collider.gameObject.name==playerobject.name)
						{
						 
							returnPosition=transform.position;
							status= Status.attacking;
						} 
					}
				}
			}
			if(status==Status.attacking){
				
				Attacking();
			}
			if(status==Status.returning)
			{
				Returning();
			}
		 
	}
	void Patrolling(){
        if (routes.Length > 0)
        {
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
            Moveforward(0);
        }
	}
	void Returning()
	{
		Quaternion toRotation= Quaternion.LookRotation (new Vector3(returnPosition.x,transform.position.y ,returnPosition.z)- transform.position );
		transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 4 * Time.deltaTime);
 
		Moveforward(2);
		if(Vector3.Distance (transform.position, returnPosition)<1)
		{
			status=Status.patrolling;
		}
	}
	void Attacking()
	{
		Quaternion toRotation= Quaternion.LookRotation (new Vector3(playerobject.transform.position.x,transform.position.y ,playerobject.transform.position.z)- transform.position );
		transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 4 * Time.deltaTime);
		 		 
		Moveforward(4);

		if(Vector3.Distance (transform.position, returnPosition)>maxDistance)
		{
			status=Status.returning;
		}
	}
	IEnumerator Wait(){
		Moveforward (0);
		yield return new WaitForSeconds(2.0F);
		status=Status.returning;
	}
	void OnCollisionEnter(Collision collision) {
		if ( collision.gameObject.name==playerobject.name) 
		{
			if(status!=Status.waiting)
			{		 
				GetComponent<AudioSource>().PlayOneShot(hitsound);
				status=Status.waiting;
				print ("colliding");
				StartCoroutine(Wait());
			}
		}

	}

	void LoseLife(int attackpoint)
	{
		GetComponent<AudioSource>().PlayOneShot (loselife);
		amountoflives-=attackpoint;
		if(amountoflives<1)
		{
			StartCoroutine(Dying());
		}
	}
	void Moveforward(int speed)
	{
		Vector3 v3 = transform.TransformDirection(Vector3.forward)* speed;
		v3.y = enemyRigidbody.velocity.y;
		enemyRigidbody.velocity = v3;
	}
	IEnumerator Dying(){
		GetComponent<AudioSource>().PlayOneShot (dying);
		yield return new WaitForSeconds(3.0F);
		Destroy (gameObject);
	}
}
