using UnityEngine;
using System.Collections;

public class WalkOnObjectScript : MonoBehaviour {

    private GameObject Player;
    private GameObject emptyObject;

    void Start()
    {
        Player = GameObject.Find("Penguin");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == Player.name)
        {
            emptyObject = new GameObject();
            emptyObject.transform.parent = gameObject.transform;
            collision.transform.parent = emptyObject.transform;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == Player.name)
        {
            collision.transform.parent = null;
            Destroy(emptyObject);
        }
    }
}
