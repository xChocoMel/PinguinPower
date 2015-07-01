using UnityEngine;
using System.Collections;

public class GateManager : MonoBehaviour {

    public string LevelToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Penguin") {
            // TODO via menumanager
            Application.LoadLevel(LevelToLoad);
        }
    }
}
