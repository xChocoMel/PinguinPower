using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    private GameObject player;
	private GameObject camera;
    public Vector3 teleportLocation;

	void Start() {
		this.player = GameObject.FindGameObjectWithTag("Penguin");
		this.camera = GameObject.FindGameObjectWithTag("MainCamera");
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == player.name)
        {
            collision.gameObject.transform.position = teleportLocation;
            camera.transform.position = teleportLocation;
        }
    }
}
