using UnityEngine;
using System.Collections;

public class ChangeCameraScript : MonoBehaviour {

    public GameObject player;
    public CameraMovement maincamera;
    public float changeYValue;
    public float changeZValue;
    private float oldAimYValue;
    private float oldAimZValue;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == player.name)
        {
            Debug.Log("Player entered");
            oldAimYValue = maincamera.aimY;
            oldAimZValue = maincamera.aimZ;
            maincamera.aimY = changeYValue;
            maincamera.aimZ = changeZValue;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == player.name)
        {
            Debug.Log("Player lef");
            maincamera.aimY = oldAimYValue;
            maincamera.aimZ = oldAimZValue;
        }
    }
}
