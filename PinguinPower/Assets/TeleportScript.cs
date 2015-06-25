using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public GameObject player;
    public GameObject camera;
    public Vector3 teleportLocation;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == player.name)
        {
            collision.gameObject.transform.position = teleportLocation;
            camera.transform.position = teleportLocation;
        }
    }
}
