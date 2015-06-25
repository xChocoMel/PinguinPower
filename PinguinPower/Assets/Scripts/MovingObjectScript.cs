using UnityEngine;
using System.Collections;

public class MovingObjectScript : MonoBehaviour
{

    public GameObject button;
    private LeverCode leverscript;
    public Vector3[] routes;
    public int routeindex = 0;
    public int movingspeed;
    public int rotatingspeed;

    public bool jumpthrough;
    public bool moveAfterTouched;
    //private bool rotating = false;
    private bool canUseRotouine = true;
    public enum RotateDirection { horizontal, vertical, circus, none };
    public RotateDirection rotatemode = RotateDirection.none;
    public GameObject Player;
    private bool canmove = true;
    public bool rotateleft;
    private bool rotatingvertical;
    private bool rotatinghorizontal;
    private bool rotatingToSlide;
    private bool rotatintBackFromSlide;
    Vector3 velocity;
    Vector3 current;
    Vector3 previous;
    public int waitrotatetime;
    public int rotatehorizontaltime;
    private float rotationleft = 180;
    private float rotationSlide = 45;
    // Use this for initialization
    void Start()
    {
        if (button != null)
        {
            leverscript = button.GetComponent<LeverCode>();
        }
        if (moveAfterTouched)
        {
            canmove = false;
        }
        if (Player == null)
        {
            Player = GameObject.Find("   ");
        }
        if (rotatehorizontaltime == 0 && rotatemode == RotateDirection.horizontal)
        {
            rotatehorizontaltime = 10;
        }

    }

    // Update is called once per frame
    void Update()
    {
        current = transform.position;
        velocity = (current - previous) / Time.deltaTime;
        previous = current;

        if (jumpthrough)
        {
            if (!rotatingvertical)
            {
                Physics.IgnoreCollision(GetComponent<BoxCollider>(), Player.GetComponent<CapsuleCollider>(), Player.transform.position.y < transform.position.y);
            }
            else
            {
                Physics.IgnoreCollision(GetComponent<BoxCollider>(), Player.GetComponent<CapsuleCollider>(), false);
            }
        }
        if (canmove)
        {

            if (canUseRotouine)
            {
                if (rotatemode == RotateDirection.horizontal || rotatemode == RotateDirection.vertical && !rotatingvertical || rotatemode == RotateDirection.circus && !rotatingToSlide && !rotatintBackFromSlide)
                {
                    StartCoroutine(rotatingEnumerator());
                }

            }


            if (leverscript == null)
            {
                Moving();
            }
            else
            {
                if (leverscript.LeverEnabled())
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
            transform.position = Vector3.MoveTowards(transform.position, routes[routeindex], movingspeed * Time.deltaTime);
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
        if (rotatingToSlide)
        {

            float rotation = rotatingspeed * Time.deltaTime;

            rotationSlide -= rotation;

            transform.Rotate(0, 0, rotation);
            if (rotationSlide <= 0)
            {
                rotatingToSlide = false;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.z = 45;
                transform.rotation = Quaternion.Euler(rotationVector);
                rotationSlide = 0;
            }

        }
        if (rotatintBackFromSlide)
        {

            float rotation = -rotatingspeed * Time.deltaTime;

            rotationSlide -= rotation;

            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).childCount > 0)
                {
                    Transform child = transform.GetChild(0).GetChild(0);
                    child.Rotate(0, 0, -rotation);
                }
            }

            transform.Rotate(0, 0, rotation);
            if (rotationSlide >= 45)
            {
                rotatintBackFromSlide = false;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.z = 0;
                transform.rotation = Quaternion.Euler(rotationVector);
                rotationSlide = 45;
            }
        }
        if (rotatingvertical)
        {

            float rotation = rotatingspeed * Time.deltaTime;

            rotationleft -= rotation;

            transform.Rotate(0, 0, rotation);
            if (rotationleft <= 0)
            {
                rotatingvertical = false;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.z = 180;
                transform.rotation = Quaternion.Euler(rotationVector);
                rotationleft = 180;
            }
        }
        if (rotatinghorizontal && rotateleft)
        {

            transform.Rotate(Vector3.up * rotatingspeed * Time.deltaTime);
        }
        else if (rotatinghorizontal && !rotateleft)
        {

            transform.Rotate(Vector3.up * rotatingspeed * -1 * Time.deltaTime);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == Player.name && rotatemode != RotateDirection.vertical)
        {
            transform.DetachChildren();
            var emptyObject = new GameObject();
            emptyObject.transform.parent = gameObject.transform;
            collision.transform.parent = emptyObject.transform;
            collision.transform.localScale = new Vector3(1, 1, 1);
            if (this.rotatemode == RotateDirection.circus)
            {
                var rotationVector = collision.transform.rotation.eulerAngles;
                rotationVector.z = 0;
                rotationVector.x = 0;
                collision.transform.rotation = Quaternion.Euler(rotationVector);
            }

            //collision.gameObject.transform.parent =gameObject.transform;
            canmove = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == Player.name)
        {
            collision.transform.localScale = new Vector3(1, 1, 1);
            collision.transform.parent = null;
            collision.transform.GetComponent<Rigidbody>().velocity += velocity;
            if (this.rotatemode == RotateDirection.circus)
            {
                var rotationVector = collision.transform.rotation.eulerAngles;
                rotationVector.z = 0;
                rotationVector.x = 0;
                collision.transform.rotation = Quaternion.Euler(rotationVector);
            }
        }

    }

    IEnumerator rotatingEnumerator()
    {
        canUseRotouine = false;
        yield return new WaitForSeconds(waitrotatetime);
        if (rotatemode == RotateDirection.horizontal)
        {
            rotatinghorizontal = true;

            yield return new WaitForSeconds(rotatehorizontaltime);
            rotatinghorizontal = false;
        }
        else if (rotatemode == RotateDirection.vertical)
        {
            rotatingvertical = true;
        }
        else if (rotatemode == RotateDirection.circus)
        {
            rotatingToSlide = true;
            rotatintBackFromSlide = false;
            yield return new WaitForSeconds(waitrotatetime);
            rotatingToSlide = false;
            rotatintBackFromSlide = true;
        }

        canUseRotouine = true;
    }
}
