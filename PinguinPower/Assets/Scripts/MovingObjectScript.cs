using UnityEngine;
using System.Collections;

public class MovingObjectScript : MonoBehaviour {

	public GameObject button;
	private LeverCode leverscript;
	public Vector3[] routes;
	public int routeindex=0;
	public int speed;
	public int rotatingspeed;
	public bool rotateleft;
	public bool jumpthrough;
	public bool moveIfTouched;
	public GameObject Player;
	private bool canmove=true; 
	Vector3 velocity;
	Vector3 current;
	Vector3	previous;
	// Use this for initialization
	void Start () {
		 if(button!=null)
		 {
			leverscript=button.GetComponent<LeverCode>();
		 } 
		if(moveIfTouched)
		{
			canmove=false;
		}
		if(Player==null)
		{
			Player=GameObject.Find ("Penguin");
		}

	}
	
	// Update is called once per frame
	void Update () {
		current = transform.position; 
		velocity = (current - previous) / Time.deltaTime;
		previous = current;
		print (velocity.y);
		if(jumpthrough)
		{
			Physics.IgnoreCollision(GetComponent<BoxCollider>(), Player.GetComponent<CapsuleCollider>(), Player.transform.position.y< transform.position.y); 
			
		}
		if(canmove){
			if(rotateleft&&rotatingspeed!=0)
			{
				transform.Rotate(Vector3.up *rotatingspeed* Time.deltaTime);
			}
			else if(rotatingspeed!=0)
			{
				transform.Rotate(Vector3.up *-1*rotatingspeed* Time.deltaTime);
			}
	        //Moving();
			if(leverscript==null)
			{
				Moving();
			}
			else
			{
				if(leverscript.LeverEnabled())
				{
					Moving();
				}
			} 
		}
	}
	void Moving()
	{
        if (routes.Length > 0)
        {
            //transform.position = Vector3.Lerp(transform.position, routes[routeindex], 1 * Time.deltaTime);
			transform.position =Vector3.MoveTowards(transform.position, routes[routeindex],speed*Time.deltaTime);
			//GetComponent<Rigidbody>() .velocity = new Vector3(0, 10, 0);
            if (Vector3.Distance(transform.position, routes[routeindex]) < 1)
            {
                routeindex++;

            }
            if (routeindex == routes.Length)
            {
                routeindex = 0;
            }
        }
	}
	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.name == Player.name) 
		{
			//var emptyObject = new GameObject ();
			//emptyObject.transform.parent = gameObject.transform;
			//collision.transform.parent = emptyObject.transform;
			collision.gameObject.transform.parent =gameObject.transform;
			canmove=true;
		}
	}
	void OnCollisionExit(Collision collision) 
	{
		if (collision.gameObject.name == Player.name)
		{

			collision.transform.parent = null;
			collision.transform.GetComponent<Rigidbody>().velocity+=velocity;
		}
		
	}
	void OnTriggerEnter(Collider collision) 
	{
		collision.gameObject.transform.parent=gameObject.transform;

	}
	void OnTriggerExit(Collider collision) 
	{
		collision.gameObject.transform.parent = null;
		 
	}	 
}
