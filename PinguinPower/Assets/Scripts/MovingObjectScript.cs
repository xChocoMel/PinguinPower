using UnityEngine;
using System.Collections;

public class MovingObjectScript : MonoBehaviour {

	public GameObject button;
	private LeverCode leverscript;
	public Vector3[] routes;
	public int routeindex=0;
	public int movingspeed;
	public int rotatingspeed;
	 
	public bool jumpthrough;
	public bool moveIfTouched;
	//private bool rotating = false;
	private bool canUseRotouine=true;
	public enum RotateDirection {horizontal, vertical,none};
	public RotateDirection rotatestatus= RotateDirection.none;
	public GameObject Player;
	private bool canmove=true;
	public bool rotateleft;
	private bool rotatevertical;
	private bool rotatehorizontal;
	Vector3 velocity;
	Vector3 current;
	Vector3	previous;
	public int waitrotatetime;
	public int rotatehorizontaltime;
	private float rotationleft=180;
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
		 
		if(jumpthrough)
		{
			Physics.IgnoreCollision(GetComponent<BoxCollider>(), Player.GetComponent<CapsuleCollider>(), Player.transform.position.y< transform.position.y); 
			
		}
		if(canmove){

			if(rotatestatus!=RotateDirection.none&&canUseRotouine)
			{
				StartCoroutine(rotatingEnumerator()); 

			}
			 
			 
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
			transform.position =Vector3.MoveTowards(transform.position, routes[routeindex],movingspeed*Time.deltaTime);
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
		if(rotatevertical)
		{
		 
			float rotation=rotatingspeed*Time.deltaTime;
			 
			rotationleft-=rotation;
			 
			 
			transform.Rotate(0,0,rotation);
			if(rotationleft<=0)
			{
				rotatevertical=false;
				rotationleft=180;
			}
		}
		if(rotatehorizontal&&rotateleft)
		{
			 
			transform.Rotate(Vector3.up *rotatingspeed* Time.deltaTime);
		}
		else if(rotatehorizontal&&!rotateleft)
		{
		 
			transform.Rotate(Vector3.up *rotatingspeed*-1* Time.deltaTime);
		}
	}
	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.name == Player.name) 
		{
			var emptyObject = new GameObject ();
			emptyObject.transform.parent = gameObject.transform;
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
	IEnumerator rotatingEnumerator()
	{
		canUseRotouine = false;

		if( rotatestatus== RotateDirection.horizontal)
		{
			rotatehorizontal=true;

			yield return new WaitForSeconds(rotatehorizontaltime);
			rotatehorizontal=false;
		}
		else if(rotatestatus== RotateDirection.vertical)
		{
			rotatevertical=true;
		}
		yield return new WaitForSeconds(waitrotatetime);
		canUseRotouine = true;
	}
}
