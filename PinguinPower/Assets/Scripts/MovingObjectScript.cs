using UnityEngine;
using System.Collections;

public class MovingObjectScript : MonoBehaviour {

	public GameObject button;
	//private LeverCode leverscript;
	public Vector3[] routes;
	public int routeindex=0;

	// Use this for initialization
	void Start () {
		/*if(button!=null)
		 {
			leverscript=button.GetComponent<LeverCode>();
		 }*/
	}
	
	// Update is called once per frame
	void Update () {

        Moving();
		/*if(leverscript==null)
		{
			Moving();
		}
		else
		{
			if(leverscript.LeverEnabled())
			{
				Moving();

			}
		}*/

	}
	void Moving()
	{
        if (routes.Length > 0)
        {
            transform.position = Vector3.Lerp(transform.position, routes[routeindex], 1 * Time.deltaTime);
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
	//	if (collision.gameObject.name == "Player") 
		//{
				//print ("collisionenter");
				//collision.transform.parent = gameObject.transform;

                var emptyObject = new GameObject();
                emptyObject.transform.parent = gameObject.transform;
                collision.transform.parent = emptyObject.transform;
		//}

	}
	void OnCollisionExit(Collision collision) 
	{
		collision.transform.parent = null;
		
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
