using UnityEngine;
using System.Collections;

public class ChangeCameraScript : MonoBehaviour {

    private GameObject player;
    private CameraMovement maincamera;
    public float changeYValue;
    public float changeZValue;
    private float oldAimYValue;
    private float oldAimZValue;

    void OnTriggerEnter(Collider collision)
    {
		this.player = GameObject.FindGameObjectWithTag("Penguin");
		GameObject g = GameObject.FindGameObjectWithTag ("MainCamera");
		this.maincamera = g.GetComponent<CameraMovement> ();

        if (collision.gameObject.name == player.name)
        {
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
            maincamera.aimY = oldAimYValue;
            maincamera.aimZ = oldAimZValue;
        }
    }
}
